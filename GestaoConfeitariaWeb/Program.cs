using Blazored.LocalStorage;
using GestaoConfeitariaWeb.Auth;
using GestaoConfeitariaWeb.Authentication;
using GestaoConfeitariaWeb.Components;
using GestaoConfeitariaWeb.Services;
using GestaoConfeitariaWeb.Services.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = true;
});


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<VendaService>();

// LocalStorage para guardar o token JWT
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();

// Autenticação custom (CustomAuthStateProvider)
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

// Suporte a [Authorize] em componentes
builder.Services.AddAuthorizationCore();

//Handler que injeta o token dinamicamente em TODAS as chamadas do HttpClient nomeado
builder.Services.AddScoped<JwtAuthorizationMessageHandler>();

// HttpClient nomeado e único para a API
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7230/");
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>(); // este handler estava gerando problema rever configuração depois...

// Client específico para Auth (sem handler problemático)
builder.Services.AddHttpClient("AuthApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7230/");
});

// Acesso ao HttpContext no servidor, útil para claims ou cookies
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();