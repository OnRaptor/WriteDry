using Stylet;
using System.Collections.Generic;
using System.Linq;
using WriteDry.Services;
using WriteDry.Utils;
using WriteDry.ViewModels.Component;
using WriteDry.ViewModels.Framework;

namespace WriteDry.ViewModels
{
    public class ProductsViewModel : Screen
    {
        public string UserName { get; set; }
        public int MaxProductsCount { get; set; }
        public string SearchText { get; set; }
        public SortItem SelectedSort { get; set; } = new SortItem { SortProduct = SortProduct.Desc };
        public FilterItem SelectedFilter { get; set; } = new FilterItem { FilterProduct = FilterProduct.All };
        public BindableCollection<ProductViewModel> Products { get; set; } = new BindableCollection<ProductViewModel>();

        private NavigationController navigationController;
        private ApplicationContext dbContext;
        private AdminService _adminService;
        private List<ProductViewModel> _productsCache; //data from db
        private DialogManager _dialogManager;
        private IViewModelFactory _viewModelFactory;

        public ProductsViewModel(NavigationController navigationController, ApplicationContext dbContext, AdminService _adminService, List<ProductViewModel> productsCache, DialogManager dialogManager, IViewModelFactory viewModelFactory)
        {
            this.navigationController = navigationController;
            this.dbContext = dbContext;
            this._adminService = _adminService;
            _productsCache = productsCache;
            _dialogManager = dialogManager;
            _viewModelFactory = viewModelFactory;
            this.DisplayName = "Продукты";
            UserName = UserFIO.GetFIO(_adminService.AuthorizedUser);
        }

        private void LoadProducts()
        {
            _productsCache = ProductViewModel.CreateCollectionFromProductList(dbContext.Products.ToList());
            Products.Clear();
            Products.AddRange(_productsCache);
        }


        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(SearchText) || propertyName == nameof(SelectedFilter))
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    LoadProductsToViewItems();
                    ApplyFilter();
                    return;
                }
                ApplyFilter();
                LoadProductsToViewItems(Products.Search(SearchText));
                ApplySort();
            }
            else if (propertyName == nameof(SelectedSort))
                ApplySort();
            base.OnPropertyChanged(propertyName);
        }

        public void ApplySort()
        {
            if (SelectedSort.SortProduct == SortProduct.Desc)
                Products = new BindableCollection<ProductViewModel>(Products.OrderByDescending(item => item.Product.ProductCost));
            else  // Ascending
                Products = new BindableCollection<ProductViewModel>(Products.OrderBy(item => item.Product.ProductCost));
        }

        public void ApplyFilter()
        {
            switch (SelectedFilter.FilterProduct)
            {
                case FilterProduct.All:
                    LoadProductsToViewItems();
                    break;
                case FilterProduct.Low:
                    Products = new BindableCollection<ProductViewModel>(_productsCache.Where(
                        item => item.Product.ProductDiscountAmount < 10));
                    break;
                case FilterProduct.Medium:
                    Products = new BindableCollection<ProductViewModel>(_productsCache.Where(
                        item => item.Product.ProductDiscountAmount >= 10 && item.Product.ProductDiscountAmount < 15));
                    break;
                case FilterProduct.High:
                    Products = new BindableCollection<ProductViewModel>(_productsCache.Where(
                        item => item.Product.ProductDiscountAmount >= 15));
                    break;
            }
        }

        public async void AddProduct() {
            var result = await _dialogManager.ShowDialogAsync(
                _viewModelFactory.CreateAddProductViewModel()
                );
        }
        public async void EditProduct(ProductViewModel product)
        {
            var result = await _dialogManager.ShowDialogAsync(
                _viewModelFactory.CreateEditProductViewModel(product)
                );
            if (result.Canceled) return;
            else
            {
                product.Product.ProductDiscountAmount = (sbyte)result.Discount;
                product.Product.ProductQuantityInStock = result.Quantity;
                product.Refresh();
                var _product = dbContext.Products.Find(product.Product.ProductArticleNumber);
                _product.ProductDiscountAmount = (sbyte)result.Discount;
                _product.ProductQuantityInStock = result.Quantity;
                await dbContext.SaveChangesAsync();
            }
        }
        private void LoadProductsToViewItems() => Products = new BindableCollection<ProductViewModel>(_productsCache);
        private void LoadProductsToViewItems(List<ProductViewModel> items) => Products = new BindableCollection<ProductViewModel>(items);
        protected override void OnActivate()
        {
            LoadProducts();
            MaxProductsCount = Products.Count;
            base.OnActivate();
        }
    }
}
