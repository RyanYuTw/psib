using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Extensions;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

[QueryProperty(nameof(ArId), "ArId")]
public partial class AccountReceivableDetailViewModel : BaseViewModel
{
    private readonly IAccountReceivableService _arService;

    [ObservableProperty] private string _arId = string.Empty;
    [ObservableProperty] private string _id = string.Empty;
    [ObservableProperty] private string _customerId = string.Empty;
    [ObservableProperty] private string _customerName = string.Empty;
    [ObservableProperty] private DateTime? _receiveDate;
    [ObservableProperty] private decimal _receiveCash;
    [ObservableProperty] private decimal _receiveAmount;
    [ObservableProperty] private decimal _totalBalance;
    [ObservableProperty] private string _memo = string.Empty;
    [ObservableProperty] private bool _isNewRecord;

    public AccountReceivableDetailViewModel(IAccountReceivableService arService)
    {
        _arService = arService;
    }

    partial void OnArIdChanged(string value) => _ = LoadAsync(value);

    private async Task LoadAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            IsNewRecord = true;
            Title = "新增應收帳款";
            Id = await _arService.GenerateNewIdAsync();
            return;
        }
        IsBusy = true;
        try
        {
            var ar = await _arService.GetByIdAsync(id);
            if (ar != null)
            {
                IsNewRecord = false;
                Title = $"應收帳款 {id}";
                Id = ar.Id; CustomerId = ar.CustomerId ?? "";
                CustomerName = ar.Customer?.Name ?? "";
                ReceiveDate = ar.ReceiveDate; ReceiveCash = ar.ReceiveCash;
                ReceiveAmount = ar.ReceiveAmount; TotalBalance = ar.TotalBalance;
                Memo = ar.Memo ?? "";
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
            var ar = new AccountReceivable
            {
                Id = Id, CustomerId = CustomerId.NullIfEmpty(),
                ReceiveDate = ReceiveDate, ReceiveCash = ReceiveCash,
                ReceiveAmount = ReceiveAmount, TotalBalance = TotalBalance,
                Memo = Memo.NullIfEmpty()
            };
            if (IsNewRecord) await _arService.AddAsync(ar);
            else await _arService.UpdateAsync(ar);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand] private async Task CancelAsync() => await Shell.Current.GoToAsync("..");
}
