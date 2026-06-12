# Changelog

## [Unreleased]

### Added
- 路由授權攔截（未登入時自動導回登入頁）
- 登入失敗 5 次後鎖定帳號 15 分鐘（Brute Force 防護）
- Seed 密碼改由 `appsettings.json` 的 `SeedPasswords` 節控制，不再寫死於程式碼

### Fixed
- 移除登入頁面顯示原始例外訊息的安全性問題

## [1.0.0] - 2026-06-12

### Added
- 商品管理（主檔、條碼、庫存）
- 客戶 / 廠商管理
- 採購單與應付帳款
- 銷售單、報價單與應收帳款
- 群組權限控管（ADMIN / MANAGER / STAFF）
- BCrypt 密碼雜湊
- EF Core + SQL Server / SQLite 雙後端支援
- `.gitignore` 排除 `appsettings.json` 與 build 產出物
