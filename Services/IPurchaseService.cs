using PSIB.Models;

namespace PSIB.Services;

public interface IPurchaseService
{
    Task<List<Purchase>> GetAllAsync(DateTime? from = null, DateTime? to = null, string? vendorId = null);
    Task<Purchase?> GetByIdAsync(string id);
    Task<string> GenerateNewIdAsync();
    Task AddAsync(Purchase purchase);
    Task UpdateAsync(Purchase purchase);
    Task DeleteAsync(string id);
}
