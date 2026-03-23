using GestaoConfeitaria.Domain.Models;
using GestaoConfeitaria.Infrastructure.Data;
using GestaoConfeitaria.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestaoConfeitaria.Infrastructure.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly BoloDbContext _context;

        public VendaRepository(BoloDbContext context)
        {
            _context = context;
        }

        public async Task<List<Venda>> GetAllActiveAsync()
        {
            return await _context.Vendas
                .Where(v => v.DataExclusao == null)
                .OrderByDescending(v => v.Data)
                .ToListAsync();
        }

        public async Task<Venda?> GetByIdAsync(int id)
        {
            return await _context.Vendas.FindAsync(id);
        }

        public async Task<Venda?> GetByIdActiveAsync(int id)
        {
            return await _context.Vendas
                .FirstOrDefaultAsync(v => v.Id == id && v.DataExclusao == null);
        }

        public async Task AddAsync(Venda venda)
        {
            await _context.Vendas.AddAsync(venda);
        }

        public async Task UpdateAsync(Venda venda)
        {
            _context.Vendas.Update(venda);
        }

        public async Task SoftDeleteAsync(int id, DateTime dataExclusao)
        {
            var venda = await GetByIdAsync(id);
            if (venda != null)
            {
                venda.GetType().GetProperty("DataExclusao")?.SetValue(venda, dataExclusao);
                _context.Vendas.Update(venda);
            }
        }

        public async Task<List<Venda>> GetByPeriodAsync(DateTime inicio, DateTime fim)
        {
            return await _context.Vendas
                .Where(v => v.Data >= inicio && v.Data <= fim && v.DataExclusao == null)
                .ToListAsync();
        }
    }
}
