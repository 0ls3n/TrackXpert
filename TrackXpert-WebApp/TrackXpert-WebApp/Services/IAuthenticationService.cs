using TrackXpert_WebApp.Components.Pages.Auth;

namespace TrackXpert_WebApp.Services
{
	public interface IAuthenticationService
	{
		public bool IsAuthorized { get; }
		public Task<string> LoginAsync(UserLoginModel user);
		public Task<string> RegisterAsync(UserRegistrationModel user);
		public Task<string> RefreshToken();

		public bool IsTokenExpired(string token);
		public Task SignOut();
	}
}
