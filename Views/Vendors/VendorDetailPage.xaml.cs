using PSIB.ViewModels;

namespace PSIB.Views.Vendors;

public partial class VendorDetailPage : ContentPage
{
    public VendorDetailPage(VendorDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
