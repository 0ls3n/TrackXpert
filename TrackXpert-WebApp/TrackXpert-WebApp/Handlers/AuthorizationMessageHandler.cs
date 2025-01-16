using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using TrackXpert_WebApp.Services;

namespace TrackXpert_WebApp.Handlers
{
	public class AuthorizationMessageHandler : DelegatingHandler
	{
		private readonly ILocalStorageService _localStorageService;
		private readonly IAuthenticationService _authenticationService;

		public AuthorizationMessageHandler(ILocalStorageService localStorageService, IAuthenticationService authenticationService)
		{
			_localStorageService = localStorageService;
			_authenticationService = authenticationService;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _localStorageService.GetItemAsync<string>("accessToken");

			if (!string.IsNullOrEmpty(token) && _authenticationService.IsTokenExpired(token))
			{
				token = await _authenticationService.RefreshToken();
			}

			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
