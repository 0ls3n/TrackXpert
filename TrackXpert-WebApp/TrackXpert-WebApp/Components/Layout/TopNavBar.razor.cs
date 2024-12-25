using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace TrackXpert_WebApp.Components.Layout;

public partial class TopNavBar : ComponentBase
{
    private string? currentUrl;

    [Inject]
    public NavigationManager? NavigationManager {get; set;}

    protected override void OnInitialized()
    {
        currentUrl = NavigationManager!.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = NavigationManager!.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    public void Dispose()
    {
        NavigationManager!.LocationChanged -= OnLocationChanged;
    }
}
