using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Application.DTOs
{
  public class RegisterRequestDto
  {
     [Required(ErrorMessage = "O Username é obrigatório")]
     [MinLength(3, ErrorMessage = "O Username deve ter no mínimo 3 caracteres")]
     [MaxLength(50, ErrorMessage = "O Username deve ter no máximo 50 caracteres")]
     public string Username { get; set; } = string.Empty;

     [Required(ErrorMessage = "A senha é obrigatória")]
     [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
     [MaxLength(100, ErrorMessage = "A senha deve ter no máximo 100 caracteres")]
     public string Password { get; set; } = string.Empty;
  }
}
