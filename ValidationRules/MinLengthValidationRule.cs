using System.Globalization;
using System.Windows.Controls;

namespace Pract15.ValidationRules
{
    public class MinLengthValidationRule : ValidationRule
    {
        public int MinLength { get; set; } = 2;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value?.ToString() ?? "";

            if (str.Length < MinLength)
            {
                return new ValidationResult(false, $"Минимальная длина: {MinLength} символа");
            }

            return ValidationResult.ValidResult;
        }
    }
}