using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Extensions;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

[QueryProperty(nameof(VendorId), "VendorId")]
public partial class VendorDetailViewModel : BaseViewModel
{
    private readonly IVendorService _vendorService;

    [ObservableProperty] private string _vendorId = string.Empty;
    [ObservableProperty] private string _id = string.Empty;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _businessNo = string.Empty;
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private string _phone = string.Empty;
    [ObservableProperty] private string _cell = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _contact = string.Empty;
    [ObservableProperty] private decimal _creditLimit;
    [ObservableProperty] private string _memo = string.Empty;
    [ObservableProperty] private bool _isNewRecord;

    public VendorDetailViewModel(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    partial void OnVendorIdChanged(string value) => _ = LoadAsync(value);

    private async Task LoadAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            IsNewRecord = true;
            Title = "新增廠商";
            Id = await _vendorService.GenerateNewIdAsync();
            return;
        }
        IsBusy = true;
        try
        {
            var vendor = await _vendorService.GetByIdAsync(id);
            if (vendor != null)
            {
                IsNewRecord = false;
                Title = "編輯廠商";
                Id = vendor.Id; Name = vendor.Name;
                BusinessNo = vendor.BusinessNo ?? "";
                Address = vendor.Address ?? ""; Phone = vendor.Phone ?? "";
                Cell = vendor.Cell ?? ""; Email = vendor.Email ?? "";
                Contact = vendor.Contact ?? ""; CreditLimit = vendor.CreditLimit;
                Memo = vendor.Memo ?? "";
            }
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name)) { SetError("請輸入廠商名稱"); return; }
        IsBusy = true;
        try
        {
            var vendor = new Vendor
            {
                Id = Id, Name = Name, BusinessNo = BusinessNo.NullIfEmpty(),
                Address = Address.NullIfEmpty(), Phone = Phone.NullIfEmpty(),
                Cell = Cell.NullIfEmpty(), Email = Email.NullIfEmpty(),
                Contact = Contact.NullIfEmpty(), CreditLimit = CreditLimit,
                Memo = Memo.NullIfEmpty(), CurrId = "TWD", IsActive = true
            };
            if (IsNewRecord) await _vendorService.AddAsync(vendor);
            else await _vendorService.UpdateAsync(vendor);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand] private async Task CancelAsync() => await Shell.Current.GoToAsync("..");
}
