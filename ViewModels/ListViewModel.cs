using Stylet;
using System.Collections.Generic;
using System.Linq;
using WriteDry.Services;
using WriteDry.ViewModels.Component;

namespace WriteDry.ViewModels {
	public class FilterItem {
		public string Title { get; set; }
		public FilterProduct FilterProduct { get; set; }
	}
	public enum FilterProduct {
		All, Low, Medium, High
	}

	public class SortItem {
		public string Title { get; set; }
		public SortProduct SortProduct { get; set; }
	}
	public enum SortProduct {
		Desc, Asc
	}

	public class ListViewModel : Screen {
		public bool CanCreateOrder { get; set; }
		public string UserName { get; set; }
		public int MaxProductsCount { get; set; }
		public string SearchText { get; set; }
		public SortItem SelectedSort { get; set; } = new SortItem { SortProduct = SortProduct.Desc };
		public FilterItem SelectedFilter { get; set; } = new FilterItem { FilterProduct = FilterProduct.All };
		public BindableCollection<ProductViewModel> Products { get; set; } = new BindableCollection<ProductViewModel>();

		private NavigationController navigationController;
		private ApplicationContext dbContext;
		private List<ProductViewModel> _productsCache; //data from db

		public ListViewModel(NavigationController _navController, UserService userService, ApplicationContext dbContext) {
			navigationController = _navController;
			this.dbContext = dbContext;
			userService.onAuthChanged += HandleAuthState;
		}

		private void HandleAuthState(object sender, UserService.AuthArgs e) {
			if (e.Failed) return;
			UserName = e.isGuest ? "Гость" : e.newUserAuth.UserName;
		}

		private void LoadProducts() {
			_productsCache = ProductViewModel.CreateCollectionFromProductList(dbContext.Products.ToList());
			Products.Clear();
			Products.AddRange(_productsCache);
		}

		private void LoadProductsToViewItems() => Products = new BindableCollection<ProductViewModel>(_productsCache);

		protected override void OnPropertyChanged(string propertyName) {
			if (propertyName == nameof(SearchText) || propertyName == nameof(SelectedFilter)) {
				if (string.IsNullOrEmpty(SearchText)) {
					LoadProductsToViewItems();
					ApplyFilter();
					return;
				}
				ApplyFilter();
				Products = new BindableCollection<ProductViewModel>((
					from p in Products
					where p.Product.ProductNameNavigation.ProductName.ToLower().Contains(SearchText.ToLower())
					select p
					).ToList());
				ApplySort();
			}
			else if (propertyName == nameof(SelectedSort))
				ApplySort();
			base.OnPropertyChanged(propertyName);
		}

		public void ApplySort() {
			if (SelectedSort.SortProduct == SortProduct.Desc)
				Products = new BindableCollection<ProductViewModel>(Products.OrderByDescending(item => item.Product.ProductCost));
			else  // Ascending
				Products = new BindableCollection<ProductViewModel>(Products.OrderBy(item => item.Product.ProductCost));
		}

		public void ApplyFilter() {
			switch (SelectedFilter.FilterProduct) {
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

		public void GoToOrders() => navigationController.NavigateToOrders();

		protected override void OnActivate() {
			LoadProducts();
			MaxProductsCount = Products.Count;
			base.OnActivate();
		}
	}
}