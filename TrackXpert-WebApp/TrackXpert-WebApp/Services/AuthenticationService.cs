using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using TrackXpert_WebApp.Components.Pages.Auth;

namespace TrackXpert_WebApp.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		public bool IsAuthorized { get; private set; }
		public ClaimsPrincipal? User { get; private set; }

		private readonly HttpClient _client;
		private readonly ILocalStorageService _localStorageService;

		public AuthenticationService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
		{
			_client = httpClientFactory.CreateClient("AuthClient");
			_localStorageService = localStorageService;
		}

		public async Task<string> LoginAsync(UserLoginModel user)
		{
			string data = JsonSerializer.Serialize(user);
			var message = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

			try
			{
				var response = await _client.PostAsync("login", message);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();

					var token = JsonSerializer.Deserialize<UserToken>(result);

					await _localStorageService.SetItemAsync("accessToken", token.AccessToken);
					await _localStorageService.SetItemAsync("refreshToken", token.RefreshToken);

					LoadUserFromToken(token.AccessToken);

					return "success";
				}
				else
				{
					var errorResponse = await response.Content.ReadAsStringAsync();
					return errorResponse;
				}
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public async Task<string> RegisterAsync(UserRegistrationModel user)
		{
			var data = JsonSerializer.Serialize(user);
			var message = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

			try
			{
				var response = await _client.PostAsync("register", message);
				if (response.IsSuccessStatusCode)
				{
					return "success";
				}
				else
				{
					var errorResponse = await response.Content.ReadAsStringAsync();

					return errorResponse;
				}
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public async Task SignOut()
		{
			await _localStorageService.RemoveItemAsync("accessToken");
			await _localStorageService.RemoveItemAsync("refreshToken");

			User = null;
			IsAuthorized = false;
		}

		private void LoadUserFromToken(string accessToken)
		{
			if (string.IsNullOrEmpty(accessToken))
			{
				IsAuthorized = false;
				User = null;
				return;
			}

			var handler = new JwtSecurityTokenHandler();
			var jwt = handler.ReadJwtToken(accessToken);

			var identity = new ClaimsIdentity(jwt.Claims, "jwt");
			User = new ClaimsPrincipal(identity);

			IsAuthorized = true;
		}

		public async Task<string> RefreshToken()
		{
			var refreshToken = await _localStorageService.GetItemAsync<string>("refreshToken");
			if (string.IsNullOrEmpty(refreshToken)) return "No refresh token found.";

			try
			{
				var message = new StringContent(JsonSerializer.Serialize(new { refreshToken }), System.Text.Encoding.UTF8, "application/json");
				var response = await _client.PostAsync("refresh", message);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var token = JsonSerializer.Deserialize<UserToken>(result);

					await _localStorageService.SetItemAsync("authToken", token.AccessToken);
					await _localStorageService.SetItemAsync("refreshToken", token.RefreshToken);

					LoadUserFromToken(token.AccessToken);

					return token.AccessToken;
				}

				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public bool IsTokenExpired(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);

			// Check if token is near expiration (e.g., within 1 minute)
			return jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(-1);
		}

		struct UserToken
		{
			[JsonPropertyName("tokenType")]
			public string TokenType { get; set; }

			[JsonPropertyName("accessToken")]
			public string AccessToken { get; set; }

			[JsonPropertyName("expiresIn")]
			public int ExpiresIn { get; set; }

			[JsonPropertyName("refreshToken")]
			public string RefreshToken { get; set; }
		}
	}
}
