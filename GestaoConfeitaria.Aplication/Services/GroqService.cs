// GestaoConfeitaria.Application/Services/GroqService.cs
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace GestaoConfeitaria.Application.Services
{
    public interface IGroqService
    {
        Task<string> GerarIndicadoresAsync(string prompt);
    }

    public class GroqService : IGroqService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;

        public GroqService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("Groq");

            var groqSection = configuration.GetSection("Groq");

            _apiKey = groqSection["ApiKey"]
                ?? throw new InvalidOperationException("Groq:ApiKey não configurado em appsettings.json.");

            _model = groqSection["Model"] ?? "llama-3.3-70b-versatile";

            _httpClient.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> GerarIndicadoresAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                throw new ArgumentException("Prompt não pode ser vazio.", nameof(prompt));

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "Você é um analista financeiro especializado em confeitaria e gestão de bolos. Seja objetivo, use números reais dos dados fornecidos e gere indicadores claros como: lucro bruto, margem de lucro, custo médio por bolo, comparação com período anterior, sugestões de melhoria e alerta de prejuízo." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.7,
                max_tokens = 1500
            };

            var response = await _httpClient.PostAsJsonAsync("chat/completions", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro na API Groq: {response.StatusCode} - {errorContent}");
            }

            var jsonResponse = await response.Content.ReadFromJsonAsync<GroqCompletionResponse>();

            return jsonResponse?.Choices?.FirstOrDefault()?.Message?.Content
                ?? "Erro ao processar resposta da IA";
        }

        // Classes internas de resposta
        private class GroqCompletionResponse
        {
            public List<GroqChoice>? Choices { get; set; }
        }

        private class GroqChoice
        {
            public GroqMessage? Message { get; set; }
        }

        private class GroqMessage
        {
            public string? Content { get; set; }
        }
    }
}