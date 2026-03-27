using System.Net.Http.Headers;
using Blazored.LocalStorage;   // ou ProtectedLocalStorage se estiver usando

namespace GestaoConfeitariaWeb.Auth
{
    public class JwtAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;   // ou ProtectedLocalStorage

        public JwtAuthorizationMessageHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Evita chamar LocalStorage durante pré-render ou render inicial
            if (OperatingSystem.IsBrowser())   // só executa no client real
            {
                try
                {
                    var token = await _localStorage.GetItemAsync<string>("authToken");
                    if (!string.IsNullOrEmpty(token))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                }
                catch
                {
                    // Silencia erro de JS Interop ou localStorage não disponível ainda
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}