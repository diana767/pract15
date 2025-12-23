using System.ComponentModel;
using System.Windows;

namespace Pract15.Windows
{
    public partial class SimpleEditWindow : Window, INotifyPropertyChanged
    {
        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public SimpleEditWindow(string title, string label, string initialValue = "")
        {
            InitializeComponent();
            Title = title;
            LabelText.Text = label;
            Value = initialValue;
            DataContext = this;
            ValueTextBox.Focus();
            ValueTextBox.SelectAll();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Value = Value?.Trim();

            if (string.IsNullOrWhiteSpace(Value) || Value.Length < 2)
            {
                MessageBox.Show("Поле должно содержать минимум 2 символа", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                ValueTextBox.Focus();
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ValueTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Save_Click(sender, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}