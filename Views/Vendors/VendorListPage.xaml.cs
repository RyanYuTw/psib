using PSIB.ViewModels;

namespace PSIB.Views.Vendors;

public partial class VendorListPage : ContentPage
{
    private readonly VendorListViewModel _vm;

    public VendorListPage(VendorListViewModel vm)
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
