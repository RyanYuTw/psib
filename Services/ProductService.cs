using Microsoft.EntityFrameworkCore;
using PSIB.Data;
using PSIB.Models;

namespace PSIB.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetAllAsync(string? keyword = null)
    {
        var query = _db.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(p => p.Name.Contains(keyword) || (p.Barcode != null && p.Barcode.Contains(keyword)));

        return await query.OrderBy(p => p.Name).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(string id) =>
        await _db.Products.Include(p => p.Category).Include(p => p.Unit).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product?> GetByBarcodeAsync(string barcode) =>
        await _db.Products.FirstOrDefaultAsync(p => p.Barcode == barcode);

    public async Task AddAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        product.LastBuyDate = product.LastBuyDate;
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            product.IsActive = false;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Product>> GetLowStockAsync() =>
        await _db.Products
            .Include(p => p.WarehouseStocks)
            .Where(p => p.IsActive && p.Stock)
            .Where(p => p.CurrentVol <= p.SafeVol)
            .ToListAsync();
}
