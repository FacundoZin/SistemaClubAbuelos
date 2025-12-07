using APIClub.Models;

namespace APIClub.Interfaces.Repository
{
    public interface IReservasRepository
    {
        Task<List<ReservaSalon>> GetAlquileresBySalon(int IdSalon);
        Task<ReservaSalon?> SearchReservaByFecha(DateOnly fehca, int IdSalon);
        Task<bool> verificarDisponibilidad(DateOnly fecha, int IdSalon);
        Task<ReservaSalon?> SearchReservaById(int idReserva);
        Task<bool> CrearReserva(ReservaSalon reserva);
        Task<bool> BorrarReserva(int idReserva);
    }
}
