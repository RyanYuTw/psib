using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

public partial class SaleListViewModel : BaseViewModel
{
    private readonly ISaleService _saleService;

    [ObservableProperty] private ObservableCollection<Sale> _sales = new();
    [ObservableProperty] private DateTime _fromDate = DateTime.Today.AddMonths(-1);
    [ObservableProperty] private DateTime _toDate = DateTime.Today;

    public SaleListViewModel(ISaleService saleService)
    {
        _saleService = saleService;
        Title = "銷售管理";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _saleService.GetAllAsync(FromDate, ToDate);
            Sales = new ObservableCollection<Sale>(list);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task AddAsync() =>
        await Shell.Current.GoToAsync("SaleDetail", new Dictionary<string, object> { ["SaleId"] = "" });

    [RelayCommand]
    private async Task EditAsync(Sale sale) =>
        await Shell.Current.GoToAsync("SaleDetail", new Dictionary<string, object> { ["SaleId"] = sale.Id });

    [RelayCommand]
    private async Task DeleteAsync(Sale sale)
    {
        var confirm = await Shell.Current.DisplayAlert("刪除確認", $"確定要刪除銷售單「{sale.Id}」？", "確定", "取消");
        if (!confirm) return;
        await _saleService.DeleteAsync(sale.Id);
        Sales.Remove(sale);
    }
}
