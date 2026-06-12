using PSIB.ViewModels;

namespace PSIB.Views;

public partial class MainMenuPage : ContentPage
{
    private readonly MainMenuViewModel _vm;

    public MainMenuPage(MainMenuViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadDashboardCommand.Execute(null);
    }
}
