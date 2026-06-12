using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

public partial class VendorListViewModel : BaseViewModel
{
    private readonly IVendorService _vendorService;

    [ObservableProperty] private ObservableCollection<Vendor> _vendors = new();
    [ObservableProperty] private string _searchKeyword = string.Empty;

    public VendorListViewModel(IVendorService vendorService)
    {
        _vendorService = vendorService;
        Title = "廠商管理";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _vendorService.GetAllAsync(SearchKeyword);
            Vendors = new ObservableCollection<Vendor>(list);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand] private async Task SearchAsync() => await LoadAsync();

    [RelayCommand]
    private async Task AddAsync() =>
        await Shell.Current.GoToAsync("VendorDetail", new Dictionary<string, object> { ["VendorId"] = "" });

    [RelayCommand]
    private async Task EditAsync(Vendor vendor) =>
        await Shell.Current.GoToAsync("VendorDetail", new Dictionary<string, object> { ["VendorId"] = vendor.Id });

    [RelayCommand]
    private async Task DeleteAsync(Vendor vendor)
    {
        var confirm = await Shell.Current.DisplayAlert("刪除確認", $"確定要刪除廠商「{vendor.Name}」？", "確定", "取消");
        if (!confirm) return;
        await _vendorService.DeleteAsync(vendor.Id);
        Vendors.Remove(vendor);
    }
}
