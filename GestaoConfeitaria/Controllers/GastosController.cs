using AutoMapper;
using GestaoConfeitaria.Application.DTOs;
using GestaoConfeitaria.Application.Interfaces;
using GestaoConfeitaria.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestaoConfeitaria.Api.Controllers
{
    [Route("api/gastos")]
    [ApiController]
    public class GastosController : ControllerBase
    {
        private readonly IGastoService _gastoService;
        private readonly IMapper _mapper;

        public GastosController(IGastoService gastoService, IMapper mapper)
        {
            _gastoService = gastoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GastoDto>>> GetGastos()
        {
            var gastos = await _gastoService.GetAllAsync();
            return Ok(_mapper.Map<List<GastoDto>>(gastos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GastoDto>> GetById(int id)
        {
            var gasto = await _gastoService.GetByIdAsync(id);
            if (gasto == null)
                return NotFound("Gasto não encontrado.");

            return Ok(_mapper.Map<GastoDto>(gasto));
        }

        [HttpPost]
        public async Task<ActionResult<GastoDto>> Create([FromBody] GastoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var gasto = new Gasto(dto.Descricao, dto.Valor, dto.Usuario);

            await _gastoService.AddAsync(gasto);

            var resultDto = _mapper.Map<GastoDto>(gasto);

            return CreatedAtAction(nameof(GetById), new { id = gasto.Id }, resultDto);
        }

        [HttpPut("{id}/soft-delete")]
        public async Task<IActionResult> SoftDelete(int id, [FromBody] DateTime dataExclusao)
        {
            var sucesso = await _gastoService.SoftDeleteAsync(id, dataExclusao);
            if (!sucesso)
                return NotFound($"Gasto com ID {id} não encontrado.");

            return Ok(new { Message = "Gasto excluído com sucesso." });
        }
    }
}