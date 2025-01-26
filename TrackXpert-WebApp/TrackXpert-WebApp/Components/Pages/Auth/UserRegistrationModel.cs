using System.ComponentModel.DataAnnotations;
using TrackXpert_WebApp.CustomValidation;

namespace TrackXpert_WebApp.Components.Pages.Auth
{
    public class UserRegistrationModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [PasswordMatch("Password", ErrorMessage = "Passwords do not match.")]
        public string? PasswordConfirmation { get; set; }

        [Required]
        public string? Firstname { get; set; }

        [Required]
        public string? Lastname { get; set; }

        [Required]
        public string? Displayname { get; set; }

        [Required]
        [AgreeTerms]
        public bool AgreeTerms { get; set; }
    }
}
