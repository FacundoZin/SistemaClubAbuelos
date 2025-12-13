using APIClub.Domain.Services;
using APIClub.Dtos.Cuota;
using APIClub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClub.Contrrollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuotasController : ControllerBase
    {
        private readonly ICuotasService _CuotasService;
        public CuotasController(ICuotasService cuotaService)
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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _CuotasService.RegistrarPagoCuoata(request.IdSocio, request.FormaPago);

            if (!result.Exit)
                return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(new { data = result.Data });
        }

       
    }
}
