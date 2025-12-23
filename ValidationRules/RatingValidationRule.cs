using System.Globalization;
using System.Windows.Controls;

namespace Pract15.ValidationRules
{
    public class RatingValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(str))
                return ValidationResult.ValidResult;

            if (!double.TryParse(str, out double rating))
                return new ValidationResult(false, "Введите число от 0 до 5");

            if (rating < 0 || rating > 5)
                return new ValidationResult(false, "Рейтинг должен быть от 0 до 5");

            return ValidationResult.ValidResult;
        }
    }
}