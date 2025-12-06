using APIClub.Dtos.Cuota;
using APIClub.Interfaces.Services;
using APIClub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClub.Contrrollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuotasController : ControllerBase
    {
        private readonly ICuotaService _CuotasService;
        public CuotasController(ICuotaService cuotaService)
        {
            _CuotasService = cuotaService;
        }

        [HttpPost("actualizarValor")]
        public async Task<IActionResult> ActualizarValorCuota([FromBody] decimal nuevoValor)
        {
            var result = await _CuotasService.ActualizarValorCuota(nuevoValor);
            return Ok(new { result.Data });
        }

        [HttpPost("pagarCuota")]
        public async Task<IActionResult> RegistrarCuota([FromBody] RegistCuotaRequest request)
        {
            var result = await _CuotasService.RegistrarPagoCuoata(request.IdSocio, request.FormaPago);

            if (!result.Exit)
                return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(new { data = result.Data });
        }

       
    }
}
