using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Pract15.Models;
using Pract15.Services;
using Pract15.Windows;

namespace Pract15.Pages
{
    public partial class ManagementPage : Page
    {
        private Pract15Context _db;

        public ManagementPage()
        {
            InitializeComponent();
            _db = new Pract15Context();
            LoadData();
        }

        private void LoadData()
        {
            LoadProducts();
            LoadCategories();
            LoadBrands();
            LoadTags();
        }

        private void LoadProducts()
        {
            var products = _db.Products
                .AsNoTracking() 
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToList();
            ProductsGrid.ItemsSource = products;
        }

        private void LoadCategories()
        {
            CategoriesGrid.ItemsSource = _db.Categories.AsNoTracking().ToList(); 
        }

        private void LoadBrands()
        {
            BrandsGrid.ItemsSource = _db.Brands.AsNoTracking().ToList(); 
        }

        private void LoadTags()
        {
            TagsGrid.ItemsSource = _db.Tags.AsNoTracking().ToList(); 
        }

       
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProductEditWindow();
            if (dialog.ShowDialog() == true)
            {
                LoadProducts();
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id) 
            {
                var product = _db.Products
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .FirstOrDefault(p => p.Id == id);

                if (product != null)
                {
                    var dialog = new ProductEditWindow(product);
                    if (dialog.ShowDialog() == true)
                    {
                        LoadProducts();
                    }
                }
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить этот товар?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (sender is Button button && button.Tag is int id) 
                {
                    var product = _db.Products.Find(id);
                    if (product != null)
                    {
                        _db.Products.Remove(product);
                        _db.SaveChanges();
                        LoadProducts();
                    }
                }
            }
        }

        private void RefreshProducts_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }
        
        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SimpleEditWindow("Добавить категорию", "Название категории");
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.Value))
            {
                using (var db = new Pract15Context())
                {
                    var category = new Category { Name = dialog.Value };
                    db.Categories.Add(category);
                    db.SaveChanges();
                }
                LoadCategories();
            }
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id) 
            {
                var category = _db.Categories.Find(id);
                if (category != null)
                {
                    var dialog = new SimpleEditWindow("Редактировать категорию", "Название категории", category.Name);
                    if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.Value))
                    {
                        category.Name = dialog.Value;
                        _db.SaveChanges();
                        LoadCategories();
                    }
                }
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить эту категорию? Все товары этой категории станут без категории.",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (sender is Button button && button.Tag is int id) 
                {
                    var category = _db.Categories.Find(id);
                    if (category != null)
                    {
                        _db.Categories.Remove(category);
                        _db.SaveChanges();
                        LoadCategories();
                    }
                }
            }
        }

        private void RefreshCategories_Click(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }

      
        private void AddBrand_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SimpleEditWindow("Добавить бренд", "Название бренд");
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.Value))
            {
                using (var db = new Pract15Context())
                {
                    var brand = new Brand { Name = dialog.Value };
                    db.Brands.Add(brand);
                    db.SaveChanges();
                }
                LoadBrands();
            }
        }
        private void EditBrand_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id) 
            {
                var brand = _db.Brands.Find(id);
                if (brand != null)
                {
                    var dialog = new SimpleEditWindow("Редактировать бренд", "Название бренда", brand.Name);
                    if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.Value))
                    {
                        brand.Name = dialog.Value;
                        _db.SaveChanges();
                        LoadBrands();
                    }
                }
            }
        }

        private void DeleteBrand_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить этот бренд? Все товары этого бренда станут без бренда.",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (sender is Button button && button.Tag is int id) 
                {
                    var brand = _db.Brands.Find(id);
                    if (brand != null)
                    {
                        _db.Brands.Remove(brand);
                        _db.SaveChanges();
                        LoadBrands();
                    }
                }
            }
        }

        private void RefreshBrands_Click(object sender, RoutedEventArgs e)
        {
            LoadBrands();
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SimpleEditWindow("Добавить тег", "Название тега");
            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.Value))
            {
                using (var db = new Pract15Context())
                {
                   
                    var tag = new Tag { Name = dialog.Value };
                    db.Tags.Add(tag);
                    db.SaveChanges();
                }
                LoadTags();
            }
        }
        private void EditTag_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id) 
            {
                var tag = _db.Tags.Find(id);
                if (tag != null)
                {
                    var dialog = new SimpleEditWindow("Редактировать тег", "Название тега", tag.Name);
                    if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.Value))
                    {
                        tag.Name = dialog.Value;
                        _db.SaveChanges();
                        LoadTags();
                    }
                }
            }
        }

        private void DeleteTag_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить этот тег?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (sender is Button button && button.Tag is int id) 
                {
                    var tag = _db.Tags.Find(id);
                    if (tag != null)
                    {
                        _db.Tags.Remove(tag);
                        _db.SaveChanges();
                        LoadTags();
                    }
                }
            }
        }

        private void RefreshTags_Click(object sender, RoutedEventArgs e)
        {
            LoadTags();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new MainPage());
        }
    }
}