using GestaoConfeitaria.Data;
using GestaoConfeitaria.Models;
using GestaoConfeitariaBiblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Venda = GestaoConfeitaria.Models.Venda;

namespace GestaoConfeitaria.Controllers
{
    [Route("api/vendas")]
    [EnableRateLimiting("limiteRequisicao")]
    [ApiController]
    [Authorize]
    public class VendasController : ControllerBase
    {
        private readonly BoloDbContext _context;

        public VendasController(BoloDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult <IEnumerable<Venda>>> GetVendas()
        {
            var vendas = await _context.Vendas
                .Where(vendas => vendas.DataExclusao == null)
                .OrderByDescending(vendas => vendas.Data)
                .ToListAsync();
            return Ok(vendas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetVendaById(int id)
        {
            Venda? venda = _context.Vendas.Where(vendas => vendas.DataExclusao == null).FirstOrDefault(venda => venda.Id == id);

            if (venda == null)
            {
                return NotFound("Venda não encontrado.");
            }

            return Ok(venda);
        }

        [HttpPost]
        public async Task<ActionResult<Venda>> Venda(Venda venda)
        {
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVendas), new {id = venda.Id}, venda);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Venda>> UpDateVenda(int id, [FromBody] Venda vendaAtualizado)
        {
            if (vendaAtualizado == null)
            {
                return BadRequest("Corpo da requisição não pode ser nulo.");
            }

            if (id != vendaAtualizado.Id)
            {
                return BadRequest("O ID na URL deve ser igual ao ID do corpo da requisição.");
            }

            var vendaExistente = await _context.Vendas.Where(venda => venda.DataExclusao == null).FirstOrDefaultAsync(venda => venda.Id == id);

            if (vendaExistente == null)
            {
                return NotFound($"Venda com ID {id} não encontrado.");
            }

            // Atualiza apenas os listados abaixo (evita sobrescrever VendaId)
            vendaExistente.FormaPagamento = vendaAtualizado.FormaPagamento;
            vendaExistente.QuantidadeBolos = vendaAtualizado.QuantidadeBolos;
            vendaExistente.Usuario = vendaAtualizado.Usuario;
            vendaExistente.ValorTotal = vendaAtualizado.ValorTotal;
            vendaExistente.Data = vendaAtualizado.Data;

            _context.Vendas.Update(vendaExistente);
            await _context.SaveChangesAsync();

            return Ok(vendaExistente);
        }

        [HttpPut("{id}/soft-delete")]
        public async Task<ActionResult<Venda>> SoftDeleteGasto(int id, [FromBody] DateTime? dataExclusao)
        {
            if (!dataExclusao.HasValue)
            {
                return BadRequest("É obrigatório informar a DataExclusao no corpo da requisição.");
            }

            var vendaExistente = await _context.Gastos
                .FirstOrDefaultAsync(venda => venda.Id == id);

            if (vendaExistente == null)
            {
                return NotFound($"Venda com ID {id} não encontrado.");
            }

            // Atualiza apenas os listados abaixo (evita sobrescrever VendaId)
            vendaExistente.DataExclusao = dataExclusao.Value;

            _context.Gastos.Update(vendaExistente);
            await _context.SaveChangesAsync();

            return Ok("Data de exclusão: " + new { DataExclusao = vendaExistente.DataExclusao });
        }
    }

}
