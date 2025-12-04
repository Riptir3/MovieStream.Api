using System.ComponentModel.DataAnnotations;

namespace MovieStream.Api.Attributes
{
    public class AllowedMovieCategoryValuesAttribute : ValidationAttribute
    {

        private readonly HashSet<string> _allowedValues;
        public AllowedMovieCategoryValuesAttribute(Type enumType) {
            _allowedValues = Enum.GetNames(enumType).ToHashSet();
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null || !_allowedValues.Contains(value.ToString()))
                return new ValidationResult($"Invalid value. Allowed: {string.Join(", ", _allowedValues)}");

            return ValidationResult.Success;
        }
    }
}
