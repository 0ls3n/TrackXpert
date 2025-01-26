using System.ComponentModel.DataAnnotations;

namespace TrackXpert_WebApp.CustomValidation
{
    

    public class PasswordMatchAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public PasswordMatchAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = value?.ToString();
            var comparisonValue = validationContext.ObjectType
                .GetProperty(_comparisonProperty)?
                .GetValue(validationContext.ObjectInstance)?
                .ToString();

            if (currentValue != comparisonValue)
            {
                return new ValidationResult($"The {validationContext.DisplayName} does not match {_comparisonProperty}.");
            }

            return ValidationResult.Success!;
        }
    }

}
