using APIClub.Common;
using APIClub.Dtos.Alquiler;
using APIClub.Interfaces.Repository;
using APIClub.Interfaces.Services;

namespace APIClub.Services
{
    public class SalonesServices : ISalonesServices
    {
        private readonly IReservasRepository _AlquilerRepository;

        public SalonesServices(IReservasRepository alquilerRepository)
        {
            _AlquilerRepository = alquilerRepository;
        }

        public async Task<Result<List<PreviewReservaBySalonDto>>> GetAlquilerBySalon(int salonId)
        {
            var alquileres = await _AlquilerRepository.GetAlquileresBySalon(salonId);

            var dto = alquileres.Select(a => new PreviewReservaBySalonDto
            {
                Id = a.Id,
                FechaAlquiler = a.FechaAlquiler,
                Pagado = a.Importe == a.TotalPagado ? true : false,
                NombreReservante = $"{a.Socio.Nombre} {a.Socio.Apellido}"
            }).ToList();

            return Result<List<PreviewReservaBySalonDto>>.Exito(dto);
        }
    }
}
