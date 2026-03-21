using GestaoConfeitaria.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoConfeitaria.Api.Controllers
{
    [Route("api/indicadores")]
    [ApiController]
    [Authorize]
    public class IndicadoresController : ControllerBase
    {
        private readonly IIndicadorService _indicadorService;

        public IndicadoresController(IIndicadorService indicadorService)
        {
            _indicadorService = indicadorService;
        }

        [HttpGet]
        public async Task<IActionResult> GerarIndicadores([FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim)
        {
            var resultado = await _indicadorService.GerarIndicadoresAsync(dataInicio, dataFim);
            return Ok(new { Indicadores = resultado });
        }
    }
}