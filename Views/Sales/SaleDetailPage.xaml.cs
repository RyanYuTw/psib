using PSIB.ViewModels;

namespace PSIB.Views.Sales;

public partial class SaleDetailPage : ContentPage
{
    public SaleDetailPage(SaleDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
