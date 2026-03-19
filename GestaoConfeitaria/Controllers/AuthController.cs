using GestaoConfeitaria.Auth;
using GestaoConfeitaria.Data;
using GestaoConfeitaria.Features.Auth;
using GestaoConfeitaria.Models;
using GestaoConfeitaria.Request.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var usuario = await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if(usuario == null || !PasswordHasher.VerifyPassword(user.PasswordHash, usuario.PasswordHash))
            {
                return Unauthorized("Credenciais inválidas");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
                new Claim("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return NoContent();

        }

        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> Register([FromBody] Request.Auth.RegisterRequest req)
        {
            var usuarioLogado = User.FindFirstValue(ClaimTypes.Role);
            if (usuarioLogado != "Admin")
            {
                return StatusCode(403, "Somente Usuários com a permissão de Admin podem criar novos Usuários.");
            }
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
