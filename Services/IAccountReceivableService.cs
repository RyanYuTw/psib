using PSIB.Models;

namespace PSIB.Services;

public interface IAccountReceivableService
{
    Task<List<AccountReceivable>> GetAllAsync(DateTime? from = null, DateTime? to = null, string? customerId = null);
    Task<AccountReceivable?> GetByIdAsync(string id);
    Task<string> GenerateNewIdAsync();
    Task AddAsync(AccountReceivable ar);
    Task UpdateAsync(AccountReceivable ar);
    Task<decimal> GetCustomerBalanceAsync(string customerId);
}
