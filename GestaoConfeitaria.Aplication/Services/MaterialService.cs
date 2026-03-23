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
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Material>> GetAllAsync()
        {
            return await _unitOfWork.MaterialRepository.GetAllActiveAsync();
        }

        public async Task<Material?> GetByIdAsync(int id)
        {
            return await _unitOfWork.MaterialRepository.GetByIdActiveAsync(id);
        }

        public async Task RegistrarUsoAsync(Material material)
        {
            await _unitOfWork.MaterialRepository.AddAsync(material);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(int id, DateTime dataExclusao)
        {
            var material = await _unitOfWork.MaterialRepository.GetByIdAsync(id);
            if (material == null) return false;

            material.GetType().GetProperty("DataExclusao")?.SetValue(material, dataExclusao);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
