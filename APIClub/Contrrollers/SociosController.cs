using APIClub.Dtos.Socios;
using APIClub.Interfaces.Services;
using APIClub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClub.Contrrollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SociosController : ControllerBase
    {
        private readonly ISocioService _SocioService;
        public SociosController(ISocioService socioService)
        {
            _SocioService = socioService;
        }

        [HttpPost]
        public async Task<IActionResult> CargarSocio([FromBody] CreateSocioDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _SocioService.cargarSocio(dto);

            if (result.Exit != true)
            {
                return StatusCode(result.Errorcode, result.Errormessage);
            }

            return Ok(result.Data);
        }


        [HttpGet("{dni}")]
        public async Task<IActionResult> GetSocioByDni(string dni)
        {
            var result = await _SocioService.GetSocioByDni(dni);

            if (result.Exit != true)
            {
                // Usa el mismo manejo de errores que en CargarSocio
                return StatusCode(result.Errorcode, result.Errormessage);
            }

            // Si todo salió bien, devolvés el DTO con código 200
            return Ok(result.Data);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSocio(int id, [FromBody] CreateSocioDto dto)
        {
            var result = await _SocioService.UpdateSocio(id, dto);

            if (result.Exit != true)
            {
                return StatusCode(result.Errorcode, result.Errormessage);
            }

            return Ok(result.Data);
        }


        [HttpGet("deudores")]
        public async Task<IActionResult> GetSociosDeudores()
        {
            var result = await _SocioService.GetSociosDeudores();

            if (result.Exit != true)
            {
                return StatusCode(result.Errorcode, result.Errormessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSocio(int id)
        {
            var result = await _SocioService.RemoveSocio(id);

            if (!result.Exit)
            {
                return StatusCode(result.Errorcode, result.Errormessage);
            }

            return Ok(result.Data);
        }


        [HttpGet("{socioId}/cuotas")]
        public async Task<IActionResult> GetHistorialCuotas(int socioId)
        {
            var result = await _SocioService.GetHistorialCuotas(socioId);

            if (!result.Exit)
                return StatusCode(result.Errorcode, new
                {
                    mensaje = result.Errormessage,
                });

            return Ok(result.Data);
        }
    }
}
