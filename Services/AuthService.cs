using Microsoft.EntityFrameworkCore;
using PSIB.Data;
using PSIB.Models;
using System.Collections.Concurrent;

namespace PSIB.Services;

public class AuthService : IAuthService
{
    private const int MaxFailedAttempts = 5;
    private const int LockoutMinutes = 15;

    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, (int Count, DateTime Until)> _failedAttempts = new();
    private User? _currentUser;
    private UserGroup? _currentGroup;

    public AuthService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public User? CurrentUser => _currentUser;
    public bool IsLoggedIn => _currentUser != null;

    public async Task<bool> LoginAsync(string userId, string password)
    {
        var key = userId.ToLowerInvariant();

        if (_failedAttempts.TryGetValue(key, out var entry) &&
            entry.Count >= MaxFailedAttempts &&
            DateTime.UtcNow < entry.Until)
        {
            var remaining = (int)(entry.Until - DateTime.UtcNow).TotalMinutes + 1;
            throw new InvalidOperationException($"帳號已暫時鎖定，請 {remaining} 分鐘後再試");
        }

        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = await db.Users
            .Include(u => u.UserGroup)
            .Where(u => u.UserId == userId && u.IsActive)
            .FirstOrDefaultAsync();

        bool valid = user != null && BCrypt.Net.BCrypt.Verify(password, user.Password);

        if (!valid)
        {
            _failedAttempts.AddOrUpdate(key,
                _ => (1, DateTime.UtcNow.AddMinutes(LockoutMinutes)),
                (_, prev) =>
                {
                    int count = prev.Count + 1;
                    return (count, DateTime.UtcNow.AddMinutes(LockoutMinutes));
                });
            return false;
        }

        _failedAttempts.TryRemove(key, out _);
        _currentUser = user;
        _currentGroup = user!.UserGroup;
        return true;
    }

    public void Logout()
    {
        _currentUser = null;
        _currentGroup = null;
    }

    public bool HasPermission(string permission) => permission switch
    {
        "sale" => _currentGroup?.CanSale ?? false,
        "purchase" => _currentGroup?.CanPurchase ?? false,
        "report" => _currentGroup?.CanReport ?? false,
        "setting" => _currentGroup?.CanSetting ?? false,
        "user_mgmt" => _currentGroup?.CanUserMgmt ?? false,
        _ => false
    };
}
