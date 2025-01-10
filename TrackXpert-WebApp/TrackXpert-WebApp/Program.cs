using Microsoft.AspNetCore.Components.Authorization;
using TrackXpert_WebApp.Components;
using TrackXpert_WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient("TrackClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7048/api/tracks");
});

builder.Services.AddTransient<IUploadService, UploadService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
