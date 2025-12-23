using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Pract15.Models;

namespace Pract15.Windows
{
    public partial class ProductEditWindow : Window, INotifyPropertyChanged
    {
        private Product _product;
        private bool _isEditMode;
        private string _productName;
        private string _productDescription;
        private double? _productPrice;
        private double? _productStock;
        private double? _productRating;
        private int? _selectedCategoryId;
        private int? _selectedBrandId;

        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(); }
        }

        public string ProductDescription
        {
            get => _productDescription;
            set { _productDescription = value; OnPropertyChanged(); }
        }

        public double? ProductPrice
        {
            get => _productPrice;
            set { _productPrice = value; OnPropertyChanged(); }
        }

        public double? ProductStock
        {
            get => _productStock;
            set { _productStock = value; OnPropertyChanged(); }
        }

        public double? ProductRating
        {
            get => _productRating;
            set { _productRating = value; OnPropertyChanged(); }
        }

        public int? SelectedCategoryId
        {
            get => _selectedCategoryId;
            set { _selectedCategoryId = value; OnPropertyChanged(); }
        }

        public int? SelectedBrandId
        {
            get => _selectedBrandId;
            set { _selectedBrandId = value; OnPropertyChanged(); }
        }

        public List<Category> Categories { get; private set; }
        public List<Brand> Brands { get; private set; }
        public List<TagViewModel> Tags { get; private set; }

        public ProductEditWindow(Product product = null)
        {
            InitializeComponent();
            DataContext = this;

            if (product != null)
            {
                _product = product;
                _isEditMode = true;
                Title = "Редактирование товара";
                ProductName = product.Name;
                ProductDescription = product.Description;
                ProductPrice = product.Price;
                ProductStock = product.Stock;
                ProductRating = product.Rating;
                SelectedCategoryId = product.CategoryId;
                SelectedBrandId = product.BrandId;
            }
            else
            {
                Title = "Добавление товара";
            }

            LoadData();
            NameTextBox.Focus();
        }

       private async void LoadData()
{
    try
    {
        using var db = new Pract15Context();

        Categories = await db.Categories
            .AsNoTracking()
            .ToListAsync();
        Brands = await db.Brands
            .AsNoTracking()
            .ToListAsync();
        var tags = await db.Tags
            .AsNoTracking()
            .ToListAsync();

        Tags = tags.Select(t => new TagViewModel
        {
            Id = t.Id,
            Name = t.Name,
            IsSelected = false
        }).ToList();

        if (_isEditMode && _product != null)
        {
            var productTags = await db.ProductTags
                .AsNoTracking()
                .Where(pt => pt.ProductId == _product.Id)
                .Select(pt => pt.TagId)
                .ToListAsync();

            foreach (var tag in Tags)
            {
                tag.IsSelected = productTags.Contains(tag.Id);
            }
        }

        OnPropertyChanged(nameof(Categories));
        OnPropertyChanged(nameof(Brands));
        OnPropertyChanged(nameof(Tags));

        if (_isEditMode && _product != null)
        {
            CategoryComboBox.SelectedValue = SelectedCategoryId;
            BrandComboBox.SelectedValue = SelectedBrandId;
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

        private bool ValidateForm()
        {
            ErrorText.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(ProductName) || ProductName.Length < 3)
            {
                ErrorText.Text = "Название должно содержать минимум 3 символа";
                ErrorText.Visibility = Visibility.Visible;
                NameTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(ProductDescription) || ProductDescription.Length < 10)
            {
                ErrorText.Text = "Описание должно содержать минимум 10 символов";
                ErrorText.Visibility = Visibility.Visible;
                DescriptionTextBox.Focus();
                return false;
            }

            if (!ProductPrice.HasValue || ProductPrice <= 0)
            {

                if (double.TryParse(PriceTextBox.Text, out double priceValue) && priceValue > 0)
                {
                    ProductPrice = priceValue;
                }
                else
                {
                    ErrorText.Text = "Цена должна быть положительным числом";
                    ErrorText.Visibility = Visibility.Visible;
                    PriceTextBox.Focus();
                    PriceTextBox.SelectAll();
                    return false;
                }
            }

            if (!ProductStock.HasValue || ProductStock < 0)
            {
                if (double.TryParse(StockTextBox.Text, out double stockValue) && stockValue >= 0)
                {
                    ProductStock = stockValue;
                }
                else
                {
                    ErrorText.Text = "Количество должно быть неотрицательным числом";
                    ErrorText.Visibility = Visibility.Visible;
                    StockTextBox.Focus();
                    StockTextBox.SelectAll();
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(RatingTextBox.Text))
            {
                if (!ProductRating.HasValue || ProductRating < 0 || ProductRating > 5)
                {
                    if (double.TryParse(RatingTextBox.Text, out double ratingValue) && ratingValue >= 0 && ratingValue <= 5)
                    {
                        ProductRating = ratingValue;
                    }
                    else
                    {
                        ErrorText.Text = "Рейтинг должен быть числом от 0 до 5";
                        ErrorText.Visibility = Visibility.Visible;
                        RatingTextBox.Focus();
                        RatingTextBox.SelectAll();
                        return false;
                    }
                }
            }
            else
            {
                ProductRating = null;
            }

            if (!SelectedCategoryId.HasValue || SelectedCategoryId == 0)
            {
                ErrorText.Text = "Выберите категорию";
                ErrorText.Visibility = Visibility.Visible;
                CategoryComboBox.Focus();
                return false;
            }

            if (!SelectedBrandId.HasValue || SelectedBrandId == 0)
            {
                ErrorText.Text = "Выберите бренд";
                ErrorText.Visibility = Visibility.Visible;
                BrandComboBox.Focus();
                return false;
            }

            return true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля корректно", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var db = new Pract15Context();

                if (_isEditMode && _product != null)
                {
                    var existingProduct = db.Products
                        .FirstOrDefault(p => p.Id == _product.Id);

                    if (existingProduct == null)
                    {
                        MessageBox.Show("Товар не найден", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    existingProduct.Name = ProductName;
                    existingProduct.Description = ProductDescription;
                    existingProduct.Price = ProductPrice ?? 0;
                    existingProduct.Stock = ProductStock ?? 0;
                    existingProduct.Rating = ProductRating;
                    existingProduct.CategoryId = SelectedCategoryId;
                    existingProduct.BrandId = SelectedBrandId;

                    db.SaveChanges();
                    _product = existingProduct;
                }
                else
                {
                    var product = new Product
                    {
                        Name = ProductName,
                        Description = ProductDescription,
                        Price = ProductPrice ?? 0,
                        Stock = ProductStock ?? 0,
                        Rating = ProductRating,
                        CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        CategoryId = SelectedCategoryId,
                        BrandId = SelectedBrandId
                    };

                    db.Products.Add(product);
                    db.SaveChanges();
                    _product = product;
                }

                var selectedTags = Tags
                    .Where(t => t.IsSelected)
                    .Select(t => t.Id)
                    .ToList();

                db.Database.ExecuteSqlRaw("DELETE FROM [product_tags$] WHERE product_id = {0}", _product.Id);

                foreach (var tagId in selectedTags)
                {
                    db.Database.ExecuteSqlRaw(
                        "INSERT INTO [product_tags$] (product_id, tag_id) VALUES ({0}, {1})",
                        _product.Id, tagId);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TagViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}