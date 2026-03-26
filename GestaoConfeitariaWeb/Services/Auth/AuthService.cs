using GestaoConfeitaria.Application.Interfaces;
using GestaoConfeitaria.Shared.DTOs;
using System.Net.Http.Json;
using LoginResult = GestaoConfeitaria.Shared.DTOs.LoginResult;

namespace GestaoConfeitariaWeb.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(UserLoginDto dto);
        Task LogoutAsync();
        Task<RegisterResult> RegisterAsync(RegisterRequestDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;   

        // Injetamos o client nomeado "AuthApi"
        public AuthService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("AuthApi");   
        }

        public async Task<LoginResult> LoginAsync(UserLoginDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new LoginResult
                    {
                        Success = false,
                        Message = $"Erro {response.StatusCode}: {errorContent}"
                    };
                }

                return await response.Content.ReadFromJsonAsync<LoginResult>()
                       ?? new LoginResult { Success = false, Message = "Resposta inválida da API" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no LoginAsync: {ex}");
                return new LoginResult { Success = false, Message = "Erro de conexão com o servidor" };
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _httpClient.PostAsync("api/auth/logout", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no Logout: {ex}");
            }
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequestDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);
            return await response.Content.ReadFromJsonAsync<RegisterResult>()
                   ?? new RegisterResult { Success = false };
        }
    }
}