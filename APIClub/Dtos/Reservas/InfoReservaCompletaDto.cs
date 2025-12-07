namespace APIClub.Dtos.Reservas
{
    public class InfoReservaCompletaDto
    {
        //info reserva
        public int IdReserva {  get; set; }
        public DateOnly FechaAlquiler { get; set; }
        public decimal Importe { get; set; }
        public decimal TotalPagado { get; set; } = 0;


        //info salon
        public string nombreSalon { get; set; }
        public string direccionSalon { get; set; }  


        // info socio
        public string nombreSocio { get; set; }
        public string apellidoSocio { get; set; }
        public string? telefonoSocio { get; set; }
        public string? direccionSocio { get; set; }
        public string? localidad {  get; set; } 
    }
}
