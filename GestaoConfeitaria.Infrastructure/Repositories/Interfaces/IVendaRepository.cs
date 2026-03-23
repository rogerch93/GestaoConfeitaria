using GestaoConfeitaria.Domain.Models;

namespace GestaoConfeitaria.Infrastructure.Repositories.Interfaces
{
    public interface IVendaRepository
    {
        Task<List<Venda>> GetAllActiveAsync();
        Task<Venda?> GetByIdAsync(int id);
        Task<Venda?> GetByIdActiveAsync(int id);
        Task AddAsync(Venda venda);
        Task UpdateAsync(Venda venda);
        Task SoftDeleteAsync(int id, DateTime dataExclusao);
        Task<List<Venda>> GetByPeriodAsync(DateTime inicio, DateTime fim);
    }
}
