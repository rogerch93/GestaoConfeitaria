using Blazored.LocalStorage;
using GestaoConfeitariaWeb.Auth;
using GestaoConfeitariaWeb.Components;
using GestaoConfeitariaWeb.Service;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<VendaServices>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<JwtAuthorizationMessageHandler>();
builder.Services.AddHttpClient("Api", client => client.BaseAddress = new Uri("https://localhost:7230/"))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

// HttpClient com token interceptor
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri("https://localhost:7230/") };
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", /* token de localStorage? */);
    // Melhor: crie um DelegatingHandler para injetar token dinamicamente
    return client;
});

//porta da API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7230/") });

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7230/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
