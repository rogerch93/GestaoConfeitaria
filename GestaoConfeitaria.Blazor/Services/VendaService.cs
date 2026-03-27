using GestaoConfeitaria.Domain.Models;
using GestaoConfeitaria.Shared.DTOs;
using System.Net.Http.Json;

namespace GestaoConfeitariaWeb.Services
{
    public interface IVendaService
    {
        Task<List<VendaDto>> GetAllAsync();
        Task<VendaDto> GetByIdAsync(int id);
        Task<VendaDto> CreateAsync(VendaCreateDto dto);
        Task<VendaDto> UpdateAsync(int id, VendaCreateDto dto);
        Task<bool> SoftDeleteAsync(int id, DateTime dataExclusao);
    }

    public class VendaService : IVendaService
    {
        private readonly HttpClient _httpClient;

        public VendaService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("GestaoApi");
        }

        public async Task<List<VendaDto>> GetAllAsync()
            => await _httpClient.GetFromJsonAsync<List<VendaDto>>("api/vendas") ?? new();

        public async Task<VendaDto?> GetByIdAsync(int id)
            => await _httpClient.GetFromJsonAsync<VendaDto>($"api/vendas/{id}");

        public async Task<VendaDto> CreateAsync(VendaCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/vendas", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<VendaDto>() ?? throw new Exception("Erro ao criar venda");
        }

        public async Task<VendaDto> UpdateAsync(int id, VendaCreateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/vendas/{id}", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<VendaDto>() ?? throw new Exception("Erro ao atualizar venda");
        }

        public async Task<bool> SoftDeleteAsync(int id, DateTime dataExclusao)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/vendas/{id}/soft-delete", dataExclusao);
            return response.IsSuccessStatusCode;
        }
    }
}
