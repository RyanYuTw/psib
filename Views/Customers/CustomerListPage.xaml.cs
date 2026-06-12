using PSIB.ViewModels;

namespace PSIB.Views.Customers;

public partial class CustomerListPage : ContentPage
{
    private readonly CustomerListViewModel _vm;

    public CustomerListPage(CustomerListViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadCommand.Execute(null);
    }
}
