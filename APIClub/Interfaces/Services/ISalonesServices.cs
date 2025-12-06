using APIClub.Common;
using APIClub.Dtos.Alquiler;

namespace APIClub.Interfaces.Services
{
    public interface ISalonesServices
    {
        Task<Result<List<PreviewReservaBySalonDto>>> GetAlquilerBySalon(int salonId);
    }
}
