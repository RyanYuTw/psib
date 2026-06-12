using Microsoft.EntityFrameworkCore;
using PSIB.Data;
using PSIB.Models;

namespace PSIB.Services;

public class PurchaseService : IPurchaseService
{
    private readonly AppDbContext _db;

    public PurchaseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Purchase>> GetAllAsync(DateTime? from = null, DateTime? to = null, string? vendorId = null)
    {
        var query = _db.Purchases.Include(p => p.Vendor).AsQueryable();
        if (from.HasValue) query = query.Where(p => p.PurchaseDate >= from.Value);
        if (to.HasValue) query = query.Where(p => p.PurchaseDate <= to.Value.AddDays(1));
        if (!string.IsNullOrEmpty(vendorId)) query = query.Where(p => p.VendorId == vendorId);
        return await query.OrderByDescending(p => p.PurchaseDate).ToListAsync();
    }

    public async Task<Purchase?> GetByIdAsync(string id) =>
        await _db.Purchases
            .Include(p => p.Vendor)
            .Include(p => p.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<string> GenerateNewIdAsync()
    {
        var today = DateTime.Today.ToString("yyyyMMdd");
        var max = await _db.Purchases.IgnoreQueryFilters()
            .Where(p => p.Id.StartsWith(today))
            .MaxAsync(p => (string?)p.Id);
        if (max == null) return $"{today}0001";
        if (int.TryParse(max[8..], out int seq))
            return $"{today}{seq + 1:D4}";
        return $"{today}0001";
    }

    public async Task AddAsync(Purchase purchase)
    {
        _db.Purchases.Add(purchase);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Purchase purchase)
    {
        purchase.UpdatedAt = DateTime.Now;
        _db.Purchases.Update(purchase);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var purchase = await _db.Purchases.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
        if (purchase != null)
        {
            purchase.Deleted = true;
            await _db.SaveChangesAsync();
        }
    }
}
