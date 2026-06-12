using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Extensions;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

[QueryProperty(nameof(PurchaseId), "PurchaseId")]
public partial class PurchaseDetailViewModel : BaseViewModel
{
    private readonly IPurchaseService _purchaseService;
    private readonly IVendorService _vendorService;

    [ObservableProperty] private string _purchaseId = string.Empty;
    [ObservableProperty] private string _id = string.Empty;
    [ObservableProperty] private DateTime _purchaseDate = DateTime.Today;
    [ObservableProperty] private string _vendorId = string.Empty;
    [ObservableProperty] private string _vendorName = string.Empty;
    [ObservableProperty] private decimal _subTotal;
    [ObservableProperty] private decimal _tax;
    [ObservableProperty] private decimal _total;
    [ObservableProperty] private string _memo = string.Empty;
    [ObservableProperty] private bool _isNewRecord;
    [ObservableProperty] private ObservableCollection<PurchaseDetail> _details = new();
    [ObservableProperty] private ObservableCollection<Vendor> _vendors = new();
    [ObservableProperty] private Vendor? _selectedVendor;

    partial void OnSelectedVendorChanged(Vendor? vendor)
    {
        if (vendor != null) { VendorId = vendor.Id; VendorName = vendor.Name; }
    }

    public PurchaseDetailViewModel(IPurchaseService purchaseService, IVendorService vendorService)
    {
        _purchaseService = purchaseService;
        _vendorService = vendorService;
    }

    partial void OnPurchaseIdChanged(string value) => _ = LoadAsync(value);

    private async Task LoadAsync(string id)
    {
        IsBusy = true;
        try
        {
            var vendorList = await _vendorService.GetAllAsync();
            Vendors = new ObservableCollection<Vendor>(vendorList);

            if (string.IsNullOrEmpty(id))
            {
                IsNewRecord = true;
                Title = "新增採購單";
                Id = await _purchaseService.GenerateNewIdAsync();
                return;
            }

            var purchase = await _purchaseService.GetByIdAsync(id);
            if (purchase != null)
            {
                IsNewRecord = false;
                Title = $"採購單 {id}";
                Id = purchase.Id;
                PurchaseDate = purchase.PurchaseDate;
                VendorId = purchase.VendorId;
                VendorName = purchase.Vendor?.Name ?? "";
                SubTotal = purchase.SubTotal;
                Tax = purchase.Tax;
                Total = purchase.Total;
                Memo = purchase.Memo ?? "";
                Details = new ObservableCollection<PurchaseDetail>(purchase.Details);
                SelectedVendor = Vendors.FirstOrDefault(v => v.Id == VendorId);
            }
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    private void RecalcTotal()
    {
        SubTotal = Details.Sum(d => d.LineTotal);
        Tax = Math.Round(SubTotal * 0.05m, 0);
        Total = SubTotal + Tax;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrEmpty(VendorId)) { SetError("請選擇廠商"); return; }
        IsBusy = true;
        try
        {
            var purchase = new Purchase
            {
                Id = Id, PurchaseDate = PurchaseDate, VendorId = VendorId,
                CurrId = "TWD", ExcRate = 1, TaxRate = 5,
                SubTotal = SubTotal, Tax = Tax, Total = Total, Paid = Total,
                Memo = Memo.NullIfEmpty(), Deleted = false,
                Details = Details.ToList()
            };
            if (IsNewRecord) await _purchaseService.AddAsync(purchase);
            else await _purchaseService.UpdateAsync(purchase);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand] private async Task CancelAsync() => await Shell.Current.GoToAsync("..");
}
