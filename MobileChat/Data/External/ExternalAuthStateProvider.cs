using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace MobileChat.Data.External;

public class ExternalAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
    private bool _isAuthenticated;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_isAuthenticated == false)
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));

        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(_currentUser)));
    }

    public Task LogInAsync(bool autenticado, string? user = null)
    {
        _isAuthenticated = autenticado;

        var claimsIdentity = new ClaimsIdentity([new Claim(ClaimTypes.Name, user)], "apiauth");

        _currentUser = new ClaimsPrincipal(claimsIdentity);

        // Notify Blazor of the authentication state change
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        return Task.CompletedTask;
    }

    public Task Logout()
    {
        _isAuthenticated = false;

        var result = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        NotifyAuthenticationStateChanged(Task.FromResult(result));
        return Task.CompletedTask;
    }
}