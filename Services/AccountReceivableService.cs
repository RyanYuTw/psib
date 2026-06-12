using Microsoft.EntityFrameworkCore;
using PSIB.Data;
using PSIB.Models;

namespace PSIB.Services;

public class AccountReceivableService : IAccountReceivableService
{
    private readonly AppDbContext _db;

    public AccountReceivableService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<AccountReceivable>> GetAllAsync(DateTime? from = null, DateTime? to = null, string? customerId = null)
    {
        var query = _db.AccountReceivables.Include(ar => ar.Customer).Include(ar => ar.Sale).AsQueryable();
        if (from.HasValue) query = query.Where(ar => ar.ReceiveDate >= from.Value);
        if (to.HasValue) query = query.Where(ar => ar.ReceiveDate <= to.Value.AddDays(1));
        if (!string.IsNullOrEmpty(customerId)) query = query.Where(ar => ar.CustomerId == customerId);
        return await query.OrderByDescending(ar => ar.CreatedAt).ToListAsync();
    }

    public async Task<AccountReceivable?> GetByIdAsync(string id) =>
        await _db.AccountReceivables.Include(ar => ar.Customer).Include(ar => ar.Sale).FirstOrDefaultAsync(ar => ar.Id == id);

    public async Task<string> GenerateNewIdAsync()
    {
        var today = DateTime.Today.ToString("yyyyMMdd");
        var max = await _db.AccountReceivables.Where(ar => ar.Id.StartsWith("AR" + today)).MaxAsync(ar => (string?)ar.Id);
        if (max == null) return $"AR{today}0001";
        if (int.TryParse(max[10..], out int seq))
            return $"AR{today}{seq + 1:D4}";
        return $"AR{today}0001";
    }

    public async Task AddAsync(AccountReceivable ar)
    {
        _db.AccountReceivables.Add(ar);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(AccountReceivable ar)
    {
        _db.AccountReceivables.Update(ar);
        await _db.SaveChangesAsync();
    }

    public async Task<decimal> GetCustomerBalanceAsync(string customerId) =>
        await _db.AccountReceivables.Where(ar => ar.CustomerId == customerId).SumAsync(ar => ar.TotalBalance);
}
