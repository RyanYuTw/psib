using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Services;

namespace PSIB.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _userId = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "PSIB 進銷存管理系統";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(Password))
        {
            SetError("請輸入帳號和密碼");
            return;
        }

        IsBusy = true;
        ClearError();

        try
        {
            // 等待 DB 初始化完成（最多 30 秒）
            var initTask = MauiProgram.DbInitTask;
            if (!initTask.IsCompleted)
            {
                SetError("系統初始化中，請稍候...");
                await Task.WhenAny(initTask, Task.Delay(30000));
            }
            if (initTask.IsFaulted)
            {
                Console.Error.WriteLine($"[DB] Init FAILED: {initTask.Exception}");
                SetError("資料庫初始化失敗，請聯絡系統管理員");
                return;
            }
            if (!initTask.IsCompletedSuccessfully)
            {
                SetError("資料庫初始化逾時，請重啟應用程式");
                return;
            }
            ClearError();

            var success = await _authService.LoginAsync(UserId, Password);
            if (success)
            {
                AppShell.EnableFlyout();
                await Shell.Current.GoToAsync("//MainMenu");
            }
            else
            {
                SetError("帳號或密碼錯誤，請重新輸入");
                Password = string.Empty;
            }
        }
        catch (InvalidOperationException ex)
        {
            // 帳號鎖定訊息可安全顯示
            SetError(ex.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Login] Unexpected error: {ex}");
            SetError("登入時發生錯誤，請稍後再試");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
