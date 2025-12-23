using System.Globalization;
using System.Windows.Controls;

namespace Pract15.ValidationRules
{
    public class RequiredValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace(value?.ToString())
                ? new ValidationResult(false, "Обязательное поле")
                : ValidationResult.ValidResult;
        }
    }
}