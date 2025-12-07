namespace APIClub.Dtos.Reservas
{
    public class CreteReservaSalonDto
    {
        public DateOnly Fecha { get; set; }
        public int SalonId { get; set; }
        public string DniSocio { get; set; } = string.Empty;
        public decimal Importe { get; set; }
        public decimal TotalPagado { get; set; }
    }
}
