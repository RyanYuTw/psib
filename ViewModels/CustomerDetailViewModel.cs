using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Extensions;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

[QueryProperty(nameof(CustomerId), "CustomerId")]
public partial class CustomerDetailViewModel : BaseViewModel
{
    private readonly ICustomerService _customerService;

    [ObservableProperty] private string _customerId = string.Empty;
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

    public CustomerDetailViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    partial void OnCustomerIdChanged(string value) => _ = LoadAsync(value);

    private async Task LoadAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            IsNewRecord = true;
            Title = "新增客戶";
            Id = await _customerService.GenerateNewIdAsync();
            return;
        }

        IsBusy = true;
        try
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer != null)
            {
                IsNewRecord = false;
                Title = "編輯客戶";
                Id = customer.Id;
                Name = customer.Name;
                BusinessNo = customer.BusinessNo ?? "";
                Address = customer.Address ?? "";
                Phone = customer.Phone ?? "";
                Cell = customer.Cell ?? "";
                Email = customer.Email ?? "";
                Contact = customer.Contact ?? "";
                CreditLimit = customer.CreditLimit;
                Memo = customer.Memo ?? "";
            }
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name)) { SetError("請輸入客戶名稱"); return; }
        IsBusy = true;
        try
        {
            var customer = new Customer
            {
                Id = Id, Name = Name, BusinessNo = BusinessNo.NullIfEmpty(),
                Address = Address.NullIfEmpty(), Phone = Phone.NullIfEmpty(),
                Cell = Cell.NullIfEmpty(), Email = Email.NullIfEmpty(),
                Contact = Contact.NullIfEmpty(), CreditLimit = CreditLimit,
                Memo = Memo.NullIfEmpty(), CurrId = "TWD", IsActive = true
            };
            if (IsNewRecord) await _customerService.AddAsync(customer);
            else await _customerService.UpdateAsync(customer);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand] private async Task CancelAsync() => await Shell.Current.GoToAsync("..");
}
