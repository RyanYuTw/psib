using PSIB.Models;

namespace PSIB.Services;

public interface ISaleService
{
    Task<List<Sale>> GetAllAsync(DateTime? from = null, DateTime? to = null, string? customerId = null);
    Task<Sale?> GetByIdAsync(string id);
    Task<string> GenerateNewIdAsync();
    Task AddAsync(Sale sale);
    Task UpdateAsync(Sale sale);
    Task DeleteAsync(string id);
}
