using PSIB.ViewModels;

namespace PSIB.Views.AccountPayables;

public partial class AccountPayableListPage : ContentPage
{
    private readonly AccountPayableListViewModel _vm;

    public AccountPayableListPage(AccountPayableListViewModel vm)
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
