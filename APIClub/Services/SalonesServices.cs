using APIClub.Common;
using APIClub.Dtos.Alquiler;
using APIClub.Dtos.Reservas;
using APIClub.Interfaces.Repository;
using APIClub.Interfaces.Services;
using APIClub.Models;
using System.Security.Cryptography.X509Certificates;

namespace APIClub.Services
{
    public class SalonesServices : ISalonesServices
    {
        private readonly IReservasRepository _ReservasRepository;
        private readonly ISocioRepository _SocioRepository;

        public SalonesServices(IReservasRepository alquilerRepository, ISocioRepository socioRepository)
        {
            _ReservasRepository = alquilerRepository;
            _SocioRepository = socioRepository;
        }

        public async Task<Result<List<PreviewReservaBySalonDto>>> GetReservasBySalon(int salonId)
        {
            var alquileres = await _ReservasRepository.GetAlquileresBySalon(salonId);

            var dto = alquileres.Select(a => new PreviewReservaBySalonDto
            {
                Id = a.Id,
                FechaAlquiler = a.FechaAlquiler,
                Pagado = a.Importe == a.TotalPagado ? true : false,
                NombreReservante = $"{a.Socio.Nombre} {a.Socio.Apellido}"
            }).ToList();

            return Result<List<PreviewReservaBySalonDto>>.Exito(dto);
        }

        public async Task<Result<FechaDisponible>> GetDisponibilidadFecha(DateOnly fecha, int salon)
        {
            var reserva = await _ReservasRepository.SearchReservaByFecha(fecha, salon);

            FechaDisponible dto = new FechaDisponible();

            if(reserva != null)
            {
   
                dto.Mensaje = $"el salon no se encuntra disponible en esta fecha: {fecha}";
                dto.Disponible = false;

                return Result<FechaDisponible>.Exito(dto);
            }

            dto.Mensaje = $"el salon {reserva.Salon.Name} esta disponible para esta fecha: {fecha}";
            dto.Disponible= true;

            return Result<FechaDisponible>.Exito(dto);  
        }

        public async Task<Result<InfoReservaCompletaDto?>> GetReservaByFechaAndSalon(DateOnly fecha, int salonId)
        {
            var reserva = await _ReservasRepository.SearchReservaByFecha(fecha, salonId);

            if(reserva == null)
            {
                return Result<InfoReservaCompletaDto?>.Exito(null);
            }

            InfoReservaCompletaDto infoReserva = new InfoReservaCompletaDto();

            infoReserva.IdReserva = reserva.Id;
            infoReserva.FechaAlquiler = reserva.FechaAlquiler;
            infoReserva.Importe = reserva.Importe;
            infoReserva.Importe = reserva.Importe;
            infoReserva.TotalPagado = reserva.TotalPagado;

            infoReserva.nombreSalon = reserva.Salon.Name;
            infoReserva.direccionSalon = reserva.Salon.Direccion;

            infoReserva.nombreSocio = reserva.Socio.Nombre;
            infoReserva.apellidoSocio = reserva.Socio.Apellido;
            infoReserva.telefonoSocio = reserva.Socio.Telefono;
            infoReserva.direccionSocio = reserva.Socio.Direcccion;
            infoReserva.localidad = reserva.Socio.Localidad;


            return Result<InfoReservaCompletaDto?>.Exito(infoReserva);
        }

        public async Task<Result<InfoReservaCompletaDto?>> GetReservaById(int reservaId)
        {
            var reserva = await _ReservasRepository.SearchReservaById(reservaId);

            if (reserva == null)
            {
                return Result<InfoReservaCompletaDto?>.Exito(null);
            }

            InfoReservaCompletaDto infoReserva = new InfoReservaCompletaDto();

            infoReserva.IdReserva = reserva.Id;
            infoReserva.FechaAlquiler = reserva.FechaAlquiler;
            infoReserva.Importe = reserva.Importe;
            infoReserva.Importe = reserva.Importe;
            infoReserva.TotalPagado = reserva.TotalPagado;

            infoReserva.nombreSalon = reserva.Salon.Name;
            infoReserva.direccionSalon = reserva.Salon.Direccion;

            infoReserva.nombreSocio = reserva.Socio.Nombre;
            infoReserva.apellidoSocio = reserva.Socio.Apellido;
            infoReserva.telefonoSocio = reserva.Socio.Telefono;
            infoReserva.direccionSocio = reserva.Socio.Direcccion;
            infoReserva.localidad = reserva.Socio.Localidad;


            return Result<InfoReservaCompletaDto?>.Exito(infoReserva);
        }

        public async Task<Result<object?>> RegistrarReservaSalon(CreteReservaSalonDto dto)
        {

            var disponible = await _ReservasRepository.verificarDisponibilidad(dto.Fecha, dto.SalonId);

            if (!disponible)
                return Result<object?>.Error(
                    "El salón no está disponible en la fecha seleccionada.",
                    409
                );


            var socio = await _SocioRepository.GetSocioByDni(dto.DniSocio);

            if (socio == null) 
                return Result<object?>.Error("el socio que esta cargando para hacer la reserva no existe, verifique si ingreso bien  su dni", 404);

            ReservaSalon reserva = new ReservaSalon();

            reserva.FechaAlquiler = dto.Fecha;
            reserva.Importe = dto.Importe;
            reserva.TotalPagado = dto.TotalPagado;
            reserva.SocioId = socio.Id;
            reserva.SalonId = dto.SalonId;  

            bool exit = await _ReservasRepository.CrearReserva(reserva);

            if(exit == false)
                return Result<object?>.Error("error al registrar la reserva en nuestra base de datos", 500);

            reservaCreatedDto reservaCreada = new reservaCreatedDto
            {
                idReserva = reserva.Id,
                mensaje = $"se creo exitosamente una reserva para el: {reserva.FechaAlquiler}" 
            };
            return Result<object?>.Exito(reservaCreada);
        }
    }
}
