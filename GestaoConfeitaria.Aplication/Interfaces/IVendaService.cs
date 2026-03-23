using GestaoConfeitaria.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Application.Interfaces
{
    public interface IVendaService
    {
        Task<List<Venda>> GetAllAsync();
        Task<Venda?> GetByIdAsync(int id);
        Task AddAsync(Venda venda);
        Task UpdateAsync(int id, Venda venda);
        Task<bool> SoftDeleteAsync(int id, DateTime dataExclusao);
    }
}
