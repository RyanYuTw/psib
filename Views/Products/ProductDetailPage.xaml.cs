using PSIB.ViewModels;

namespace PSIB.Views.Products;

public partial class ProductDetailPage : ContentPage
{
    public ProductDetailPage(ProductDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
