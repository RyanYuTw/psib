using PSIB.ViewModels;

namespace PSIB.Views.Sales;

public partial class SaleListPage : ContentPage
{
    private readonly SaleListViewModel _vm;

    public SaleListPage(SaleListViewModel vm)
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
