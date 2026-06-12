using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

public partial class ProductListViewModel : BaseViewModel
{
    private readonly IProductService _productService;

    [ObservableProperty] private ObservableCollection<Product> _products = new();
    [ObservableProperty] private string _searchKeyword = string.Empty;
    [ObservableProperty] private Product? _selectedProduct;

    public ProductListViewModel(IProductService productService)
    {
        _productService = productService;
        Title = "商品管理";
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _productService.GetAllAsync(SearchKeyword);
            Products = new ObservableCollection<Product>(list);
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SearchAsync() => await LoadAsync();

    [RelayCommand]
    private async Task AddAsync() =>
        await Shell.Current.GoToAsync("ProductDetail", new Dictionary<string, object> { ["ProductId"] = "" });

    [RelayCommand]
    private async Task EditAsync(Product product) =>
        await Shell.Current.GoToAsync("ProductDetail", new Dictionary<string, object> { ["ProductId"] = product.Id });

    [RelayCommand]
    private async Task DeleteAsync(Product product)
    {
        var confirm = await Shell.Current.DisplayAlert("刪除確認", $"確定要刪除商品「{product.Name}」？", "確定", "取消");
        if (!confirm) return;
        await _productService.DeleteAsync(product.Id);
        Products.Remove(product);
    }
}
