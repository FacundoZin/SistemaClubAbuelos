using APIClub.Dtos.Reservas;
using APIClub.Interfaces.Services;
using APIClub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClub.Contrrollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalonController : ControllerBase
    {
        private readonly ISalonesServices _SalonesServices;

        public SalonController (ISalonesServices salonesServices)
        {
            _SalonesServices = salonesServices;
        }

        [HttpGet("{idSalon}/reservas")]
        public async Task<IActionResult> GetReservasBySalon(int idSalon)
        {
            var result = await _SalonesServices.GetReservasBySalon(idSalon);

            if (!result.Exit)
                return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }

        [HttpGet("{salonId}/disponibilidad")]
        public async Task<IActionResult> GetDisponibilidad([FromQuery] DateOnly fecha, int salonId)
        {
            var result = await _SalonesServices.GetDisponibilidadFecha(fecha, salonId);

            if (!result.Exit)
                return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result);
        }

        [HttpGet("{salonId}/reserva")]
        public async Task<IActionResult> GetReservaByFecha(DateOnly fecha, int salonId)
        {
            var result = await _SalonesServices.GetReservaByFechaAndSalon(fecha, salonId);

            if (!result.Exit)
                return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result);
        }

        [HttpGet("reserva/{reservaId}")]
        public async Task<IActionResult> GetReservaById(int reservaId)
        {
            var result = await _SalonesServices.GetReservaById(reservaId);

            if (!result.Exit)
                return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result);
        }

        [HttpPost("reserva")]
        public async Task<IActionResult> CrearReserva([FromBody] CreteReservaSalonDto dto)
        {
            var result = await _SalonesServices.RegistrarReservaSalon(dto);

            if (!result.Exit)
                return StatusCode(result.Errorcode, result);

            return Ok(result);
        }

    }
}
