using Microsoft.EntityFrameworkCore;
using PSIB.Data;
using PSIB.Models;

namespace PSIB.Services;

public class AuthService : IAuthService
{
    private readonly IServiceProvider _serviceProvider;
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
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = await db.Users
            .Include(u => u.UserGroup)
            .Where(u => u.UserId == userId && u.IsActive)
            .FirstOrDefaultAsync();

        if (user == null) return false;

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password)) return false;

        _currentUser = user;
        _currentGroup = user.UserGroup;
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
