using System.ComponentModel.DataAnnotations;

namespace MovieStream.Api.Attributes
{
    public class ReleaseYearAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is int year)
            {
                return year >= 1900 && year <= DateTime.Now.Year;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be between 1900 and {DateTime.Now.Year}.";
        }
    }
}
