using GestaoConfeitaria.Data;
using GestaoConfeitaria.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims; // ou System.Text.Json

namespace GestaoConfeitaria.Controllers
{
    [Route("api/indicadores")]
    [EnableRateLimiting("limiteRequisicao")]
    [ApiController]
    [Authorize]
    public class IndicadoresController : ControllerBase
    {
        private readonly BoloDbContext _db;
        private readonly GroqService _groq;

        public IndicadoresController(BoloDbContext db, GroqService groq)
        {
            _db = db;
            _groq = groq;
        }

        [HttpGet]
        public async Task<IActionResult> GerarIndicadores([FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim)
        {
            var inicio = dataInicio ?? DateTime.UtcNow.AddMonths(-1);
            var fim = dataFim ?? DateTime.UtcNow;

            var usuarioLogado = User.FindFirstValue(ClaimTypes.Role);
            if (usuarioLogado != "Admin")
            {
                return StatusCode(403, "Somente Usuários com a permissão de Admin podem criar novos Usuários.");
            }

            var vendas = await _db.Vendas
                .Where(v => v.Data >= inicio && v.Data <= fim)
                .ToListAsync();

            var materiais = await _db.MateriaisUsados
                .Where(m => m.DataUso >= inicio && m.DataUso <= fim)
                .ToListAsync();

            var gastos = await _db.Gastos
                .Where(g => g.Data >= inicio && g.Data <= fim)
                .ToListAsync();

            //Montagem do prompt para IA 

            var prompt = $@"
Período analisado:{inicio:dd/MM/yyyy} a {fim:dd/MM/yyyy}

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
- Comparação com período anterior (se possível)
- Sugestões de melhoria ou alertas (ex: prejuízo, custo alto)
- Formate com markdown para fácil leitura
";

            try
            {
                var respostaIA = await _groq.GerarIndicadoresAsync(prompt);
                return Ok(new { Indicadores = respostaIA });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao gerar indicadores: {ex.Message}");
            }

        }
    }
}
