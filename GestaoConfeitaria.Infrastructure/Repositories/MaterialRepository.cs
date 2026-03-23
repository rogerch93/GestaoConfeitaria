using GestaoConfeitaria.Domain.Models;
using GestaoConfeitaria.Infrastructure.Data;
using GestaoConfeitaria.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestaoConfeitaria.Infrastructure.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly BoloDbContext _context;

        public MaterialRepository(BoloDbContext context)
        {
            _context = context;
        }

        public async Task<List<Material>> GetAllActiveAsync()
        {
            return await _context.MateriaisUsados
                .Where(m => m.DataExclusao == null)
                .OrderByDescending(m => m.DataUso)
                .ToListAsync();
        }

        public async Task<Material?> GetByIdAsync(int id)
        {
            return await _context.MateriaisUsados.FindAsync(id);
        }

        public async Task<Material?> GetByIdActiveAsync(int id)
        {
            return await _context.MateriaisUsados
                .FirstOrDefaultAsync(m => m.Id == id && m.DataExclusao == null);
        }

        public async Task AddAsync(Material material)
        {
            await _context.MateriaisUsados.AddAsync(material);
        }

        public async Task UpdateAsync(Material material)
        {
            _context.MateriaisUsados.Update(material);
        }

        public async Task SoftDeleteAsync(int id, DateTime dataExclusao)
        {
            var material = await GetByIdAsync(id);
            if (material != null)
            {
                material.GetType().GetProperty("DataExclusao")?.SetValue(material, dataExclusao);
                _context.MateriaisUsados.Update(material);
            }
        }

        public async Task<List<Material>> GetByPeriodAsync(DateTime inicio, DateTime fim)
        {
            return await _context.MateriaisUsados
                .Where(m => m.DataUso >= inicio && m.DataUso <= fim && m.DataExclusao == null)
                .ToListAsync();
        }
    }
}
