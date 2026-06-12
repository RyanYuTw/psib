using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Extensions;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

[QueryProperty(nameof(SaleId), "SaleId")]
public partial class SaleDetailViewModel : BaseViewModel
{
    private readonly ISaleService _saleService;
    private readonly ICustomerService _customerService;

    [ObservableProperty] private string _saleId = string.Empty;
    [ObservableProperty] private string _id = string.Empty;
    [ObservableProperty] private DateTime _saleDate = DateTime.Today;
    [ObservableProperty] private string _customerId = string.Empty;
    [ObservableProperty] private string _customerName = string.Empty;
    [ObservableProperty] private decimal _subTotal;
    [ObservableProperty] private decimal _tax;
    [ObservableProperty] private decimal _total;
    [ObservableProperty] private string _memo = string.Empty;
    [ObservableProperty] private bool _isNewRecord;
    [ObservableProperty] private ObservableCollection<SaleDetail> _details = new();
    [ObservableProperty] private ObservableCollection<Customer> _customers = new();
    [ObservableProperty] private Customer? _selectedCustomer;

    partial void OnSelectedCustomerChanged(Customer? customer)
    {
        if (customer != null) { CustomerId = customer.Id; CustomerName = customer.Name; }
    }

    public SaleDetailViewModel(ISaleService saleService, ICustomerService customerService)
    {
        _saleService = saleService;
        _customerService = customerService;
    }

    partial void OnSaleIdChanged(string value) => _ = LoadAsync(value);

    private async Task LoadAsync(string id)
    {
        IsBusy = true;
        try
        {
            var customerList = await _customerService.GetAllAsync();
            Customers = new ObservableCollection<Customer>(customerList);

            if (string.IsNullOrEmpty(id))
            {
                IsNewRecord = true;
                Title = "新增銷售單";
                Id = await _saleService.GenerateNewIdAsync();
                return;
            }

            var sale = await _saleService.GetByIdAsync(id);
            if (sale != null)
            {
                IsNewRecord = false;
                Title = $"銷售單 {id}";
                Id = sale.Id; SaleDate = sale.SaleDate;
                CustomerId = sale.CustomerId; CustomerName = sale.Customer?.Name ?? "";
                SubTotal = sale.SubTotal; Tax = sale.Tax; Total = sale.Total;
                Memo = sale.Memo ?? "";
                Details = new ObservableCollection<SaleDetail>(sale.Details);
                SelectedCustomer = Customers.FirstOrDefault(c => c.Id == CustomerId);
            }
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrEmpty(CustomerId)) { SetError("請選擇客戶"); return; }
        IsBusy = true;
        try
        {
            var sale = new Sale
            {
                Id = Id, SaleDate = SaleDate, CustomerId = CustomerId,
                CurrId = "TWD", ExcRate = 1, TaxRate = 5,
                SubTotal = SubTotal, Tax = Tax, Total = Total, Received = Total,
                Memo = Memo.NullIfEmpty(), Deleted = false,
                Details = Details.ToList()
            };
            if (IsNewRecord) await _saleService.AddAsync(sale);
            else await _saleService.UpdateAsync(sale);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand] private async Task CancelAsync() => await Shell.Current.GoToAsync("..");
}
