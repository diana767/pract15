using System.Globalization;
using System.Windows.Controls;

namespace Pract15.ValidationRules
{
    public class PriceValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, "Введите цену");

            if (!double.TryParse(str, out double price))
                return new ValidationResult(false, "Введите число");

            if (price <= 0)
                return new ValidationResult(false, "Цена должна быть больше 0");

            return ValidationResult.ValidResult;
        }
    }
}