using GestaoConfeitaria.Data;
using GestaoConfeitaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GestaoConfeitaria.Controllers
{
    [Route("api/gastos")]
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
        public async Task<ActionResult<IEnumerable<Gasto>>> GetMateriais()
        {
            return await _context.Gastos.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Gasto>> PosVenda(Gasto gasto)
        {
            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMateriais), new { id = gasto.Id }, gasto);
        }
    }
}
