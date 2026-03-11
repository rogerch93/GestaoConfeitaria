using GestaoConfeitariaBiblioteca.Models;
using System.Net.Http.Json;

namespace GestaoConfeitariaWeb.Service
{
    public class VendaServices
    {
        private readonly HttpClient _http;

        public VendaServices(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("Api");
        }

        public async Task<List<Venda>> GetAllAsync()
        {
            var response = await _http.GetAsync("api/vendas");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Venda>>() ?? new();
        }

        public async Task<Venda> GetVendaByIdAsync(int id)
        {
            var response = await _http.GetAsync($"api/vendas/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Venda>() ?? new();
        }

        public async Task<Venda> CreateVendaAsync(Venda venda)
        {
            if(venda == null) throw new ArgumentNullException(nameof(venda));

            var response = await _http.PostAsJsonAsync("api/vendas", venda);

            response.EnsureSuccessStatusCode();

            var vendaCriada = await response.Content.ReadFromJsonAsync<Venda>();
            return vendaCriada ?? throw new Exception("Não foi possível desserializar a venda criada");
        }

        public async Task UpdateVendaAsync(int id, Venda venda)
        {
            if (venda == null)
                throw new ArgumentNullException(nameof(venda));

            if (id <= 0)
                throw new ArgumentException("ID inválido", nameof(id));

            var response = await _http.PutAsJsonAsync($"api/vendas/{id}", venda);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao atualizar venda. Status: {response.StatusCode}. Detalhe: {errorContent}");
            }
        }

        public async Task UpdateVendaSoftDelete(int id, Venda venda)
        {
            if (venda == null)
                throw new ArgumentNullException(nameof(venda));

            if (id <= 0)
                throw new ArgumentException("ID inválido", nameof(id));

            var response = await _http.PutAsJsonAsync($"api/vendas/{id}", venda);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao realizar o Softdelete de venda. Status: {response.StatusCode}. Detalhe: {errorContent}");
            }
        }

    }
}
