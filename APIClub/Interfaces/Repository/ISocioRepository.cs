using APIClub.Models;

namespace APIClub.Interfaces.Repository
{
    public interface ISocioRepository
    {
        Task cargarSocio(Socio socio);
        Task<bool> SocioExists(string dni);
        Task<Socio?> GetSocioByDni(string dni); 
        Task<Socio?> GetSocioById(int id);
        Task UpdateSocio(Socio socio);
        Task<bool> SocioExistsForUpdate(string dni, int id);
    }
}
