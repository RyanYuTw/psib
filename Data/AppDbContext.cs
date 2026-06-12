using Microsoft.EntityFrameworkCore;
using PSIB.Models;

namespace PSIB.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseStock> WarehouseStocks => Set<WarehouseStock>();
    public DbSet<Bank> Banks => Set<Bank>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseDetail> PurchaseDetails => Set<PurchaseDetail>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleDetail> SaleDetails => Set<SaleDetail>();
    public DbSet<Quotation> Quotations => Set<Quotation>();
    public DbSet<QuotationDetail> QuotationDetails => Set<QuotationDetail>();
    public DbSet<AccountPayable> AccountPayables => Set<AccountPayable>();
    public DbSet<AccountReceivable> AccountReceivables => Set<AccountReceivable>();
    public DbSet<PrePaid> PrePaids => Set<PrePaid>();
    public DbSet<PreReceived> PreReceiveds => Set<PreReceived>();
    public DbSet<Reminder> Reminders => Set<Reminder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // WarehouseStock 複合主鍵
        modelBuilder.Entity<WarehouseStock>()
            .HasKey(ws => new { ws.WarehouseId, ws.ProductId });

        // User 以 EmployeeNo 為 PK（字串）
        modelBuilder.Entity<User>()
            .HasKey(u => u.EmployeeNo);

        // Product indexes
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Barcode);
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name);

        // Customer indexes
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Name);

        // Vendor indexes
        modelBuilder.Entity<Vendor>()
            .HasIndex(v => v.Name);

        // Purchase soft delete filter
        modelBuilder.Entity<Purchase>()
            .HasQueryFilter(p => !p.Deleted);
        modelBuilder.Entity<PurchaseDetail>()
            .HasQueryFilter(pd => !pd.Deleted);
        modelBuilder.Entity<Sale>()
            .HasQueryFilter(s => !s.Deleted);
        modelBuilder.Entity<SaleDetail>()
            .HasQueryFilter(sd => !sd.Deleted);
        modelBuilder.Entity<Quotation>()
            .HasQueryFilter(q => !q.Deleted);

        // AccountPayable → Purchase (nullable FK，不設 cascade)
        modelBuilder.Entity<AccountPayable>()
            .HasOne(ap => ap.Purchase)
            .WithMany(p => p.AccountPayables)
            .HasForeignKey(ap => ap.PurchaseId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AccountPayable>()
            .HasOne(ap => ap.Vendor)
            .WithMany(v => v.AccountPayables)
            .HasForeignKey(ap => ap.VendorId)
            .OnDelete(DeleteBehavior.SetNull);

        // AccountReceivable → Sale
        modelBuilder.Entity<AccountReceivable>()
            .HasOne(ar => ar.Sale)
            .WithMany(s => s.AccountReceivables)
            .HasForeignKey(ar => ar.SaleId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AccountReceivable>()
            .HasOne(ar => ar.Customer)
            .WithMany(c => c.AccountReceivables)
            .HasForeignKey(ar => ar.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        // Purchase → Vendor (no cascade on AP)
        modelBuilder.Entity<Purchase>()
            .HasOne(p => p.Vendor)
            .WithMany(v => v.Purchases)
            .HasForeignKey(p => p.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Sale → Customer
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
