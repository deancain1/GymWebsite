using Blazored.LocalStorage;
using Gym.Client.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Gym.Client.Security
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly AuthTokenProvider _tokenProvider;
        private readonly IHttpClientFactory _httpClientFactory;
       

        public AuthTokenHandler( AuthTokenProvider tokenProvider, IHttpClientFactory httpClientFactory)
        {
            _tokenProvider = tokenProvider;
            _httpClientFactory = httpClientFactory;
           
        }

        protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokenProvider.AccessToken;

            if (!string.IsNullOrEmpty(token) && _tokenProvider.IsAccessTokenExpiringSoon(60))
            {
                await RefreshTokenAsync();  
                token = _tokenProvider.AccessToken;
            }

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RefreshTokenAsync();
                token = _tokenProvider.AccessToken;

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            return response;
        }

        private async Task RefreshTokenAsync()
        {
            var refreshToken = _tokenProvider.RefreshToken;
            if (string.IsNullOrEmpty(refreshToken)) return;

            var client = _httpClientFactory.CreateClient("NoAuth");

            var refreshRequest = new RefreshTokenRequest
            {
                Token = _tokenProvider.AccessToken,
                RefreshToken = refreshToken
            };

            var refreshResponse = await client.PostAsJsonAsync("api/Auth/refresh", refreshRequest);

            if (!refreshResponse.IsSuccessStatusCode) return;

            var newTokens = await refreshResponse.Content.ReadFromJsonAsync<TokenResponse>();
            if (newTokens == null || string.IsNullOrEmpty(newTokens.Token)) return;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(newTokens.Token);

            _tokenProvider.SetTokens(newTokens.Token, newTokens.RefreshToken, jwtToken.ValidTo);
        }

    }
}