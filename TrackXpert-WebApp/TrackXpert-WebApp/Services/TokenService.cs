using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using TrackXpert_WebApp.Handlers;

namespace TrackXpert_WebApp.Services
{
	public class TokenService
	{
		public enum TokenType
		{
			ACCESS_TOKEN,
			REFRESH_TOKEN
		}
		private readonly ILocalStorageService _localStorage;
		private readonly AuthenticationStateService _authState;
		private readonly AuthenticationStateProvider _authenticationStateProvider;

		public TokenService(ILocalStorageService localStorageService, AuthenticationStateService tokenState, AuthenticationStateProvider authenticationStateProvider)
		{
			_localStorage = localStorageService;
			_authState = tokenState;
			_authenticationStateProvider = authenticationStateProvider;
		}

		public async Task InitializeTokenAsync()
		{
			string? accessToken = await _localStorage.GetItemAsync<string>("accessToken");

			_authState.SetAccessToken(accessToken);
			_authState.SetRefreshToken(await _localStorage.GetItemAsync<string>("refreshToken"));

			if (accessToken != null)
				LoadUserFromToken(accessToken);
		}

		public async Task SetTokenAsync(string? token, TokenType type)
		{
			if (token != null)
			{
				switch (type)
				{
					case TokenType.ACCESS_TOKEN:
						await _localStorage.SetItemAsync<string>("accessToken", token);
						_authState.SetAccessToken(token);
						break;
					case TokenType.REFRESH_TOKEN:
						await _localStorage.SetItemAsync<string>("refreshToken", token);
						_authState.SetRefreshToken(token);
						break;
				}
			}
		}

		public void LoadUserFromToken(string accessToken)
		{
			if (string.IsNullOrEmpty(accessToken))
			{
				_authState.SetUserIdentity(null);
				return;
			}

			var handler = new JwtSecurityTokenHandler();
			var jwt = handler.ReadJwtToken(accessToken);

			var identity = new ClaimsIdentity(jwt.Claims, "jwt");
			_authState.SetUserIdentity(new ClaimsPrincipal(identity));

            if (_authenticationStateProvider is CustomAuthenticationStateProvider authStateProvider)
            {
                authStateProvider.NotifyUserAuthentication();
            }
        }

		public async Task RemoveTokenAsync(string key, TokenType type)
		{
			await _localStorage.RemoveItemAsync(key);

			switch (type)
			{
				case TokenType.ACCESS_TOKEN:
					_authState.SetAccessToken(null);
					break;
				case TokenType.REFRESH_TOKEN:
					_authState.SetRefreshToken(null);
					break;
			}
		}

		public void ResetUserClaim()
		{
			_authState.SetUserIdentity(null);

            if (_authenticationStateProvider is CustomAuthenticationStateProvider authStateProvider)
            {
                authStateProvider.NotifyUserLogout();
            }
        }

		public string? GetToken(TokenType type)
		{
			switch (type)
			{
				case TokenType.ACCESS_TOKEN:
					return _authState.AccessToken != null ? _authState.AccessToken : null;

				case TokenType.REFRESH_TOKEN:
					return _authState.RefreshToken != null ? _authState.RefreshToken : null;
			}

			return null;

		}

		public bool IsTokenExpired(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			// Check if token is near expiration (e.g., within 1 minute)
			return jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(-1);
		}
	}
}
