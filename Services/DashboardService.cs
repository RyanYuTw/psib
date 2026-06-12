using Microsoft.EntityFrameworkCore;
using PSIB.Data;

namespace PSIB.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardSummary> GetSummaryAsync()
    {
        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);

        var todaySales = await _db.Sales
            .Where(s => s.SaleDate.Date == today)
            .SumAsync(s => (decimal?)s.Total) ?? 0;

        var monthSales = await _db.Sales
            .Where(s => s.SaleDate >= monthStart)
            .SumAsync(s => (decimal?)s.Total) ?? 0;

        var todayPurchases = await _db.Purchases
            .Where(p => p.PurchaseDate.Date == today)
            .SumAsync(p => (decimal?)p.Total) ?? 0;

        var monthPurchases = await _db.Purchases
            .Where(p => p.PurchaseDate >= monthStart)
            .SumAsync(p => (decimal?)p.Total) ?? 0;

        var pendingPayables = await _db.AccountPayables
            .CountAsync(ap => ap.TotalBalance > 0);

        var pendingReceivables = await _db.AccountReceivables
            .CountAsync(ar => ar.TotalBalance > 0);

        var lowStockCount = await _db.Products
            .Where(p => p.IsActive && p.Stock && p.CurrentVol <= p.SafeVol)
            .CountAsync();

        // 近6個月月報
        var sixMonthsAgo = monthStart.AddMonths(-5);
        var salesByMonth = await _db.Sales
            .Where(s => s.SaleDate >= sixMonthsAgo)
            .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Total = g.Sum(s => s.Total) })
            .ToListAsync();

        var purchasesByMonth = await _db.Purchases
            .Where(p => p.PurchaseDate >= sixMonthsAgo)
            .GroupBy(p => new { p.PurchaseDate.Year, p.PurchaseDate.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Total = g.Sum(p => p.Total) })
            .ToListAsync();

        var monthlySummary = Enumerable.Range(0, 6)
            .Select(i => monthStart.AddMonths(-5 + i))
            .Select(m =>
            {
                var label = m.ToString("yyyy/MM");
                var s = salesByMonth.FirstOrDefault(x => x.Year == m.Year && x.Month == m.Month)?.Total ?? 0;
                var p = purchasesByMonth.FirstOrDefault(x => x.Year == m.Year && x.Month == m.Month)?.Total ?? 0;
                return (label, s, p);
            })
            .ToList();

        return new DashboardSummary(
            todaySales, monthSales,
            todayPurchases, monthPurchases,
            pendingPayables, pendingReceivables,
            lowStockCount, monthlySummary);
    }
}
