using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using TrackXpert_WebApp.Components;
using TrackXpert_WebApp.Services;
using TrackXpert_WebApp.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddHttpClient("AuthClient", client =>
{
	client.BaseAddress = new Uri("https://localhost:7048/auth/");
});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AuthorizationMessageHandler>();

builder.Services.AddHttpClient("TrackClient", client =>
{
	client.BaseAddress = new Uri("https://localhost:7048/api/tracks");
}).AddHttpMessageHandler<AuthorizationMessageHandler>();


builder.Services.AddTransient<IUploadService, UploadService>();


var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();



app.Run();
