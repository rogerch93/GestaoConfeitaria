using GestaoConfeitaria.Data;
using GestaoConfeitaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoConfeitaria.Controllers
{
    [Route("api/vendas")]
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
            return await _context.Vendas.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Venda>> PosVenda(Venda venda)
        {
            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVendas), new {id = venda.Id}, venda);
        }

    }
}
