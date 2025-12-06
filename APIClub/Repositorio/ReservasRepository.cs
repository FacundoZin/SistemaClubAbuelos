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

        public async Task<List<ReservaSalon>> GetAlquileresBySalon(int IdSalon)
        {
            return await _Context.AlquileresSalones
                .Where(a => a.SalonId == IdSalon)
                .Include(s => s.Socio)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
