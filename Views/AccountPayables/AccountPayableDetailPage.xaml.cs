using PSIB.ViewModels;

namespace PSIB.Views.AccountPayables;

public partial class AccountPayableDetailPage : ContentPage
{
    private readonly AccountPayableDetailViewModel _vm;

    public AccountPayableDetailPage(AccountPayableDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        PayDatePicker.Date = _vm.PayDate ?? DateTime.Today;
        PayDatePicker.DateSelected += OnPayDateSelected;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        PayDatePicker.DateSelected -= OnPayDateSelected;
    }

    private void OnPayDateSelected(object? sender, DateChangedEventArgs e) =>
        _vm.PayDate = e.NewDate;
}
