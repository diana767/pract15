using System.Globalization;
using System.Windows.Controls;

namespace Pract15.ValidationRules
{
    public class StockValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, "Введите количество");

            if (!double.TryParse(str, out double stock))
                return new ValidationResult(false, "Введите число");

            if (stock < 0)
                return new ValidationResult(false, "Количество не может быть отрицательным");

            if (stock > 10000)
                return new ValidationResult(false, "Слишком большое количество");

            return ValidationResult.ValidResult;
        }
    }
}