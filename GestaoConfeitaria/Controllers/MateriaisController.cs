using Microsoft.AspNetCore.Mvc;
using GestaoConfeitaria.Application.DTOs;
using GestaoConfeitaria.Application.Interfaces;
using GestaoConfeitaria.Domain.Models;
using AutoMapper;

namespace GestaoConfeitaria.Api.Controllers
{
    [Route("api/materiais")]
    [ApiController]
    public class MateriaisController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly IMapper _mapper;

        public MateriaisController(IMaterialService materialService, IMapper mapper)
        {
            _materialService = materialService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MaterialDto>>> GetMateriais()
        {
            var materiais = await _materialService.GetAllAsync();
            return Ok(_mapper.Map<List<MaterialDto>>(materiais));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialDto>> GetById(int id)
        {
            var material = await _materialService.GetByIdAsync(id);
            if (material == null)
                return NotFound("Material não encontrado.");

            return Ok(_mapper.Map<MaterialDto>(material));
        }

        [HttpPost("pos-venda")]
        public async Task<ActionResult<MaterialDto>> PosVenda([FromBody] MaterialCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var material = new Material(dto.VendaId, dto.Nome, dto.Quantidade, dto.CustoUnitario);

            if (dto.DataUso.HasValue)
                material.GetType().GetProperty("DataUso")?.SetValue(material, dto.DataUso.Value);

            await _materialService.RegistrarUsoAsync(material);

            var resultDto = _mapper.Map<MaterialDto>(material);

            return CreatedAtAction(nameof(GetById), new { id = material.Id }, resultDto);
        }

        [HttpPut("{id}/soft-delete")]
        public async Task<IActionResult> SoftDelete(int id, [FromBody] DateTime dataExclusao)
        {
            var sucesso = await _materialService.SoftDeleteAsync(id, dataExclusao);
            if (!sucesso)
                return NotFound($"Material com ID {id} não encontrado.");

            return Ok(new { Message = "Material excluído com sucesso." });
        }
    }
}