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
        Task<Socio?> GetSocioByIdWithCuotas(int id);
        Task<List<Socio>> GetSociosDeudores(int anioActual, int semestreActual);
        Task RemoveSocios(Socio socio);
        Task<List<Cuota>> GetCuotasSocioById(int socioId);
        Task<List<Socio>> GetSociosDeudoresByLote(string lote, int anioActual, int semestreActual);
    }
}
