using BCrypt.Net;
using GestaoConfeitaria.Application.DTOs;
using GestaoConfeitaria.Application.Interfaces;
using GestaoConfeitaria.Domain.Models;
using GestaoConfeitaria.Infrastructure.Repositories.Interfaces;
using GestaoConfeitaria.Infrastructure.UnitOfWork;

namespace GestaoConfeitaria.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResult> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Credenciais inválidas"
                };
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return new LoginResult
            {
                Success = true,
                Message = "Login realizado com sucesso",
                User = userDto
            };
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequestDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Username))
                return new RegisterResult { Success = false, Message = "Username é obrigatório" };

            if (string.IsNullOrWhiteSpace(registerDto.Password))
                return new RegisterResult { Success = false, Message = "Password é obrigatório" };

            if (registerDto.Password.Length < 8)
                return new RegisterResult { Success = false, Message = "A senha deve ter pelo menos 8 caracteres" };

            var existente = await _unitOfWork.UserRepository.ExistsByUsernameAsync(registerDto.Username);
            if (existente)
                return new RegisterResult { Success = false, Message = "Username já está sendo usado por outro usuário" };

            var hash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new User(registerDto.Username, hash, "User");

            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();

            return new RegisterResult
            {
                Success = true,
                Message = "Usuário criado com sucesso",
                UserId = newUser.Id
            };
        }
    }
}