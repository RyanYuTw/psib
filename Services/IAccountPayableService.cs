using PSIB.Models;

namespace PSIB.Services;

public interface IAccountPayableService
{
    Task<List<AccountPayable>> GetAllAsync(DateTime? from = null, DateTime? to = null, string? vendorId = null);
    Task<AccountPayable?> GetByIdAsync(string id);
    Task<string> GenerateNewIdAsync();
    Task AddAsync(AccountPayable ap);
    Task UpdateAsync(AccountPayable ap);
    Task<decimal> GetVendorBalanceAsync(string vendorId);
}
