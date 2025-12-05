using APIClub.Common;
using APIClub.Enums;
using APIClub.Interfaces.Repository;
using APIClub.Interfaces.Services;
using APIClub.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace APIClub.Services
{
    public class CuotasService : ICuotaService
    {
        private readonly ISocioRepository _SocioRepository;
        private readonly ICuotaRepository _CuotaRepository;
        public CuotasService(ISocioRepository socioRepository, ICuotaRepository cuotaRepository) 
        {
            _SocioRepository = socioRepository; 
            _CuotaRepository = cuotaRepository;
        }

        public async Task<Result<object>> ActualizarValorCuota(decimal nuevoValor)
        {
            var fechaActualizacion = DateTime.Now;
            await _CuotaRepository.ActualizarValorCuota(nuevoValor, fechaActualizacion);
            return Result<object>.Exito("al valor de la cuoata ahora es $" + nuevoValor);
        }

        public async Task<Result<object>> RegistrarPagoCuoata(int idSocio, FormasDePago formaPago)
        {
            var socio = await _SocioRepository.GetSocioById(idSocio);
            if (socio == null)
                return Result<object>.Error("Socio no encontrado.", 404);

            // Obtener fecha actual
            var fechaPago = DateOnly.FromDateTime(DateTime.Now);
            int anio = fechaPago.Year;
            int semestre = fechaPago.Month <= 6 ? 1 : 2;

            // Validar que no exista una cuota igual en el mismo semestre y año
            bool cuotaExistente = socio.HistorialCuotas.Any(c =>
                c.Anio == anio && c.Semestre == semestre);

            if (cuotaExistente)
                return Result<object>.Error("Ya existe una cuota registrada para este semestre y año.", 409);

            var valorCuotaActual = await _CuotaRepository.ObtenerValorCuota();

            // Crear nueva cuota
            var nuevaCuota = new Cuota
            {
                FechaPago = fechaPago,
                Monto = valorCuotaActual,
                FormaDePago = formaPago,
                Anio = anio,
                Semestre = semestre,
                SocioId = socio.Id,
                Socio = socio
            };

            // Agregar cuota al historial y actualizar socio
            socio.HistorialCuotas.Add(nuevaCuota);
            await _SocioRepository.UpdateSocio(socio);

            return Result<object>.Exito(new
            {
                Mensaje = "Pago de cuota registrado exitosamente.",
                Cuota = nuevaCuota
            });

        }
    }
}
