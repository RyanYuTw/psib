using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

public partial class AccountPayableListViewModel : BaseViewModel
{
    private readonly IAccountPayableService _apService;

    [ObservableProperty] private ObservableCollection<AccountPayable> _accountPayables = new();
    [ObservableProperty] private DateTime _fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    [ObservableProperty] private DateTime _toDate = DateTime.Today;

    public AccountPayableListViewModel(IAccountPayableService apService)
    {
        _apService = apService;
        Title = "應付帳款";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _apService.GetAllAsync(FromDate, ToDate);
            AccountPayables = new ObservableCollection<AccountPayable>(list);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task AddAsync() =>
        await Shell.Current.GoToAsync("AccountPayableDetail", new Dictionary<string, object> { ["ApId"] = "" });

    [RelayCommand]
    private async Task EditAsync(AccountPayable ap) =>
        await Shell.Current.GoToAsync("AccountPayableDetail", new Dictionary<string, object> { ["ApId"] = ap.Id });
}
