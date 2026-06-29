using Bogus;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using PSIB.Models;

namespace PSIB.Data;

public class DatabaseSeeder
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public DatabaseSeeder(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task SeedAsync()
    {
        if (!_db.Users.Any())
        {
            Randomizer.Seed = new Random(42);
            await SeedBaseDataAsync();
            await SeedMasterDataAsync();
            await SeedTransactionDataAsync();
            await _db.SaveChangesAsync();
        }

        await SeedExtraUsersAsync();
    }

    private async Task SeedExtraUsersAsync()
    {
        var demoPwd = _config["SeedPasswords:Demo"];
        if (demoPwd == null) return;

        if (!_db.Users.Any(u => u.UserId == "demo"))
        {
            _db.Users.Add(new User
            {
                EmployeeNo = "9999",
                UserId = "demo",
                Password = BCrypt.Net.BCrypt.HashPassword(demoPwd),
                Name = "示範帳號",
                UserGroupId = "STAFF"
            });
            await _db.SaveChangesAsync();
        }
    }

    private static string GenerateAndLogPassword(string role)
    {
        var pwd = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(12));
        Console.Error.WriteLine($"[Seed] {role} 初始密碼（請立即變更）: {pwd}");
        return pwd;
    }

    private async Task SeedBaseDataAsync()
    {
        // 使用者群組
        var groups = new List<UserGroup>
        {
            new() { Id = "ADMIN", Name = "系統管理員", CanSetting = true, CanUserMgmt = true },
            new() { Id = "MANAGER", Name = "管理人員", CanSetting = false, CanUserMgmt = false },
            new() { Id = "STAFF", Name = "一般人員", CanSetting = false, CanUserMgmt = false, CanReport = false }
        };
        _db.UserGroups.AddRange(groups);

        // 使用者（密碼從設定檔讀取，避免寫死在程式碼中）
        var adminPwd = _config["SeedPasswords:Admin"] ?? GenerateAndLogPassword("admin");
        var managerPwd = _config["SeedPasswords:Manager"] ?? GenerateAndLogPassword("manager");
        var staffPwd = _config["SeedPasswords:Staff"] ?? GenerateAndLogPassword("staff");

        var users = new List<User>
        {
            new() { EmployeeNo = "0001", UserId = "admin", Password = BCrypt.Net.BCrypt.HashPassword(adminPwd), Name = "系統管理員", UserGroupId = "ADMIN" },
            new() { EmployeeNo = "0002", UserId = "manager", Password = BCrypt.Net.BCrypt.HashPassword(managerPwd), Name = "王大明", UserGroupId = "MANAGER" },
            new() { EmployeeNo = "0003", UserId = "staff1", Password = BCrypt.Net.BCrypt.HashPassword(staffPwd), Name = "李小華", UserGroupId = "STAFF" },
            new() { EmployeeNo = "0004", UserId = "staff2", Password = BCrypt.Net.BCrypt.HashPassword(staffPwd), Name = "陳美玲", UserGroupId = "STAFF" }
        };
        _db.Users.AddRange(users);

        // 店家資料
        _db.Shops.Add(new Shop
        {
            Id = 1,
            SName = "Wish Estate",
            BusinessNo = "12345678",
            Address = "台北市信義區松仁路100號",
            Phone = "02-12345678",
            Email = "info@wishestate.com.tw"
        });

        // 系統設定
        var settings = new List<Setting>
        {
            new() { Parameter = "tax_rate", Value = "5", Description = "稅率(%)" },
            new() { Parameter = "warehouse_id", Value = "1", Description = "預設倉庫" },
            new() { Parameter = "backup_remind", Value = "7", Description = "備份提醒天數" },
            new() { Parameter = "currency", Value = "TWD", Description = "預設幣別" },
            new() { Parameter = "decimal_places", Value = "0", Description = "金額小數位數" },
            new() { Parameter = "company_name", Value = "Wish Estate", Description = "公司名稱" },
            new() { Parameter = "invoice_prefix", Value = "WE", Description = "單號前綴" }
        };
        _db.Settings.AddRange(settings);

        // 幣別
        var currencies = new List<Currency>
        {
            new() { CurrId = "TWD", Name = "新台幣", ExcRate = 1 },
            new() { CurrId = "USD", Name = "美元", ExcRate = 31.5m },
            new() { CurrId = "CNY", Name = "人民幣", ExcRate = 4.35m },
            new() { CurrId = "JPY", Name = "日圓", ExcRate = 0.21m }
        };
        _db.Currencies.AddRange(currencies);

        // 銀行
        var banks = new List<Bank>
        {
            new() { Id = "004", Name = "台灣銀行", BranchCode = "0040", BranchName = "信義分行" },
            new() { Id = "012", Name = "台北富邦銀行", BranchCode = "0120", BranchName = "信義分行" },
            new() { Id = "013", Name = "國泰世華銀行", BranchCode = "0130", BranchName = "信義分行" },
            new() { Id = "822", Name = "中國信託銀行", BranchCode = "8220", BranchName = "信義分行" }
        };
        _db.Banks.AddRange(banks);

        // 倉庫
        _db.Warehouses.Add(new Warehouse
        {
            Id = 1,
            Name = "主倉庫",
            Address = "台北市信義區松仁路100號 B1",
            IsDefault = true
        });

        await _db.SaveChangesAsync();
    }

    private async Task SeedMasterDataAsync()
    {
        // 商品類別
        var categories = new List<Category>
        {
            new() { Id = "CAT001", Name = "住宅" },
            new() { Id = "CAT002", Name = "商辦" },
            new() { Id = "CAT003", Name = "廠房" },
            new() { Id = "CAT004", Name = "土地" },
            new() { Id = "CAT005", Name = "店面" }
        };
        _db.Categories.AddRange(categories);

        // 單位
        var units = new List<Unit>
        {
            new() { Id = "U001", Name = "坪" },
            new() { Id = "U002", Name = "戶" },
            new() { Id = "U003", Name = "棟" },
            new() { Id = "U004", Name = "間" },
            new() { Id = "U005", Name = "坐" }
        };
        _db.Units.AddRange(units);

        await _db.SaveChangesAsync();

        // 商品（房產物件）
        var faker = new Faker("zh_TW");
        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => $"P{f.IndexFaker + 1:D4}")
            .RuleFor(p => p.Name, f => f.PickRandom(new[]
            {
                $"信義區{f.Address.StreetName()}{f.Random.Int(1,50)}號{f.Random.Int(1,30)}F",
                $"大安區{f.Address.StreetName()}{f.Random.Int(1,100)}號",
                $"中山區{f.Address.StreetName()}{f.Random.Int(1,200)}號{f.Random.Int(1,20)}F",
                $"內湖區{f.Address.StreetName()}{f.Random.Int(1,300)}號"
            }))
            .RuleFor(p => p.Barcode, f => f.Commerce.Ean13())
            .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id)
            .RuleFor(p => p.UnitId, f => f.PickRandom(units).Id)
            .RuleFor(p => p.Pack, f => $"{f.Random.Int(10, 200)}坪")
            .RuleFor(p => p.Cost, f => f.Random.Decimal(500000, 50000000))
            .RuleFor(p => p.Price, (f, p) => p.Cost * f.Random.Decimal(1.05m, 1.3m))
            .RuleFor(p => p.CurrentVol, f => f.Random.Decimal(0, 10))
            .RuleFor(p => p.SafeVol, _ => 0)
            .RuleFor(p => p.Stock, _ => true)
            .RuleFor(p => p.IsActive, _ => true)
            .RuleFor(p => p.CreatedAt, f => f.Date.Past(2));

        var products = productFaker.Generate(50);
        _db.Products.AddRange(products);

        // 客戶
        var customerFaker = new Faker<Customer>()
            .RuleFor(c => c.Id, f => $"C{f.IndexFaker + 1:D4}")
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.BusinessNo, f => f.Random.String2(8, "0123456789"))
            .RuleFor(c => c.Address, f => $"台北市{f.Address.StreetAddress()}")
            .RuleFor(c => c.Phone, f => $"02-{f.Random.Int(10000000, 99999999)}")
            .RuleFor(c => c.Cell, f => $"09{f.Random.Int(10000000, 99999999)}")
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Contact, f => f.Name.FullName())
            .RuleFor(c => c.CreditLimit, f => f.Random.Decimal(100000, 10000000))
            .RuleFor(c => c.CurrId, _ => "TWD")
            .RuleFor(c => c.IsActive, _ => true)
            .RuleFor(c => c.CreatedAt, f => f.Date.Past(3));

        var customers = customerFaker.Generate(30);
        _db.Customers.AddRange(customers);

        // 廠商
        var vendorFaker = new Faker<Vendor>()
            .RuleFor(v => v.Id, f => $"V{f.IndexFaker + 1:D4}")
            .RuleFor(v => v.Name, f => f.Company.CompanyName())
            .RuleFor(v => v.BusinessNo, f => f.Random.String2(8, "0123456789"))
            .RuleFor(v => v.Address, f => $"台北市{f.Address.StreetAddress()}")
            .RuleFor(v => v.Phone, f => $"02-{f.Random.Int(10000000, 99999999)}")
            .RuleFor(v => v.Cell, f => $"09{f.Random.Int(10000000, 99999999)}")
            .RuleFor(v => v.Email, f => f.Internet.Email())
            .RuleFor(v => v.Contact, f => f.Name.FullName())
            .RuleFor(v => v.CreditLimit, f => f.Random.Decimal(100000, 5000000))
            .RuleFor(v => v.CurrId, _ => "TWD")
            .RuleFor(v => v.IsActive, _ => true)
            .RuleFor(v => v.CreatedAt, f => f.Date.Past(3));

        var vendors = vendorFaker.Generate(20);
        _db.Vendors.AddRange(vendors);

        await _db.SaveChangesAsync();

        // 庫存
        var stocks = products.Select(p => new WarehouseStock
        {
            WarehouseId = 1,
            ProductId = p.Id,
            OpeningStock = faker.Random.Decimal(0, 5),
            OpeningCost = p.Cost,
            SafeVolumn = 0,
            CurrentVolumn = p.CurrentVol
        });
        _db.WarehouseStocks.AddRange(stocks);

        await _db.SaveChangesAsync();
    }

    private async Task SeedTransactionDataAsync()
    {
        var faker = new Faker("zh_TW");
        var products = _db.Products.ToList();
        var customers = _db.Customers.ToList();
        var vendors = _db.Vendors.ToList();
        var employees = _db.Users.ToList();

        // 採購單（過去一年）
        var purchases = new List<Purchase>();
        var purchaseDetails = new List<PurchaseDetail>();
        var accountPayables = new List<AccountPayable>();

        for (int i = 1; i <= 60; i++)
        {
            var vendor = faker.PickRandom(vendors);
            var purchaseDate = faker.Date.Past(1);
            var purchaseId = purchaseDate.ToString("yyyyMMdd") + i.ToString("D4");

            var selectedProducts = faker.PickRandom(products, faker.Random.Int(1, 4)).ToList();
            decimal subTotal = 0;
            int seq = 1;

            foreach (var product in selectedProducts)
            {
                var amount = faker.Random.Decimal(1, 5);
                var discount = faker.PickRandom(new[] { 80m, 85m, 90m, 95m, 100m });
                var cost = product.Cost;
                var lineTotal = Math.Round(amount * cost * discount / 100, 0);
                subTotal += lineTotal;

                purchaseDetails.Add(new PurchaseDetail
                {
                    PurchaseId = purchaseId,
                    Seq = seq++,
                    ProductId = product.Id,
                    Amount = amount,
                    Discount = discount,
                    Cost = cost,
                    LineTotal = lineTotal
                });
            }

            var tax = Math.Round(subTotal * 0.05m, 0);
            var total = subTotal + tax;

            purchases.Add(new Purchase
            {
                Id = purchaseId,
                PurchaseDate = purchaseDate,
                VendorId = vendor.Id,
                CurrId = "TWD",
                ExcRate = 1,
                TaxRate = 5,
                SubTotal = subTotal,
                Tax = tax,
                Total = total,
                Paid = total,
                EmployeeNo = faker.PickRandom(employees).EmployeeNo,
                Deleted = false,
                CreatedAt = purchaseDate
            });

            // 應付帳款
            accountPayables.Add(new AccountPayable
            {
                Id = $"AP{purchaseDate:yyyyMMdd}{i:D4}",
                PurchaseId = purchaseId,
                VendorId = vendor.Id,
                PayDate = purchaseDate.AddDays(faker.Random.Int(1, 30)),
                PayCash = total,
                PayAmount = total,
                TotalBalance = 0,
                CreatedAt = purchaseDate
            });
        }

        _db.Purchases.AddRange(purchases);
        _db.PurchaseDetails.AddRange(purchaseDetails);
        await _db.SaveChangesAsync();
        _db.AccountPayables.AddRange(accountPayables);

        // 銷售單（過去一年）
        var sales = new List<Sale>();
        var saleDetails = new List<SaleDetail>();
        var accountReceivables = new List<AccountReceivable>();

        for (int i = 1; i <= 80; i++)
        {
            var customer = faker.PickRandom(customers);
            var saleDate = faker.Date.Past(1);
            var saleId = saleDate.ToString("yyyyMMdd") + i.ToString("D4");

            var selectedProducts = faker.PickRandom(products, faker.Random.Int(1, 3)).ToList();
            decimal subTotal = 0;
            int seq = 1;

            foreach (var product in selectedProducts)
            {
                var amount = faker.Random.Decimal(1, 3);
                var discount = faker.PickRandom(new[] { 85m, 90m, 95m, 100m });
                var price = product.Price;
                var lineTotal = Math.Round(amount * price * discount / 100, 0);
                subTotal += lineTotal;

                saleDetails.Add(new SaleDetail
                {
                    SaleId = saleId,
                    Seq = seq++,
                    ProductId = product.Id,
                    Amount = amount,
                    Discount = discount,
                    Price = price,
                    LineTotal = lineTotal
                });
            }

            var tax = Math.Round(subTotal * 0.05m, 0);
            var total = subTotal + tax;

            sales.Add(new Sale
            {
                Id = saleId,
                SaleDate = saleDate,
                CustomerId = customer.Id,
                CurrId = "TWD",
                ExcRate = 1,
                TaxRate = 5,
                SubTotal = subTotal,
                Tax = tax,
                Total = total,
                Received = total,
                EmployeeNo = faker.PickRandom(employees).EmployeeNo,
                Deleted = false,
                CreatedAt = saleDate
            });

            // 應收帳款
            accountReceivables.Add(new AccountReceivable
            {
                Id = $"AR{saleDate:yyyyMMdd}{i:D4}",
                SaleId = saleId,
                CustomerId = customer.Id,
                ReceiveDate = saleDate.AddDays(faker.Random.Int(1, 45)),
                ReceiveCash = total,
                ReceiveAmount = total,
                TotalBalance = 0,
                CreatedAt = saleDate
            });
        }

        _db.Sales.AddRange(sales);
        _db.SaleDetails.AddRange(saleDetails);
        await _db.SaveChangesAsync();
        _db.AccountReceivables.AddRange(accountReceivables);

        // 提醒事項
        var reminders = new List<Reminder>
        {
            new() { Title = "廠商 V0001 合約到期", Content = "記得聯絡廠商續約", RemindDate = DateTime.Now.AddDays(7), EmployeeNo = "0001" },
            new() { Title = "客戶 C0005 應收款追蹤", Content = "已逾期30天，請聯繫財務確認", RemindDate = DateTime.Now.AddDays(1), EmployeeNo = "0002" },
            new() { Title = "月底庫存盤點", Content = "每月最後一天進行庫存盤點", RemindDate = DateTime.Now.AddDays(14), EmployeeNo = "0001" }
        };
        _db.Reminders.AddRange(reminders);

        await _db.SaveChangesAsync();
    }
}
