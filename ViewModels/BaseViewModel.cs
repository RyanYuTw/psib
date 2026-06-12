using CommunityToolkit.Mvvm.ComponentModel;

namespace PSIB.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public bool IsNotBusy => !IsBusy;

    protected void SetError(string message) => ErrorMessage = message;
    protected void ClearError() => ErrorMessage = string.Empty;
}
