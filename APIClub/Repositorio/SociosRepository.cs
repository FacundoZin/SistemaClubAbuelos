using APIClub.Data;
using APIClub.Interfaces.Repository;
using APIClub.Models;
using Microsoft.EntityFrameworkCore;

namespace APIClub.Repositorio
{
    public class SociosRepository : ISocioRepository
    {
        private readonly AppDbcontext _Dbcontext;

        public SociosRepository(AppDbcontext dbcontext) 
        {
            _Dbcontext = dbcontext;
        }
        
        public async Task cargarSocio(Socio socio)
        {
            _Dbcontext.Socios.Add(socio);
            await _Dbcontext.SaveChangesAsync();
        }

        public async Task<Socio?> GetSocioByDni(string dni)
        {
            return await _Dbcontext.Socios
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Dni == dni);
        }

        public async Task<bool> SocioExists(string dni)
        {
            return await _Dbcontext.Socios.AnyAsync(s => s.Dni == dni);
        }

        public async Task<bool> SocioExistsForUpdate(string dni, int id)
        {
            return await _Dbcontext.Socios.AnyAsync(s => s.Dni == dni && s.Id != id);
        }

        public async Task<Socio?> GetSocioById(int id)
        {
            return await _Dbcontext.Socios
                .FirstOrDefaultAsync(s => s.Id == id);
                
        }

        public async Task UpdateSocio(Socio socio)
        {
           _Dbcontext.Socios.Update(socio); 
             await _Dbcontext.SaveChangesAsync();
        }

        public Task<Socio?> GetSocioByIdWithCuotas(int id)
        {
            return _Dbcontext.Socios.Include(s => s.HistorialCuotas).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task RemoveSocios(Socio socio)
        {
            _Dbcontext.Socios.Remove(socio);
            await _Dbcontext.SaveChangesAsync();
        }

        public async Task<List<Cuota>>GetCuotasSocioById(int socioId)
        {
        return await _Dbcontext.Cuotas .Where(c => c.SocioId == socioId)
        .OrderByDescending(c => c.FechaPago) .ToListAsync();
        }
    }
}
