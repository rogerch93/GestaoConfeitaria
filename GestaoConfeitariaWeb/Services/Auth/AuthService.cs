using GestaoConfeitaria.Application.Interfaces;
using GestaoConfeitaria.Shared.DTOs;

namespace GestaoConfeitariaWeb.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(UserLoginDto dto);
        Task LogoutAsync();
        Task<RegisterResult> RegisterAsync(RegisterRequestDto dto);
    }

    public class AuthService: IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("Api");
        }

        public async Task<LoginResult> LoginAsync(UserLoginDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);
            return await response.Content.ReadFromJsonAsync<LoginResult>() ?? new LoginResult { Success = false };
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("api/auth/logout", null);
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequestDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);
            return await response.Content.ReadFromJsonAsync<RegisterResult>() ?? new RegisterResult { Success = false };
        }
    }
}
