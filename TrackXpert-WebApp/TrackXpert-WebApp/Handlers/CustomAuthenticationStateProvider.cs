using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using TrackXpert_WebApp.Services;

namespace TrackXpert_WebApp.Handlers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationStateService _authState;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(AuthenticationStateService authState)
        {
            _authState = authState;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = _authState.AccessToken;

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }

            var jwtToken = _tokenHandler.ReadJwtToken(savedToken);
            var claims = jwtToken.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public void NotifyUserAuthentication()
        {
            var identity = _authState.User;
            var authenticatedUser = new ClaimsPrincipal(identity!);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
