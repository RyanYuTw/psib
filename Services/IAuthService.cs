using PSIB.Models;

namespace PSIB.Services;

public interface IAuthService
{
    User? CurrentUser { get; }
    bool IsLoggedIn { get; }
    Task<bool> LoginAsync(string userId, string password);
    void Logout();
    bool HasPermission(string permission);
}
