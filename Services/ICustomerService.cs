using PSIB.Models;

namespace PSIB.Services;

public interface ICustomerService
{
    Task<List<Customer>> GetAllAsync(string? keyword = null);
    Task<Customer?> GetByIdAsync(string id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(string id);
    Task<string> GenerateNewIdAsync();
}
