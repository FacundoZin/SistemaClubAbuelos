using APIClub.Common;
using APIClub.Dtos.Cuota;
using APIClub.Dtos.Socios;

namespace APIClub.Domain.Services
{
    public interface ISociosManagmentService
    {
        Task<Result<List<PreviewSocioDto>>> GetSociosDeudores();
        Task<Result<CreatedSocio>> cargarSocio(CreateSocioDto _dto);
        Task<Result<PreviewSocioDto>> GetSocioByDni(string dni);
        Task<Result<object>> UpdateSocio(int id, CreateSocioDto dto);
        Task<Result<object>> RemoveSocio(int id);
        Task<Result<List<PreviewCuotaDto>>> GetHistorialCuotas(int socioId);

    }
}
