using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using Pract15.Models;
using Pract15.Services;

namespace Pract15.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Pract15Context _db;
        private string _searchQuery = "";
        private int? _selectedCategoryId; 
        private int? _selectedBrandId;    
        private double? _priceFrom;
        private double? _priceTo;
        private ICollectionView _productsView;
        private ObservableCollection<Product> _products;

        public MainViewModel()
        {
            _db = new Pract15Context(); 
            _products = new ObservableCollection<Product>();

            LoadCategories();
            LoadBrands();
            LoadTags();

            _productsView = CollectionViewSource.GetDefaultView(_products);
            _productsView.Filter = FilterProduct;

            UserRole = AuthService.IsManagerMode ? "Менеджер" : "Посетитель";
            IsManagerMode = AuthService.IsManagerMode;
        }

        public ObservableCollection<Product> Products => _products;
        public ICollectionView ProductsView => _productsView;
        public ObservableCollection<Category> Categories { get; } = new();
        public ObservableCollection<Brand> Brands { get; } = new();
        public ObservableCollection<Tag> Tags { get; } = new();

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    RefreshFilter();
                }
            }
        }

        public int? SelectedCategoryId 
        {
            get => _selectedCategoryId;
            set
            {
                if (_selectedCategoryId != value)
                {
                    _selectedCategoryId = value;
                    OnPropertyChanged();
                    RefreshFilter();
                }
            }
        }

        public int? SelectedBrandId 
        {
            get => _selectedBrandId;
            set
            {
                if (_selectedBrandId != value)
                {
                    _selectedBrandId = value;
                    OnPropertyChanged();
                    RefreshFilter();
                }
            }
        }

        public string PriceFrom
        {
            get => _priceFrom?.ToString();
            set
            {
                if (double.TryParse(value, out double result))
                {
                    _priceFrom = result;
                }
                else if (string.IsNullOrEmpty(value))
                {
                    _priceFrom = null;
                }
                OnPropertyChanged();
                RefreshFilter();
            }
        }

        public string PriceTo
        {
            get => _priceTo?.ToString();
            set
            {
                if (double.TryParse(value, out double result))
                {
                    _priceTo = result;
                }
                else if (string.IsNullOrEmpty(value))
                {
                    _priceTo = null;
                }
                OnPropertyChanged();
                RefreshFilter();
            }
        }

        public int TotalProductsCount => _products.Count;
        public int DisplayedProductsCount => _productsView?.Cast<object>().Count() ?? 0;

       
        public string UserRole { get; }
        public bool IsManagerMode { get; }


        public void SortByNameAsc()
        {
            _productsView.SortDescriptions.Clear();
            _productsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _productsView.Refresh();
        }

        public void SortByNameDesc()
        {
            _productsView.SortDescriptions.Clear();
            _productsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
            _productsView.Refresh();
        }

        public void SortByPriceAsc()
        {
            _productsView.SortDescriptions.Clear();
            _productsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
            _productsView.Refresh();
        }

        public void SortByPriceDesc()
        {
            _productsView.SortDescriptions.Clear();
            _productsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
            _productsView.Refresh();
        }

        public void SortByStockAsc()
        {
            _productsView.SortDescriptions.Clear();
            _productsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Ascending));
            _productsView.Refresh();
        }

        public void SortByStockDesc()
        {
            _productsView.SortDescriptions.Clear();
            _productsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Descending));
            _productsView.Refresh();
        }

        public void ResetFilters()
        {
            SearchQuery = "";
            SelectedCategoryId = null;
            SelectedBrandId = null;
            PriceFrom = "";
            PriceTo = "";
            _productsView.SortDescriptions.Clear();
            _productsView.Refresh();
            OnPropertyChanged(nameof(TotalProductsCount));
            OnPropertyChanged(nameof(DisplayedProductsCount));
        }

       
        public void LoadProducts()
        {
            _products.Clear();

            var products = _db.Products
                .AsNoTracking() 
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .ToList();

            foreach (var product in products)
            {
                _products.Add(product);
            }

            RefreshFilter();
        }

        private void LoadCategories()
        {
            Categories.Clear();
            var categories = _db.Categories.AsNoTracking().ToList(); 
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private void LoadBrands()
        {
            Brands.Clear();
            var brands = _db.Brands.AsNoTracking().ToList(); 
            foreach (var brand in brands)
            {
                Brands.Add(brand);
            }
        }

        private void LoadTags()
        {
            Tags.Clear();
            var tags = _db.Tags.AsNoTracking().ToList(); 
            foreach (var tag in tags)
            {
                Tags.Add(tag);
            }
        }

     
        private bool FilterProduct(object obj)
        {
            if (obj is not Product product)
                return false;

            if (!string.IsNullOrEmpty(SearchQuery) &&
                !product.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                return false;

            if (SelectedCategoryId.HasValue && product.CategoryId != SelectedCategoryId.Value)
                return false;

            if (SelectedBrandId.HasValue && product.BrandId != SelectedBrandId.Value)
                return false;

            if (_priceFrom.HasValue && product.Price < _priceFrom.Value)
                return false;

            if (_priceTo.HasValue && product.Price > _priceTo.Value)
                return false;

            return true;
        }

        public void RefreshFilter()
        {
            _productsView?.Refresh();
            OnPropertyChanged(nameof(DisplayedProductsCount));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}