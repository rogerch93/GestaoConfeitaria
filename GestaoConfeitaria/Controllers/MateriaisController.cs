using GestaoConfeitaria.Data;
using GestaoConfeitaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace GestaoConfeitaria.Controllers
{
    [Route("api/materiais")]
    [EnableRateLimiting("limiteRequisicao")]
    [ApiController]
    [Authorize]
    public class MateriaisController : ControllerBase
    {
        private readonly BoloDbContext _context;

        public MateriaisController(BoloDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetMateriais()
        {
            var material = await _context.MateriaisUsados
               .Where(material => material.DataExclusao == null)
               .OrderByDescending(material => material.DataUso)
               .ToListAsync();
            return Ok(material);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMaterialById(int id)
        {
            Material? material = _context.MateriaisUsados.Where(material => material.DataExclusao == null).FirstOrDefault(material => material.Id == id);

            if(material == null)
            {
                return NotFound("Material não encontrado.");
            }

            return Ok(material);
        }
        [HttpPost]
        public async Task<ActionResult<Material>> PosVenda(Material material)
        {
            _context.MateriaisUsados.Add(material);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMateriais), new { id = material.Id }, material);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Material>> UpDateMaterial(int id, [FromBody] Material materialAtualizado)
        {
           if(materialAtualizado == null)
           {
               return BadRequest("Corpo da requisição não pode ser nulo.");
           }

           if(id != materialAtualizado.Id)
            {
                return BadRequest("O ID na URL deve ser igual ao ID do corpo da requisição.");
            }

           var materialExistente = await _context.MateriaisUsados.Where(material => material.DataExclusao == null).FirstOrDefaultAsync(material => material.Id == id);

            if(materialExistente == null)
            {
                return NotFound($"Material com ID {id} não encontrado.");
            }

            // Atualiza apenas os listados abaixo (evita sobrescrever VendaId)
            materialExistente.Nome = materialAtualizado.Nome;
            materialExistente.Quantidade = materialAtualizado.Quantidade;
            materialExistente.CustoUnitario = materialAtualizado.CustoUnitario;
            materialExistente.DataUso = materialAtualizado.DataUso;

            _context.MateriaisUsados.Update(materialExistente);
            await _context.SaveChangesAsync();

            return Ok(materialExistente);
        }
    }
}
