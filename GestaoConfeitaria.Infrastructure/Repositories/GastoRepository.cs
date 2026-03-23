using GestaoConfeitaria.Domain.Models;
using GestaoConfeitaria.Infrastructure.Data;
using GestaoConfeitaria.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestaoConfeitaria.Infrastructure.Repositories
{
    public class GastoRepository : IGastoRepository
    {
        private readonly BoloDbContext _context;

        public GastoRepository(BoloDbContext context)
        {
            _context = context;
        }

        public async Task<List<Gasto>> GetAllActiveAsync()
        {
            return await _context.Gastos
                .Where(g => g.DataExclusao == null)
                .OrderByDescending(g => g.Data)
                .ToListAsync();
        }

        public async Task<Gasto?> GetByIdAsync(int id)
        {
            return await _context.Gastos.FindAsync(id);
        }

        public async Task<Gasto?> GetByIdActiveAsync(int id)
        {
            return await _context.Gastos
                .FirstOrDefaultAsync(g => g.Id == id && g.DataExclusao == null);
        }

        public async Task AddAsync(Gasto gasto)
        {
            await _context.Gastos.AddAsync(gasto);
        }

        public async Task UpdateAsync(Gasto gasto)
        {
            _context.Gastos.Update(gasto);
        }

        public async Task SoftDeleteAsync(int id, DateTime dataExclusao)
        {
            var gasto = await GetByIdAsync(id);
            if (gasto != null)
            {
                gasto.GetType().GetProperty("DataExclusao")?.SetValue(gasto, dataExclusao);
                _context.Gastos.Update(gasto);
            }
        }

        public async Task<List<Gasto>> GetByPeriodAsync(DateTime inicio, DateTime fim)
        {
            return await _context.Gastos
                .Where(g => g.Data >= inicio && g.Data <= fim && g.DataExclusao == null)
                .ToListAsync();
        }
    }
}
