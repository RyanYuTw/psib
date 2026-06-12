using PSIB.ViewModels;

namespace PSIB.Views.Purchases;

public partial class PurchaseDetailPage : ContentPage
{
    public PurchaseDetailPage(PurchaseDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
