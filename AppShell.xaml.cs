using PSIB.Services;
using PSIB.Views.Products;
using PSIB.Views.Customers;
using PSIB.Views.Vendors;
using PSIB.Views.Purchases;
using PSIB.Views.Sales;
using PSIB.Views.AccountPayables;
using PSIB.Views.AccountReceivables;

namespace PSIB;

public partial class AppShell : Shell
{
    private readonly IAuthService _authService;

    public AppShell(IAuthService authService)
    {
        _authService = authService;
        InitializeComponent();

        Routing.RegisterRoute("ProductDetail", typeof(ProductDetailPage));
        Routing.RegisterRoute("CustomerDetail", typeof(CustomerDetailPage));
        Routing.RegisterRoute("VendorDetail", typeof(VendorDetailPage));
        Routing.RegisterRoute("PurchaseDetail", typeof(PurchaseDetailPage));
        Routing.RegisterRoute("SaleDetail", typeof(SaleDetailPage));
        Routing.RegisterRoute("AccountPayableDetail", typeof(AccountPayableDetailPage));
        Routing.RegisterRoute("AccountReceivableDetail", typeof(AccountReceivableDetailPage));
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);

        var target = args.Target.Location.OriginalString;
        if (!target.Contains("Login") && !_authService.IsLoggedIn)
        {
            args.Cancel();
            Dispatcher.Dispatch(async () =>
                await Current.GoToAsync("//Login"));
        }
    }

    public static void EnableFlyout() =>
        Current.FlyoutBehavior = FlyoutBehavior.Flyout;

    public static void DisableFlyout() =>
        Current.FlyoutBehavior = FlyoutBehavior.Disabled;
}
