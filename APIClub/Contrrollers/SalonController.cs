using APIClub.Interfaces.Services;
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
            var result = await _SalonesServices.GetAlquilerBySalon(idSalon);

            if (!result.Exit)
                return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }


    }
}
