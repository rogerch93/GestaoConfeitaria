using GestaoConfeitaria.Application.Interfaces;
using GestaoConfeitaria.Domain.Models;
using GestaoConfeitaria.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Application.Services
{
    public class GastoService : IGastoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GastoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Gasto>> GetAllAsync()
        {
            return await _unitOfWork.GastoRepository.GetAllActiveAsync();
        }

        public async Task<Gasto?> GetByIdAsync(int id)
        {
            return await _unitOfWork.GastoRepository.GetByIdActiveAsync(id);
        }

        public async Task AddAsync(Gasto gasto)
        {
            await _unitOfWork.GastoRepository.AddAsync(gasto);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(int id, DateTime dataExclusao)
        {
            var gasto = await _unitOfWork.GastoRepository.GetByIdAsync(id);
            if (gasto == null) return false;

            gasto.GetType().GetProperty("DataExclusao")?.SetValue(gasto, dataExclusao);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
