using GestaoConfeitaria.Auth;
using GestaoConfeitaria.Data;
using GestaoConfeitaria.Models;
using GestaoConfeitaria.Request.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestaoConfeitaria.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BoloDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(BoloDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPostAttribute("login")]
        public async Task<IActionResult> Login(Auth.LoginRequest request)
        {
            // Usuário fixo para demonstração
            if (request.Username == "admin" && request.Password == "12345")
            {
                var sectionFixed = _config.GetSection("JwtSettings");
                if (!sectionFixed.Exists())
                    return Problem("Seção JwtSettings não encontrada no configuration");

                var jwtSettingsFixed = sectionFixed.Get<JwtSettings>();
                if (jwtSettingsFixed == null || string.IsNullOrEmpty(jwtSettingsFixed.SecretKey))
                    return Problem("JwtSettings carregado, mas SecretKey vazia ou null");

                var claimsFixed = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var keyFixed = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettingsFixed.SecretKey));
                var credsFixed = new SigningCredentials(keyFixed, SecurityAlgorithms.HmacSha256);

                var tokenFixed = new JwtSecurityToken(
                    issuer: jwtSettingsFixed.Issuer,
                    audience: jwtSettingsFixed.Audience,
                    claims: claimsFixed,
                    expires: DateTime.UtcNow.AddMinutes(jwtSettingsFixed.ExpiresInMinutes),
                    signingCredentials: credsFixed);

                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(tokenFixed) });
            }

            // Usuário do banco
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Credenciais Inválidas");

            var section = _config.GetSection("JwtSettings");
            if (!section.Exists())
                return Problem("Seção JwtSettings não encontrada no configuration");

            var jwtSettings = section.Get<JwtSettings>();
            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
                return Problem("JwtSettings carregado, mas SecretKey vazia ou null");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpiresInMinutes),
                signingCredentials: creds);

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Request.Auth.RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username))
                return BadRequest("Username é obrigatório");

            if (string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Password é Obrigatório");

            if (req.Password.Length < 8)
                return BadRequest("A senha deve ter pelo menos 8 caracteres");

            var existente = await _db.Users.AnyAsync(u => u.Username == req.Username);
            if (existente)
                return Conflict("Username já esta sendo usado por outro usuário");

            var hash = BCrypt.Net.BCrypt.HashPassword(req.Password);

            var user = new User
            {
                Username = req.Username,
                PasswordHash = hash,
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Created($"/api/auth/users/{user.Id}", new
            {
                user.Id,
                user.Username,
                user.Role
            });
        }
    }
}
