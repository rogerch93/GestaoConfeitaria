using GestaoConfeitaria.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Application.Interfaces
{
    public interface IGastoService
    {
        Task<List<Gasto>> GetAllAsync();
        Task<Gasto?> GetByIdAsync(int id);
        Task AddAsync(Gasto gasto);
        Task<bool> SoftDeleteAsync(int id, DateTime dataExclusao);
    }
}
