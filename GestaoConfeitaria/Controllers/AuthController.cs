using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GestaoConfeitaria.Application.DTOs;
using GestaoConfeitaria.Application.Interfaces;

namespace GestaoConfeitaria.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Realiza login e cria o cookie de autenticação
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            // Cria o cookie de autenticação
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.User!.Username),
                new Claim(ClaimTypes.Role, result.User.Role),
                new Claim("UserId", result.User.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,           // Mantém o login após fechar o navegador
                    ExpiresUtc = DateTime.UtcNow.AddHours(8)
                });

            return Ok(new
            {
                message = "Login realizado com sucesso",
                user = new
                {
                    result.User.Id,
                    result.User.Username,
                    result.User.Role
                }
            });
        }

        /// <summary>
        /// Registra um novo usuário (apenas Admin)
        /// </summary>
        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Regra de negócio: Apenas Admin pode registrar novos usuários
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole != "Admin")
            {
                return StatusCode(403, new
                {
                    message = "Acesso negado. Apenas usuários com permissão de Administrador podem criar novos usuários."
                });
            }

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Created($"/api/auth/users/{result.UserId}", new
            {
                message = "Usuário criado com sucesso",
                userId = result.UserId,
                username = registerDto.Username,
                role = "User"
            });
        }

        /// <summary>
        /// Realiza logout (remove o cookie)
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logout realizado com sucesso" });
        }
    }
}