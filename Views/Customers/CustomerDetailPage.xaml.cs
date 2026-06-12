using PSIB.ViewModels;

namespace PSIB.Views.Customers;

public partial class CustomerDetailPage : ContentPage
{
    public CustomerDetailPage(CustomerDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
