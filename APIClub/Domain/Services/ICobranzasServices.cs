using APIClub.Common;
using APIClub.Dtos.Socios;

namespace APIClub.Domain.Services
{
    public interface ICobranzasServices
    {
        Task<Result<List<PreviewSocioForCobranzaDto>>> ListarSociosDedudoresPorLote(string lote); 
    }
}
