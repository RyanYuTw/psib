using Microsoft.EntityFrameworkCore;
using PSIB.Data;
using PSIB.Models;

namespace PSIB.Services;

public class SaleService : ISaleService
{
    private readonly AppDbContext _db;

    public SaleService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Sale>> GetAllAsync(DateTime? from = null, DateTime? to = null, string? customerId = null)
    {
        var query = _db.Sales.Include(s => s.Customer).AsQueryable();
        if (from.HasValue) query = query.Where(s => s.SaleDate >= from.Value);
        if (to.HasValue) query = query.Where(s => s.SaleDate <= to.Value.AddDays(1));
        if (!string.IsNullOrEmpty(customerId)) query = query.Where(s => s.CustomerId == customerId);
        return await query.OrderByDescending(s => s.SaleDate).ToListAsync();
    }

    public async Task<Sale?> GetByIdAsync(string id) =>
        await _db.Sales
            .Include(s => s.Customer)
            .Include(s => s.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<string> GenerateNewIdAsync()
    {
        var today = DateTime.Today.ToString("yyyyMMdd");
        var max = await _db.Sales.IgnoreQueryFilters()
            .Where(s => s.Id.StartsWith(today))
            .MaxAsync(s => (string?)s.Id);
        if (max == null) return $"{today}0001";
        if (int.TryParse(max[8..], out int seq))
            return $"{today}{seq + 1:D4}";
        return $"{today}0001";
    }

    public async Task AddAsync(Sale sale)
    {
        _db.Sales.Add(sale);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        sale.UpdatedAt = DateTime.Now;
        _db.Sales.Update(sale);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var sale = await _db.Sales.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Id == id);
        if (sale != null)
        {
            sale.Deleted = true;
            await _db.SaveChangesAsync();
        }
    }
}
