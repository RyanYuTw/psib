using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Extensions;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

[QueryProperty(nameof(ApId), "ApId")]
public partial class AccountPayableDetailViewModel : BaseViewModel
{
    private readonly IAccountPayableService _apService;

    [ObservableProperty] private string _apId = string.Empty;
    [ObservableProperty] private string _id = string.Empty;
    [ObservableProperty] private string _vendorId = string.Empty;
    [ObservableProperty] private string _vendorName = string.Empty;
    [ObservableProperty] private DateTime? _payDate;
    [ObservableProperty] private decimal _payCash;
    [ObservableProperty] private decimal _payAmount;
    [ObservableProperty] private decimal _totalBalance;
    [ObservableProperty] private string _memo = string.Empty;
    [ObservableProperty] private bool _isNewRecord;

    public AccountPayableDetailViewModel(IAccountPayableService apService)
    {
        _apService = apService;
    }

    partial void OnApIdChanged(string value) => _ = LoadAsync(value);

    private async Task LoadAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            IsNewRecord = true;
            Title = "新增應付帳款";
            Id = await _apService.GenerateNewIdAsync();
            return;
        }
        IsBusy = true;
        try
        {
            var ap = await _apService.GetByIdAsync(id);
            if (ap != null)
            {
                IsNewRecord = false;
                Title = $"應付帳款 {id}";
                Id = ap.Id; VendorId = ap.VendorId ?? "";
                VendorName = ap.Vendor?.Name ?? "";
                PayDate = ap.PayDate; PayCash = ap.PayCash;
                PayAmount = ap.PayAmount; TotalBalance = ap.TotalBalance;
                Memo = ap.Memo ?? "";
            }
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        IsBusy = true;
        try
        {
            var ap = new AccountPayable
            {
                Id = Id, VendorId = VendorId.NullIfEmpty(),
                PayDate = PayDate, PayCash = PayCash,
                PayAmount = PayAmount, TotalBalance = TotalBalance,
                Memo = Memo.NullIfEmpty()
            };
            if (IsNewRecord) await _apService.AddAsync(ap);
            else await _apService.UpdateAsync(ap);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand] private async Task CancelAsync() => await Shell.Current.GoToAsync("..");
}
