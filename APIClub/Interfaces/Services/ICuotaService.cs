using APIClub.Common;
using APIClub.Enums;

namespace APIClub.Interfaces.Services
{
    public interface ICuotaService
    {
        Task<Result<object>> RegistrarPagoCuoata(int idSocio, FormasDePago formaPago);
        Task<Result<object>> ActualizarValorCuota(decimal nuevoValor);
       
    }
}
