using PSIB.Models;

namespace PSIB.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync(string? keyword = null);
    Task<Product?> GetByIdAsync(string id);
    Task<Product?> GetByBarcodeAsync(string barcode);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(string id);
    Task<List<Product>> GetLowStockAsync();
}
