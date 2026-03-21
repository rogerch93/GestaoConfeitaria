using Microsoft.AspNetCore.Mvc;
using GestaoConfeitaria.Application.DTOs;
using GestaoConfeitaria.Application.Interfaces;
using GestaoConfeitaria.Domain.Models;

namespace GestaoConfeitaria.Api.Controllers
{
    [Route("api/vendas")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly IVendaService _vendaService;
        private readonly IMapper _mapper;

        public VendasController(IVendaService vendaService, IMapper mapper)
        {
            _vendaService = vendaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<VendaDto>>> GetVendas()
        {
            var vendas = await _vendaService.GetAllAsync();
            return Ok(_mapper.Map<List<VendaDto>>(vendas));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VendaDto>> GetVendaById(int id)
        {
            var venda = await _vendaService.GetByIdAsync(id);
            if (venda == null)
                return NotFound("Venda não encontrada.");

            return Ok(_mapper.Map<VendaDto>(venda));
        }

        [HttpPost]
        public async Task<ActionResult<VendaDto>> Create([FromBody] VendaCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var venda = new Venda(dto.ValorTotal, dto.QuantidadeBolos, dto.FormaPagamento, dto.Usuario);

            await _vendaService.AddAsync(venda);

            var resultDto = _mapper.Map<VendaDto>(venda);

            return CreatedAtAction(nameof(GetVendaById), new { id = venda.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VendaDto>> Update(int id, [FromBody] VendaCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendaExistente = await _vendaService.GetByIdAsync(id);
            if (vendaExistente == null)
                return NotFound($"Venda com ID {id} não encontrada.");

            // Atualiza a entidade existente (melhor prática)
            var updatedVenda = new Venda(dto.ValorTotal, dto.QuantidadeBolos, dto.FormaPagamento, dto.Usuario);
            // Como não temos método Update ainda, vamos implementar depois no Service

            await _vendaService.UpdateAsync(id, updatedVenda);

            return Ok(_mapper.Map<VendaDto>(updatedVenda));
        }

        [HttpPut("{id}/soft-delete")]
        public async Task<IActionResult> SoftDelete(int id, [FromBody] DateTime dataExclusao)
        {
            var sucesso = await _vendaService.SoftDeleteAsync(id, dataExclusao);
            if (!sucesso)
                return NotFound($"Venda com ID {id} não encontrada.");

            return Ok(new { Message = "Venda excluída com sucesso (soft delete).", DataExclusao = dataExclusao });
        }
    }
}