using Microsoft.EntityFrameworkCore;
using PSIB.Data;
using PSIB.Models;

namespace PSIB.Services;

public class AccountPayableService : IAccountPayableService
{
    private readonly AppDbContext _db;

    public AccountPayableService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<AccountPayable>> GetAllAsync(DateTime? from = null, DateTime? to = null, string? vendorId = null)
    {
        var query = _db.AccountPayables.Include(ap => ap.Vendor).Include(ap => ap.Purchase).AsQueryable();
        if (from.HasValue) query = query.Where(ap => ap.PayDate >= from.Value);
        if (to.HasValue) query = query.Where(ap => ap.PayDate <= to.Value.AddDays(1));
        if (!string.IsNullOrEmpty(vendorId)) query = query.Where(ap => ap.VendorId == vendorId);
        return await query.OrderByDescending(ap => ap.CreatedAt).ToListAsync();
    }

    public async Task<AccountPayable?> GetByIdAsync(string id) =>
        await _db.AccountPayables.Include(ap => ap.Vendor).Include(ap => ap.Purchase).FirstOrDefaultAsync(ap => ap.Id == id);

    public async Task<string> GenerateNewIdAsync()
    {
        var today = DateTime.Today.ToString("yyyyMMdd");
        var max = await _db.AccountPayables.Where(ap => ap.Id.StartsWith("AP" + today)).MaxAsync(ap => (string?)ap.Id);
        if (max == null) return $"AP{today}0001";
        if (int.TryParse(max[10..], out int seq))
            return $"AP{today}{seq + 1:D4}";
        return $"AP{today}0001";
    }

    public async Task AddAsync(AccountPayable ap)
    {
        _db.AccountPayables.Add(ap);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(AccountPayable ap)
    {
        _db.AccountPayables.Update(ap);
        await _db.SaveChangesAsync();
    }

    public async Task<decimal> GetVendorBalanceAsync(string vendorId) =>
        await _db.AccountPayables.Where(ap => ap.VendorId == vendorId).SumAsync(ap => ap.TotalBalance);
}
