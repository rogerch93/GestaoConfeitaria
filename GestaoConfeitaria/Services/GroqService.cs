using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace GestaoConfeitaria.Services;

public class GroqService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GroqService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiKey = configuration["Groq:ApiKey"]
        ?? throw new InvalidOperationException("Groq:ApiKey não configurado em appsettings.");

        _httpClient.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> GerarIndicadoresAsync(string prompt)
    {
        var requestBody = new
        {
            model = "mixtral-8x7b-32768",  // ou "llama3-70b-8192" ou outro modelo disponível
            messages = new[]
            {
                new { role = "system", content = "Você é um analista financeiro especializado em confeitaria e gestão de bolos. Seja objetivo, use números reais dos dados fornecidos e gere indicadores claros como: lucro bruto, margem de lucro, custo médio por bolo, comparação com período anterior, sugestões de melhoria e alerta de prejuízo." },
                new { role = "user", content = prompt }
            },
            temperature = 0.7,
            max_tokens = 1500
        };

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsJsonAsync("https://api.groq.com/openai/v1/chat/completions", requestBody);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro na API Groq: {response.StatusCode} - {errorContent}");
        }

        var jsonResponse = await response.Content.ReadFromJsonAsync<GroqCompletionResponse>();

        return jsonResponse?.Choices?.FirstOrDefault()?.Message?.Content
            ?? "Erro ao processar resposta da IA";
    }

    // Classes de resposta EXATAS para o formato Groq/OpenAI
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