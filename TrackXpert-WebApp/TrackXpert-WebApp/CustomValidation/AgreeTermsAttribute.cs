using System.ComponentModel.DataAnnotations;

namespace TrackXpert_WebApp.CustomValidation
{
    public class AgreeTermsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if ((bool)value! != true)
            {
                return new ValidationResult("You need to agree to the terms to register a new account");
            }
            return ValidationResult.Success!;
        }
    }
}
