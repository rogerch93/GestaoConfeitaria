using GestaoConfeitaria.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GestaoConfeitariaWeb.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private ClaimsPrincipal? _currentUser;

        public CustomAuthStateProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Tenta recuperar o usuário atual a partir do cookie (chamando a API)
            var client = _httpClientFactory.CreateClient("Api");

            try
            {
                // Endpoint simples que você pode criar na API para retornar o usuário logado
                var response = await client.GetAsync("api/auth/me");

                if (response.IsSuccessStatusCode)
                {
                    var userInfo = await response.Content.ReadFromJsonAsync<UserInfoDto>();

                    if (userInfo != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, userInfo.Username),
                            new Claim(ClaimTypes.Role, userInfo.Role),
                            new Claim("UserId", userInfo.Id.ToString())
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        _currentUser = new ClaimsPrincipal(identity);
                    }
                }
            }
            catch
            {
                // Se falhar (não logado ou cookie inválido), retorna usuário anônimo
                _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            }

            return new AuthenticationState(_currentUser ?? new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Método que você chamou na página de Login
        public async Task NotifyUserAuthenticationAsync(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            _currentUser = new ClaimsPrincipal(identity);

            // Notifica todos os componentes que o estado de autenticação mudou
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        // Método útil para logout
        public async Task NotifyUserLogoutAsync()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }
}