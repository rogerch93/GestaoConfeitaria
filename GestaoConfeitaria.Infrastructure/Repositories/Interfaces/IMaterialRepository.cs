using GestaoConfeitaria.Domain.Models;

namespace GestaoConfeitaria.Infrastructure.Repositories.Interfaces
{
    public interface IMaterialRepository
    {
        Task<List<Material>> GetAllActiveAsync();
        Task<Material?> GetByIdAsync(int id);
        Task<Material?> GetByIdActiveAsync(int id);
        Task AddAsync(Material material);
        Task UpdateAsync(Material material);
        Task SoftDeleteAsync(int id, DateTime dataExclusao);
        Task<List<Material>> GetByPeriodAsync(DateTime inicio, DateTime fim);
    }
}
