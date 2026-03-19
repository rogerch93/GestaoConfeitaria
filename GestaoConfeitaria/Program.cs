using GestaoConfeitaria.Auth;
using GestaoConfeitaria.Data;
using GestaoConfeitaria.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<BoloDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MeuProjetoIA",
        Version = "v1"
    });

    //Configuração para mostrar o cadeado baerer Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer.\r\n\r\n" +
                      "Digite 'Bearer' [espaço] seguido do seu token gerado no /api/auth/login.\r\n\r\n" +
                      "Exemplo: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
           new OpenApiSecurityScheme
           {
               Reference = new OpenApiReference
               {
                   Type = ReferenceType.SecurityScheme,
                   Id = "Bearer"
               }
           },
           new string[] {}
        }
    });

});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
                    ?? throw new InvalidOperationException("JwtSettings não está configurado corretamente.");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken) && context.Request.Path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
}).AddCookie(options =>
{
    options.Cookie.Name = ".GestaoConfeitaria.Auth";          // nome do cookie (pode mudar)
    options.Cookie.HttpOnly = true;                           // protege contra JS
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // só HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict;            // anti-CSRF
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);        // tempo de expiração
    options.SlidingExpiration = true;                         // renova se usar
    options.LoginPath = "/login";                             // opcional
    options.AccessDeniedPath = "/access-denied";              // opcional

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Cookie.Name = ".GestaoConfeitaria.Auth";          // nome do cookie (pode mudar)
//        options.Cookie.HttpOnly = true;                           // protege contra JS
//        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // só HTTPS
//        options.Cookie.SameSite = SameSiteMode.Strict;            // anti-CSRF
//        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);        // tempo de expiração
//        options.SlidingExpiration = true;                         // renova se usar
//        options.LoginPath = "/login";                             // opcional
//        options.AccessDeniedPath = "/access-denied";              // opcional
//    });

builder.Services.AddAuthorization();

//Rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("limiteRequisicao", opt =>
    {
        opt.PermitLimit = 10; // limite de 10 requisições
        opt.Window = TimeSpan.FromMinutes(1); //por minuto
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 5; //fila de espera
    });

    options.RejectionStatusCode = 429;
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.Headers.Append("Retry-After", "60");
        await context.HttpContext.Response.WriteAsync("Muitas requisições. Tente novamente em 1 minuto.");
    };
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<GroqService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorDev", policy =>
    {
        policy.WithOrigins("https://localhost:7029/")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("BlazorDev");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

