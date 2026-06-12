using PSIB.Models;

namespace PSIB.Services;

public interface IVendorService
{
    Task<List<Vendor>> GetAllAsync(string? keyword = null);
    Task<Vendor?> GetByIdAsync(string id);
    Task AddAsync(Vendor vendor);
    Task UpdateAsync(Vendor vendor);
    Task DeleteAsync(string id);
    Task<string> GenerateNewIdAsync();
}
