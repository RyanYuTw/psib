using PSIB.ViewModels;

namespace PSIB.Views.AccountReceivables;

public partial class AccountReceivableDetailPage : ContentPage
{
    private readonly AccountReceivableDetailViewModel _vm;

    public AccountReceivableDetailPage(AccountReceivableDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ReceiveDatePicker.Date = _vm.ReceiveDate ?? DateTime.Today;
        ReceiveDatePicker.DateSelected += OnReceiveDateSelected;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ReceiveDatePicker.DateSelected -= OnReceiveDateSelected;
    }

    private void OnReceiveDateSelected(object? sender, DateChangedEventArgs e) =>
        _vm.ReceiveDate = e.NewDate;
}
