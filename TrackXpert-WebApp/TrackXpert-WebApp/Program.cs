using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using TrackXpert_WebApp.Components;
using TrackXpert_WebApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TrackXpert_WebApp.Handlers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = "https://localhost:7048",
		ValidAudience = "https://localhost:7048",
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("NuJRxNzoZPkPxrlSjzeuPD5HP2JPP9LigW4OpvanKwEQRo2OM2B73T64pNLfbi9QttxkGMZAqjl42Ub"))
	};
});


builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<AuthenticationStateService>();
builder.Services.AddScoped<TokenService>();


builder.Services.AddHttpClient("AuthClient", client =>
{
	client.BaseAddress = new Uri("https://localhost:7048/api/auth/");
});

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();


builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddHttpClient("TrackClient", client =>
{
	client.BaseAddress = new Uri("https://localhost:7048/api/tracks");
}).AddHttpMessageHandler<AuthorizationMessageHandler>();


builder.Services.AddTransient<IUploadService, UploadService>();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();



app.Run();
