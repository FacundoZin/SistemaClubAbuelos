using APIClub.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClub.Contrrollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CobranzasController : ControllerBase
    {
        private readonly ICobranzasServices _cobranzasServices;
        public CobranzasController(ICobranzasServices cobranzasServices)
        {
            _cobranzasServices = cobranzasServices;
        }

        [HttpGet("lotes/{lote}/deudores")]
        public async Task<IActionResult> ListarSociosDeudoresPorLote(string lote)
        {
            var result = await _cobranzasServices.ListarSociosDedudoresPorLote(lote);

            if(!result.Exit) return BadRequest(result.Errormessage);

            return Ok(result.Data);
        } 

    }
}
