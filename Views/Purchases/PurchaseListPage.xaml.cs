using PSIB.ViewModels;

namespace PSIB.Views.Purchases;

public partial class PurchaseListPage : ContentPage
{
    private readonly PurchaseListViewModel _vm;

    public PurchaseListPage(PurchaseListViewModel vm)
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
