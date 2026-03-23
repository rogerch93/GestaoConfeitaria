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
    public class VendaService : IVendaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VendaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Venda>> GetAllAsync()
        {
            return await _unitOfWork.VendaRepository.GetAllActiveAsync();
        }

        public async Task<Venda?> GetByIdAsync(int id)
        {
            return await _unitOfWork.VendaRepository.GetByIdActiveAsync(id);
        }

        public async Task AddAsync(Venda venda)
        {
            await _unitOfWork.VendaRepository.AddAsync(venda);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Venda vendaAtualizada)
        {
            var vendaExistente = await _unitOfWork.VendaRepository.GetByIdActiveAsync(id);
            if (vendaExistente == null)
                throw new KeyNotFoundException($"Venda com ID {id} não encontrada.");

            // Atualiza propriedades (não substitui a entidade)
            vendaExistente.GetType().GetProperty("ValorTotal")?.SetValue(vendaExistente, vendaAtualizada.ValorTotal);
            vendaExistente.GetType().GetProperty("QuantidadeBolos")?.SetValue(vendaExistente, vendaAtualizada.QuantidadeBolos);
            vendaExistente.GetType().GetProperty("FormaPagamento")?.SetValue(vendaExistente, vendaAtualizada.FormaPagamento);
            vendaExistente.GetType().GetProperty("Usuario")?.SetValue(vendaExistente, vendaAtualizada.Usuario);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(int id, DateTime dataExclusao)
        {
            var venda = await _unitOfWork.VendaRepository.GetByIdAsync(id);
            if (venda == null) return false;

            venda.GetType().GetProperty("DataExclusao")?.SetValue(venda, dataExclusao);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
