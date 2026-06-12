using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

public partial class PurchaseListViewModel : BaseViewModel
{
    private readonly IPurchaseService _purchaseService;

    [ObservableProperty] private ObservableCollection<Purchase> _purchases = new();
    [ObservableProperty] private DateTime _fromDate = DateTime.Today.AddMonths(-1);
    [ObservableProperty] private DateTime _toDate = DateTime.Today;

    public PurchaseListViewModel(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
        Title = "採購管理";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _purchaseService.GetAllAsync(FromDate, ToDate);
            Purchases = new ObservableCollection<Purchase>(list);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task AddAsync() =>
        await Shell.Current.GoToAsync("PurchaseDetail", new Dictionary<string, object> { ["PurchaseId"] = "" });

    [RelayCommand]
    private async Task EditAsync(Purchase purchase) =>
        await Shell.Current.GoToAsync("PurchaseDetail", new Dictionary<string, object> { ["PurchaseId"] = purchase.Id });

    [RelayCommand]
    private async Task DeleteAsync(Purchase purchase)
    {
        var confirm = await Shell.Current.DisplayAlert("刪除確認", $"確定要刪除採購單「{purchase.Id}」？", "確定", "取消");
        if (!confirm) return;
        await _purchaseService.DeleteAsync(purchase.Id);
        Purchases.Remove(purchase);
    }
}
