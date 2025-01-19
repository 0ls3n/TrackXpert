using System.Security.Claims;
using TrackXpert_WebApp.Components.Pages.Auth;

namespace TrackXpert_WebApp.Services
{
	public interface IAuthenticationService
	{
		public Task<string> LoginAsync(UserLoginModel user);
		public Task<string> RegisterAsync(UserRegistrationModel user);
		public Task<string> RefreshTokenAsync();
		public Task SignOut();
	}
}
