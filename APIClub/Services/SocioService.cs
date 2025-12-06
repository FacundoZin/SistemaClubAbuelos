using APIClub.Common;
using APIClub.Dtos.Cuota;
using APIClub.Dtos.Socios;
using APIClub.Interfaces.Repository;
using APIClub.Interfaces.Services;
using APIClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace APIClub.Services
{
    public class SocioService : ISocioService
    {
        private readonly ISocioRepository _SocioRepository;
        public SocioService(ISocioRepository socioRepository)
        {
            _SocioRepository = socioRepository;
        }


        public async Task<Result<CreatedSocio>> cargarSocio(CreateSocioDto _dto)
        {
            var exists = await _SocioRepository.SocioExists(_dto.Dni);

            if (exists)
            {
                return Result<CreatedSocio>.Error("el socio que quiere cargar ya fue cargado", 400);
            }

            var socio = new Socio
            {
                Nombre = _dto.Nombre,
                Apellido = _dto.Apellido,
                Dni = _dto.Dni,
                Telefono = _dto.Telefono,
                Direcccion = _dto.Direcccion,
                Lote = _dto.Lote,
                Localidad = _dto.Localidad
            };

            await _SocioRepository.cargarSocio(socio);

            return Result<CreatedSocio>.Exito(new CreatedSocio
            {
                Id = socio.Id,
                Nombre = socio.Nombre,
            });
        }

        public async Task<Result<PreviewSocioDto>> GetSocioByDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return Result<PreviewSocioDto>.Error("Debe indicar un DNI válido.", 400);
            }

            var socio = await _SocioRepository.GetSocioByDni(dni);

            if (socio is null)
            {
                return Result<PreviewSocioDto>.Error("No se encontró un socio con ese DNI.", 404);
            }

            var fechaPagoActual = DateOnly.FromDateTime(DateTime.Now);
            int anioActual = fechaPagoActual.Year;
            int semestreActual = fechaPagoActual.Month <= 6 ? 1 : 2;

            bool debeCuota = false;

            if (!socio.HistorialCuotas.Any(c => c.Anio == anioActual && c.Semestre == semestreActual))
                debeCuota = true;

            var dto = new PreviewSocioDto
            {
                Id = socio.Id,
                Nombre = socio.Nombre,
                Apellido = socio.Apellido,
                Dni = socio.Dni,
                Telefono = socio.Telefono,
                Direcccion = socio.Direcccion,
                Lote = socio.Lote,
                Localidad = socio.Localidad,
                AdeudaCuotas = debeCuota,
            };

            return Result<PreviewSocioDto>.Exito(dto);
        }

        public async Task<Result<List<PreviewSocioDto>>> GetSociosDeudores()
        {
            // Obtener fecha actual
            var fechaPagoActual = DateOnly.FromDateTime(DateTime.Now);
            int anioActual = fechaPagoActual.Year;
            int semestreActual = fechaPagoActual.Month <= 6 ? 1 : 2;

            //obtengo los socios que no pagaron la cuota
            var socios = await _SocioRepository.GetSociosDeudores(anioActual, semestreActual);

            //mapeo los socios a el dto que voy a devolver
            var dto = socios.Select(s => new PreviewSocioDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Apellido = s.Apellido,
                Dni = s.Dni,
                Telefono = s.Telefono,
                Direcccion = s.Direcccion,
                Lote = s.Lote,
                Localidad = s.Localidad,
                AdeudaCuotas = true,
            }).ToList();

            return Result<List<PreviewSocioDto>>.Exito(dto);
        }

        public async Task<Result<object>> UpdateSocio(int id, CreateSocioDto dto)
        {
            if (id <= 0)
            {
                return Result<object>.Error("El ID proporcionado no es válido.", 400);
            }

            var socio = await _SocioRepository.GetSocioById(id);

            if (socio is null)
            {
                return Result<object>.Error("No se encontró un socio con ese ID.", 404);
            }

            // Validar que el nuevo DNI no esté asignado a otro socio
            if (socio.Dni != dto.Dni)
            {
                var dniExists = await _SocioRepository.SocioExists(dto.Dni);
                if (dniExists)
                {
                    return Result<object>.Error("Ya existe un socio con ese DNI.", 400);
                }
            }

            // Actualizar propiedades
            socio.Nombre = dto.Nombre;
            socio.Apellido = dto.Apellido;
            socio.Dni = dto.Dni;
            socio.Telefono = dto.Telefono;
            socio.Direcccion = dto.Direcccion;
            socio.Lote = dto.Lote;
            socio.Localidad = dto.Localidad;

            await _SocioRepository.UpdateSocio(socio);

            return Result<object>.Exito(new
            {
                Message = "Socio actualizado correctamente.",
                SocioId = socio.Id
            });
        

        }
        public async Task<Result<object>> RemoveSocio(int id)
        {
            if (id <= 0)
            {
                return Result<object>.Error("El ID proporcionado no es válido.", 400);
            }

            var socio = await _SocioRepository.GetSocioById(id);

            if (socio is null)
            {
                return Result<object>.Error("No se encontró un socio con ese ID.", 404);
            }

            await _SocioRepository.RemoveSocios(socio);

            return Result<object>.Exito(new
            {
                Message = "Socio eliminado correctamente.",
                SocioId = id
            });
        }
        public async Task<Result<List<PreviewCuotaDto>>> GetHistorialCuotas(int socioId)
        {
            // Validar ID
            if (socioId <= 0)
                return Result<List<PreviewCuotaDto>>.Error("El ID del socio no es válido.", 400);

            // Buscar socio
            var socio = await _SocioRepository.GetSocioById(socioId);

            if (socio is null)
                return Result<List<PreviewCuotaDto>>.Error("No existe un socio con ese ID.", 404);

            // Obtener cuotas del socio
            var cuotas = await _SocioRepository.GetCuotasSocioById(socioId);

            if (cuotas is null || !cuotas.Any())
                return Result<List<PreviewCuotaDto>>.Exito(new List<PreviewCuotaDto>());

            // Mapear a DTO
            var dto = cuotas.Select(c => new PreviewCuotaDto
            {
                Id = c.Id,
                FechaPago = c.FechaPago,
                Importe = c.Monto,
                MetodoPago = c.FormaDePago.ToString()
            }).ToList();


            return Result<List<PreviewCuotaDto>>.Exito(dto);
        }





    }
}

