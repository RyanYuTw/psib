using PSIB.ViewModels;

namespace PSIB.Views.AccountReceivables;

public partial class AccountReceivableListPage : ContentPage
{
    private readonly AccountReceivableListViewModel _vm;

    public AccountReceivableListPage(AccountReceivableListViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadCommand.Execute(null);
    }
}
