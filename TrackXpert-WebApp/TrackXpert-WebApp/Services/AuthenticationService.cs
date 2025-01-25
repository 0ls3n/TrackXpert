using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using TrackXpert_WebApp.Components.Pages.Auth;
using TrackXpert_WebApp.Handlers;

namespace TrackXpert_WebApp.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly HttpClient _client;
		private readonly TokenService _tokenService;

		public AuthenticationService(IHttpClientFactory httpClientFactory, TokenService tokenService)
		{
			_client = httpClientFactory.CreateClient("AuthClient");
			_tokenService = tokenService;
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

					await _tokenService.SetTokenAsync(token.AccessToken, TokenService.TokenType.ACCESS_TOKEN);
					await _tokenService.SetTokenAsync(token.RefreshToken, TokenService.TokenType.REFRESH_TOKEN);

					_tokenService.LoadUserFromToken(token.AccessToken);

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
			await _tokenService.RemoveTokenAsync("accessToken", TokenService.TokenType.ACCESS_TOKEN);
			await _tokenService.RemoveTokenAsync("refreshToken", TokenService.TokenType.REFRESH_TOKEN);

			_tokenService.ResetUserClaim();

		}

		public async Task<string> RefreshTokenAsync()
		{
			var refreshToken = _tokenService.GetToken(TokenService.TokenType.REFRESH_TOKEN);
			if (string.IsNullOrEmpty(refreshToken)) return "No refresh token found.";

			try
			{
				var message = new StringContent(JsonSerializer.Serialize(new { refreshToken }), System.Text.Encoding.UTF8, "application/json");
				var response = await _client.PostAsync("refresh", message);

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var token = JsonSerializer.Deserialize<UserToken>(result);

					await _tokenService.SetTokenAsync(token.AccessToken, TokenService.TokenType.ACCESS_TOKEN);
					await _tokenService.SetTokenAsync(token.RefreshToken, TokenService.TokenType.REFRESH_TOKEN);

					_tokenService.LoadUserFromToken(token.AccessToken);

					return token.AccessToken;
				}

				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		struct UserToken
		{

			[JsonPropertyName("accessToken")]
			public string AccessToken { get; set; }

			[JsonPropertyName("refreshToken")]
			public string RefreshToken { get; set; }
		}
	}
}
