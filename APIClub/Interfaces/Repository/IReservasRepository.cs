using APIClub.Models;

namespace APIClub.Interfaces.Repository
{
    public interface IReservasRepository
    {
        Task<List<ReservaSalon>> GetAlquileresBySalon(int IdSalon);
    }
}
