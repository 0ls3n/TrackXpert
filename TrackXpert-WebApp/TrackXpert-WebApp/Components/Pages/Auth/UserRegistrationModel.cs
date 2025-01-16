using System.ComponentModel.DataAnnotations;

namespace TrackXpert_WebApp.Components.Pages.Auth
{
	public class UserRegistrationModel
	{

		[Required]
		[EmailAddress]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }

		[Required]
		public string? PasswordConfirmation { get; set; }

		[Required]
		public bool AgreeTerms { get; set; }
	}
}
