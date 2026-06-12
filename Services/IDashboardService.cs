namespace PSIB.Services;

public record DashboardSummary(
    decimal TodaySales,
    decimal MonthSales,
    decimal TodayPurchases,
    decimal MonthPurchases,
    int PendingPayables,
    int PendingReceivables,
    int LowStockCount,
    List<(string Month, decimal Sales, decimal Purchases)> MonthlySummary
);

public interface IDashboardService
{
    Task<DashboardSummary> GetSummaryAsync();
}
