using PSIB.ViewModels;

namespace PSIB.Views.Products;

public partial class ProductListPage : ContentPage
{
    private readonly ProductListViewModel _vm;

    public ProductListPage(ProductListViewModel vm)
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
