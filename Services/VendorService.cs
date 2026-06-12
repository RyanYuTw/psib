using Microsoft.EntityFrameworkCore;
using PSIB.Data;
using PSIB.Models;

namespace PSIB.Services;

public class VendorService : IVendorService
{
    private readonly AppDbContext _db;

    public VendorService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Vendor>> GetAllAsync(string? keyword = null)
    {
        var query = _db.Vendors.Where(v => v.IsActive);
        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(v => v.Name.Contains(keyword) || (v.Phone != null && v.Phone.Contains(keyword)));
        return await query.OrderBy(v => v.Name).ToListAsync();
    }

    public async Task<Vendor?> GetByIdAsync(string id) =>
        await _db.Vendors.FirstOrDefaultAsync(v => v.Id == id);

    public async Task AddAsync(Vendor vendor)
    {
        _db.Vendors.Add(vendor);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vendor vendor)
    {
        _db.Vendors.Update(vendor);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var vendor = await _db.Vendors.FindAsync(id);
        if (vendor != null)
        {
            vendor.IsActive = false;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<string> GenerateNewIdAsync()
    {
        var max = await _db.Vendors.MaxAsync(v => (string?)v.Id);
        if (max == null) return "V0001";
        if (int.TryParse(max[1..], out int num))
            return $"V{num + 1:D4}";
        return $"V{_db.Vendors.Count() + 1:D4}";
    }
}
