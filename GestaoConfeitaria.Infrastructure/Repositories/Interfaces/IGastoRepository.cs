using GestaoConfeitaria.Domain.Models;

namespace GestaoConfeitaria.Infrastructure.Repositories.Interfaces
{
    public interface IGastoRepository
    {
        Task<List<Gasto>> GetAllActiveAsync();
        Task<Gasto?> GetByIdAsync(int id);
        Task<Gasto?> GetByIdActiveAsync(int id);
        Task AddAsync(Gasto gasto);
        Task UpdateAsync(Gasto gasto);
        Task SoftDeleteAsync(int id, DateTime dataExclusao);
        Task<List<Gasto>> GetByPeriodAsync(DateTime inicio, DateTime fim);
    }
}
