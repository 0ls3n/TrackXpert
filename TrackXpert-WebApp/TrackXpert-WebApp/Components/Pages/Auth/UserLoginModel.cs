using System.ComponentModel.DataAnnotations;

namespace TrackXpert_WebApp.Components.Pages.Auth
{
	public class UserLoginModel
	{
		[Required]
		[EmailAddress]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
	}
}
