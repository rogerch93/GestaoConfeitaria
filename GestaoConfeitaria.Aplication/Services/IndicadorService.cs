using GestaoConfeitaria.Application.Interfaces;
using GestaoConfeitaria.Infrastructure.Repositories.Interfaces;
using GestaoConfeitaria.Infrastructure.UnitOfWork;
using Newtonsoft.Json;

namespace GestaoConfeitaria.Application.Services
{
    public class IndicadorService : IIndicadorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGroqService _groqService;

        public IndicadorService(IUnitOfWork unitOfWork, IGroqService groqService)
        {
            _unitOfWork = unitOfWork;
            _groqService = groqService;
        }

        public async Task<string> GerarIndicadoresAsync(DateTime? dataInicio, DateTime? dataFim)
        {
            var inicio = dataInicio ?? DateTime.UtcNow.AddMonths(-1);
            var fim = dataFim ?? DateTime.UtcNow;

            var vendas = await _unitOfWork.VendaRepository.GetByPeriodAsync(inicio, fim);
            var materiais = await _unitOfWork.MaterialRepository.GetByPeriodAsync(inicio, fim);
            var gastos = await _unitOfWork.GastoRepository.GetByPeriodAsync(inicio, fim);

            var prompt = $@"
Período analisado: {inicio:dd/MM/yyyy} a {fim:dd/MM/yyyy}

Vendas ({vendas.Count} registros):
{JsonConvert.SerializeObject(vendas, Formatting.Indented)}

Materiais usados ({materiais.Count} registros):
{JsonConvert.SerializeObject(materiais, Formatting.Indented)}

Gastos ({gastos.Count} registros):
{JsonConvert.SerializeObject(gastos, Formatting.Indented)}

Gere indicadores claros e profissionais em português, incluindo:
- Receita total
- Custo total de materiais
- Lucro bruto (receita - custo materiais)
- Margem de lucro (%)
- Custo médio por bolo vendido
- Principais gastos
- Sugestões de melhoria ou alertas
Formate com markdown para fácil leitura.";

            return await _groqService.GerarIndicadoresAsync(prompt);
        }
    }
}