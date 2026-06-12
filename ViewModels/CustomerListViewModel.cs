using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

public partial class CustomerListViewModel : BaseViewModel
{
    private readonly ICustomerService _customerService;

    [ObservableProperty] private ObservableCollection<Customer> _customers = new();
    [ObservableProperty] private string _searchKeyword = string.Empty;

    public CustomerListViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        Title = "客戶管理";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _customerService.GetAllAsync(SearchKeyword);
            Customers = new ObservableCollection<Customer>(list);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand] private async Task SearchAsync() => await LoadAsync();

    [RelayCommand]
    private async Task AddAsync() =>
        await Shell.Current.GoToAsync("CustomerDetail", new Dictionary<string, object> { ["CustomerId"] = "" });

    [RelayCommand]
    private async Task EditAsync(Customer customer) =>
        await Shell.Current.GoToAsync("CustomerDetail", new Dictionary<string, object> { ["CustomerId"] = customer.Id });

    [RelayCommand]
    private async Task DeleteAsync(Customer customer)
    {
        var confirm = await Shell.Current.DisplayAlert("刪除確認", $"確定要刪除客戶「{customer.Name}」？", "確定", "取消");
        if (!confirm) return;
        await _customerService.DeleteAsync(customer.Id);
        Customers.Remove(customer);
    }
}
