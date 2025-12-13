using APIClub.Common;
using APIClub.Enums;

namespace APIClub.Domain.Services
{
    public interface ICuotasService
    {
        Task<Result<object>> RegistrarPagoCuoata(int idSocio, FormasDePago formaPago);
        Task<Result<object>> ActualizarValorCuota(decimal nuevoValor);
       
    }
}
