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
            return await _context.MateriaisUsados.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Material>> PosVenda(Material material)
        {
            _context.MateriaisUsados.Add(material);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMateriais), new { id = material.Id }, material);
        }
    }
}
