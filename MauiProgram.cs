using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PSIB.Data;
using PSIB.Services;
using PSIB.ViewModels;
using PSIB.Views;
using PSIB.Views.Products;
using PSIB.Views.Customers;
using PSIB.Views.Vendors;
using PSIB.Views.Purchases;
using PSIB.Views.Sales;
using PSIB.Views.AccountPayables;
using PSIB.Views.AccountReceivables;

namespace PSIB;

public static class MauiProgram
{
    public static Task DbInitTask { get; private set; } = Task.CompletedTask;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Configuration
        using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").GetAwaiter().GetResult();
        var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
        builder.Configuration.AddConfiguration(config);

        // Database - MSSQL
        var connStr = config.GetConnectionString("DefaultConnection")!;
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connStr), ServiceLifetime.Transient);

        // Services
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddTransient<IProductService, ProductService>();
        builder.Services.AddTransient<ICustomerService, CustomerService>();
        builder.Services.AddTransient<IVendorService, VendorService>();
        builder.Services.AddTransient<IPurchaseService, PurchaseService>();
        builder.Services.AddTransient<ISaleService, SaleService>();
        builder.Services.AddTransient<IAccountPayableService, AccountPayableService>();
        builder.Services.AddTransient<IAccountReceivableService, AccountReceivableService>();
        builder.Services.AddTransient<IDashboardService, DashboardService>();
        builder.Services.AddTransient<DatabaseSeeder>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MainMenuViewModel>();
        builder.Services.AddTransient<ProductListViewModel>();
        builder.Services.AddTransient<ProductDetailViewModel>();
        builder.Services.AddTransient<CustomerListViewModel>();
        builder.Services.AddTransient<CustomerDetailViewModel>();
        builder.Services.AddTransient<VendorListViewModel>();
        builder.Services.AddTransient<VendorDetailViewModel>();
        builder.Services.AddTransient<PurchaseListViewModel>();
        builder.Services.AddTransient<PurchaseDetailViewModel>();
        builder.Services.AddTransient<SaleListViewModel>();
        builder.Services.AddTransient<SaleDetailViewModel>();
        builder.Services.AddTransient<AccountPayableListViewModel>();
        builder.Services.AddTransient<AccountPayableDetailViewModel>();
        builder.Services.AddTransient<AccountReceivableListViewModel>();
        builder.Services.AddTransient<AccountReceivableDetailViewModel>();

        // Shell
        builder.Services.AddSingleton<AppShell>();

        // Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainMenuPage>();
        builder.Services.AddTransient<ProductListPage>();
        builder.Services.AddTransient<ProductDetailPage>();
        builder.Services.AddTransient<CustomerListPage>();
        builder.Services.AddTransient<CustomerDetailPage>();
        builder.Services.AddTransient<VendorListPage>();
        builder.Services.AddTransient<VendorDetailPage>();
        builder.Services.AddTransient<PurchaseListPage>();
        builder.Services.AddTransient<PurchaseDetailPage>();
        builder.Services.AddTransient<SaleListPage>();
        builder.Services.AddTransient<SaleDetailPage>();
        builder.Services.AddTransient<AccountPayableListPage>();
        builder.Services.AddTransient<AccountPayableDetailPage>();
        builder.Services.AddTransient<AccountReceivableListPage>();
        builder.Services.AddTransient<AccountReceivableDetailPage>();


        var app = builder.Build();

        // Initialize DB in background to avoid blocking the main thread
        var services = app.Services;
        DbInitTask = Task.Run(async () =>
        {
            try
            {
                using var scope = services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.EnsureCreatedAsync();
                var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
                await seeder.SeedAsync();
                Console.Error.WriteLine("[DB] Init completed successfully");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[DB] Init FAILED: {ex}");
                throw;
            }
        });

        return app;
    }
}
