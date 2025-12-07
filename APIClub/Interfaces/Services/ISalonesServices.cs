using APIClub.Common;
using APIClub.Dtos.Alquiler;
using APIClub.Dtos.Reservas;

namespace APIClub.Interfaces.Services
{
    public interface ISalonesServices
    {
        Task<Result<List<PreviewReservaBySalonDto>>> GetReservasBySalon(int salonId);
        Task<Result<FechaDisponible>> GetDisponibilidadFecha(DateOnly fecha, int Idsalon);
        Task<Result<InfoReservaCompletaDto?>> GetReservaByFechaAndSalon(DateOnly fecha, int salonId);
        Task<Result<InfoReservaCompletaDto?>> GetReservaById(int reservaId);
        Task<Result<object?>> RegistrarReservaSalon(CreteReservaSalonDto dto);
    }
}
