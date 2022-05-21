using Blazored.LocalStorage;
using Metanet.Client.Helpers;
using Metanet.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Metanet.Client.Services.Auth
{
    public class AuthState : AuthenticationStateProvider
    {
        private HttpClient http;
        private ILocalStorageService localStorage;


        public AuthState(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            http = httpClient;
            localStorage = localStorageService;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItemAsync<string>(Constants.Token);
            if(string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            else
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JWTParser.ParseClaimsFromJwt(token),"jwtAuthType")));
            }
            
        }

        public void NotifyUserLoggedIn(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JWTParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
