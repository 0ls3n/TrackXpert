using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace TrackXpert_WebApp.Services;

public class AuthenticationStateService
{

    public ClaimsPrincipal? User { get; private set; }
    public string? AccessToken {get; private set;}
    public string? RefreshToken {get; private set;}

    public void SetAccessToken(string? token)
    {
        AccessToken = token;
    }

    public void SetRefreshToken(string? token)
    {
        RefreshToken = token;
    }

    public void SetUserIdentity(ClaimsPrincipal? user)
    {
        User = user;
    }
}
