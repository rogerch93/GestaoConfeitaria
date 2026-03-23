using GestaoConfeitaria.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Application.Interfaces
{
    public interface IMaterialService
    {
        Task<List<Material>> GetAllAsync();
        Task<Material?> GetByIdAsync(int id);
        Task RegistrarUsoAsync(Material material);
        Task<bool> SoftDeleteAsync(int id, DateTime dataExclusao);
    }
}
