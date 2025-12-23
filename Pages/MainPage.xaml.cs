using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Pract15.Services;
using Pract15.ViewModels;
using Pract15.Windows;

namespace Pract15.Pages
{
    public partial class MainPage : Page
    {
        private MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadProducts();

            var itemsControl = FindVisualChild<ItemsControl>(this);
            if (itemsControl != null)
            {
                foreach (var item in itemsControl.Items)
                {
                    var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item);
                    if (container is ContentPresenter presenter && VisualTreeHelper.GetChildrenCount(presenter) > 0)
                    {
                        var border = VisualTreeHelper.GetChild(presenter, 0) as Border;
                        if (border != null)
                        {
                            border.MouseDown += ProductCard_MouseDown;
                            border.MouseEnter += ProductCard_MouseEnter;
                            border.MouseLeave += ProductCard_MouseLeave;
                        }
                    }
                }
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                    return result;

                var childResult = FindVisualChild<T>(child);
                if (childResult != null)
                    return childResult;
            }
            return null;
        }

        private void SortByNameAsc_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SortByNameAsc();
        }

        private void SortByNameDesc_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SortByNameDesc();
        }

        private void SortByPriceAsc_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SortByPriceAsc();
        }

        private void SortByPriceDesc_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SortByPriceDesc();
        }

        private void SortByStockAsc_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SortByStockAsc();
        }

        private void SortByStockDesc_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SortByStockDesc();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetFilters();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProductEditWindow();
            if (dialog.ShowDialog() == true)
            {
                _viewModel.LoadProducts();
                _viewModel.RefreshFilter();
            }
        }

        private void Manage_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new ManagementPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            AuthService.Logout();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.MainFrame.Navigate(new LoginPage());
        }

        private void ProductCard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                if (AuthService.IsManagerMode)
                {
                    var border = sender as Border;
                    if (border?.DataContext is Pract15.Models.Product product)
                    {
                        var dialog = new ProductEditWindow(product);
                        if (dialog.ShowDialog() == true)
                        {
                            _viewModel.LoadProducts();
                            _viewModel.RefreshFilter();
                        }
                    }
                }
            }
        }
        private void ProductCard_MouseEnter(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            if (border != null && border.DataContext is Pract15.Models.Product product)
            {
                border.Opacity = 0.9;

                if (product.Stock.HasValue && product.Stock.Value < 10)
                {
                    border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffd129"));
                    border.BorderThickness = new Thickness(2);
                }
            }
        }

        private void ProductCard_MouseLeave(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            if (border != null && border.DataContext is Pract15.Models.Product product)
            {
                border.Opacity = 1.0;

                if (product.Stock.HasValue && product.Stock.Value >= 10)
                {
                    border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E1E8ED"));
                    border.BorderThickness = new Thickness(1);
                }
            }
        }
    }
}