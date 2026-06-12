using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

public partial class AccountReceivableListViewModel : BaseViewModel
{
    private readonly IAccountReceivableService _arService;

    [ObservableProperty] private ObservableCollection<AccountReceivable> _accountReceivables = new();
    [ObservableProperty] private DateTime _fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    [ObservableProperty] private DateTime _toDate = DateTime.Today;

    public AccountReceivableListViewModel(IAccountReceivableService arService)
    {
        _arService = arService;
        Title = "應收帳款";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _arService.GetAllAsync(FromDate, ToDate);
            AccountReceivables = new ObservableCollection<AccountReceivable>(list);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task AddAsync() =>
        await Shell.Current.GoToAsync("AccountReceivableDetail", new Dictionary<string, object> { ["ArId"] = "" });

    [RelayCommand]
    private async Task EditAsync(AccountReceivable ar) =>
        await Shell.Current.GoToAsync("AccountReceivableDetail", new Dictionary<string, object> { ["ArId"] = ar.Id });
}
