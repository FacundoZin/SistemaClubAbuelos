using APIClub.Data;
using APIClub.Interfaces.Repository;
using APIClub.Models;
using Microsoft.EntityFrameworkCore;

namespace APIClub.Repositorio
{
    public class ReservasRepository : IReservasRepository
    {
        private readonly AppDbcontext _Context;

        public ReservasRepository(AppDbcontext dbcontext)
        {
            _Context = dbcontext;
        }

        public async Task<bool> BorrarReserva(int idReserva)
        {
            var reserva = await _Context.ReservasSalones.FirstOrDefaultAsync(r => r.Id == idReserva);

            if(reserva == null) return false;

            if (await _Context.SaveChangesAsync() == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CrearReserva(ReservaSalon reserva)
        {
            _Context.ReservasSalones.Add(reserva);
            
            if(await _Context.SaveChangesAsync() == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<List<ReservaSalon>> GetAlquileresBySalon(int IdSalon)
        {
            return await _Context.ReservasSalones
                .Where(a => a.SalonId == IdSalon)
                .Include(s => s.Socio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ReservaSalon?> SearchReservaByFecha(DateOnly fehca, int IdSalon)
        {
            return await _Context.ReservasSalones
                .Include(r => r.Salon)
                .Include (s => s.Socio)
                .FirstOrDefaultAsync(r => r.SalonId == IdSalon && r.FechaAlquiler == fehca);
        }

        public async Task<ReservaSalon?> SearchReservaById(int idReserva)
        {
            return await _Context.ReservasSalones
                .Include(r => r.Salon)
                .Include(s => s.Socio)
                .FirstOrDefaultAsync(r => r.Id == idReserva);
        }

        public async Task<bool> verificarDisponibilidad(DateOnly fecha, int IdSalon)
        {
            bool existeReserva = await _Context.ReservasSalones
                .AnyAsync(r => r.SalonId == IdSalon && r.FechaAlquiler == fecha);

            
            return !existeReserva;
        }
    }
}
