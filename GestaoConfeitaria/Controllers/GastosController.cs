using GestaoConfeitaria.Data;
using GestaoConfeitaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace GestaoConfeitaria.Controllers
{
    [Route("api/gastos")]
    [EnableRateLimiting("limiteRequisicao")]
    [ApiController]
    [Authorize]
    public class GastosController : ControllerBase
    {
        private readonly BoloDbContext _context;

        public GastosController(BoloDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gasto>>> GetGasto()
        {
            var gastos = await _context.Gastos
                .Where(gastos => gastos.DataExclusao == null)
                .OrderByDescending(gastos => gastos.Data)
                .ToListAsync();
            return Ok(gastos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetGastosById(int id)
        {
            Gasto? gasto = _context.Gastos.Where(gastos => gastos.DataExclusao == null).FirstOrDefault(gasto => gasto.Id == id);

            if (gasto == null)
            {
                return NotFound("Gasto não encontrado.");
            }

            return Ok(gasto);
        }

        [HttpPost]
        public async Task<ActionResult<Gasto>> Gasto(Gasto gasto)
        {
            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGasto), new { id = gasto.Id }, gasto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Gasto>> UpDateGasto(int id, [FromBody] Gasto gastoAtualizado)
        {
            if (gastoAtualizado == null)
            {
                return BadRequest("Corpo da requisição não pode ser nulo.");
            }

            if (id != gastoAtualizado.Id)
            {
                return BadRequest("O ID na URL deve ser igual ao ID do corpo da requisição.");
            }

            var gastoExistente = await _context.Gastos.Where(gastos => gastos.DataExclusao == null).FirstOrDefaultAsync(gasto => gasto.Id == id);

            if (gastoExistente == null)
            {
                return NotFound($"Material com ID {id} não encontrado.");
            }

            // Atualiza apenas os listados abaixo (evita sobrescrever VendaId)
            gastoExistente.Descricao = gastoAtualizado.Descricao;
            gastoExistente.Usuario = gastoAtualizado.Usuario;
            gastoExistente.Data = gastoAtualizado.Data;
            gastoExistente.Valor = gastoAtualizado.Valor;

            _context.Gastos.Update(gastoExistente);
            await _context.SaveChangesAsync();

            return Ok(gastoExistente);
        }
    }
}
