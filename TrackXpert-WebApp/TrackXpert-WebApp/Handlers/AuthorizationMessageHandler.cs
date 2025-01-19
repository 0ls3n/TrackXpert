using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TrackXpert_WebApp.Services;

namespace TrackXpert_WebApp.Handlers
{
	public class AuthorizationMessageHandler : DelegatingHandler
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly TokenService _tokenService;

		public AuthorizationMessageHandler(IAuthenticationService authenticationService, TokenService tokenService)
		{
			_authenticationService = authenticationService;
			_tokenService = tokenService;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string? token = _tokenService.GetToken(TokenService.TokenType.ACCESS_TOKEN);

			if (!string.IsNullOrEmpty(token) && _tokenService.IsTokenExpired(token))
			{
				token = await _authenticationService.RefreshTokenAsync();
			}

			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
