# PSIB 進銷存管理系統

適用於 Wish Estate 的進貨、銷售、庫存與帳務管理系統，以 .NET MAUI 開發，支援 Windows 與 macOS。

## 技術架構

| 層級 | 技術 |
|------|------|
| UI | .NET MAUI 9 + CommunityToolkit.Maui |
| ViewModel | CommunityToolkit.Mvvm（ObservableProperty、RelayCommand） |
| ORM | Entity Framework Core 9 |
| 資料庫 | SQL Server（正式環境）/ SQLite（可切換） |
| 認證 | BCrypt.Net-Next |

## 功能模組

- **商品管理** — 商品主檔、條碼、庫存查詢
- **客戶 / 廠商管理** — 主檔維護、信用額度
- **採購管理** — 採購單建立、應付帳款追蹤
- **銷售管理** — 銷售單、報價單、應收帳款追蹤
- **帳務** — 應付 / 應收帳款管理
- **權限控管** — 群組權限（銷售、採購、報表、設定、使用者管理）

## 環境需求

- .NET 9 SDK（或 .NET 10 SDK）
- SQL Server 2019+（本機或遠端）
- Visual Studio 2022 17.9+（含 .NET MAUI 工作負載）或 Rider

## 快速開始

### 1. 複製設定檔

```bash
cp appsettings.example.json appsettings.json
```

編輯 `appsettings.json`，填入實際資料庫連線資訊與初始密碼：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PSIB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  },
  "SeedPasswords": {
    "Admin":   "設定強密碼",
    "Manager": "設定強密碼",
    "Staff":   "設定強密碼"
  }
}
```

> ⚠️ `appsettings.json` 已加入 `.gitignore`，請勿手動追蹤此檔案。

### 2. 建置並執行

```bash
# Windows
dotnet build -f net10.0-windows10.0.19041.0
dotnet run -f net10.0-windows10.0.19041.0

# macOS
dotnet build -f net9.0-maccatalyst
dotnet run -f net9.0-maccatalyst
```

首次啟動時會自動建立資料庫 schema 並植入範例資料（Seeder）。

### 3. 預設帳號

Seeder 執行後會依 `appsettings.json` 中 `SeedPasswords` 設定建立下列帳號：

| 帳號 | 群組 | 權限 |
|------|------|------|
| `admin` | 系統管理員 | 全部 |
| `manager` | 管理人員 | 銷售、採購、報表 |
| `staff1` / `staff2` | 一般人員 | 銷售、採購 |

> 若 `SeedPasswords` 未設定，系統會自動產生隨機密碼並輸出至 stderr，請於首次啟動後立即變更。

## 安全性說明

- 密碼使用 BCrypt（work factor 10）雜湊儲存
- 登入失敗 5 次後鎖定帳號 15 分鐘
- 所有頁面導航均驗證登入狀態（Shell `OnNavigating` 攔截）
- 連線字串與種子密碼僅存於本機 `appsettings.json`，不進入版本控制

## 專案結構

```
PSIB/
├── Data/               # DbContext、DatabaseSeeder
├── Models/             # EF Core 實體
├── Services/           # 業務邏輯 interface + 實作
├── ViewModels/         # MVVM ViewModel
├── Views/              # MAUI XAML 頁面
├── Converters/         # XAML 值轉換器
├── Extensions/         # 擴充方法
├── Platforms/          # 平台特定入口
├── appsettings.example.json   # 設定範本（請複製為 appsettings.json）
└── .gitignore
```
