using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Services;

namespace PSIB.ViewModels;

public partial class MainMenuViewModel : BaseViewModel
{
    private readonly IDashboardService _dashboardService;
    private readonly IAuthService _authService;

    [ObservableProperty] private decimal _todaySales;
    [ObservableProperty] private decimal _monthSales;
    [ObservableProperty] private decimal _todayPurchases;
    [ObservableProperty] private decimal _monthPurchases;
    [ObservableProperty] private int _pendingPayables;
    [ObservableProperty] private int _pendingReceivables;
    [ObservableProperty] private int _lowStockCount;
    [ObservableProperty] private string _userName = string.Empty;
    public string TodayDate => DateTime.Today.ToString("yyyy/MM/dd");

    public MainMenuViewModel(IDashboardService dashboardService, IAuthService authService)
    {
        _dashboardService = dashboardService;
        _authService = authService;
        Title = "首頁";
        UserName = authService.CurrentUser?.Name ?? "";
    }

    [RelayCommand]
    private async Task LoadDashboardAsync()
    {
        IsBusy = true;
        try
        {
            var summary = await _dashboardService.GetSummaryAsync();
            TodaySales = summary.TodaySales;
            MonthSales = summary.MonthSales;
            TodayPurchases = summary.TodayPurchases;
            MonthPurchases = summary.MonthPurchases;
            PendingPayables = summary.PendingPayables;
            PendingReceivables = summary.PendingReceivables;
            LowStockCount = summary.LowStockCount;
        }
        catch (Exception ex)
        {
            SetError(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand] private Task GoToProducts() => Shell.Current.GoToAsync("//ProductList");
    [RelayCommand] private Task GoToCustomers() => Shell.Current.GoToAsync("//CustomerList");
    [RelayCommand] private Task GoToVendors() => Shell.Current.GoToAsync("//VendorList");
    [RelayCommand] private Task GoToPurchases() => Shell.Current.GoToAsync("//PurchaseList");
    [RelayCommand] private Task GoToSales() => Shell.Current.GoToAsync("//SaleList");
    [RelayCommand] private Task GoToAccountPayables() => Shell.Current.GoToAsync("//AccountPayableList");
    [RelayCommand] private Task GoToAccountReceivables() => Shell.Current.GoToAsync("//AccountReceivableList");

    [RelayCommand]
    private async Task LogoutAsync()
    {
        var confirm = await Shell.Current.DisplayAlert("登出", "確定要登出嗎？", "確定", "取消");
        if (confirm)
        {
            _authService.Logout();
            await Shell.Current.GoToAsync("//Login");
        }
    }
}
