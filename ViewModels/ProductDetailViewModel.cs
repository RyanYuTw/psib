using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PSIB.Models;
using PSIB.Services;

namespace PSIB.ViewModels;

[QueryProperty(nameof(ProductId), "ProductId")]
public partial class ProductDetailViewModel : BaseViewModel
{
    private readonly IProductService _productService;

    [ObservableProperty] private string _productId = string.Empty;
    [ObservableProperty] private string _id = string.Empty;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _barcode = string.Empty;
    [ObservableProperty] private decimal _cost;
    [ObservableProperty] private decimal _price;
    [ObservableProperty] private decimal _currentVol;
    [ObservableProperty] private string _memo = string.Empty;
    [ObservableProperty] private bool _isNewRecord;

    public ProductDetailViewModel(IProductService productService)
    {
        _productService = productService;
    }

    partial void OnProductIdChanged(string value) => _ = LoadAsync(value);

    private async Task LoadAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            IsNewRecord = true;
            Title = "新增商品";
            Id = $"P{DateTime.Now:yyyyMMddHHmmss}";
            return;
        }

        IsBusy = true;
        try
        {
            var product = await _productService.GetByIdAsync(id);
            if (product != null)
            {
                IsNewRecord = false;
                Title = "編輯商品";
                Id = product.Id;
                Name = product.Name;
                Barcode = product.Barcode ?? "";
                Cost = product.Cost;
                Price = product.Price;
                CurrentVol = product.CurrentVol;
                Memo = product.Memo ?? "";
            }
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            SetError("請輸入商品名稱");
            return;
        }

        IsBusy = true;
        try
        {
            var product = new Product
            {
                Id = Id,
                Name = Name,
                Barcode = string.IsNullOrEmpty(Barcode) ? null : Barcode,
                Cost = Cost,
                Price = Price,
                CurrentVol = CurrentVol,
                Memo = string.IsNullOrEmpty(Memo) ? null : Memo,
                IsActive = true
            };

            if (IsNewRecord)
                await _productService.AddAsync(product);
            else
                await _productService.UpdateAsync(product);

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex) { SetError(ex.Message); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task CancelAsync() => await Shell.Current.GoToAsync("..");
}
