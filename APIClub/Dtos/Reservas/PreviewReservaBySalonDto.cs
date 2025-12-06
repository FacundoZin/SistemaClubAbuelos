namespace APIClub.Dtos.Alquiler
{
    public class PreviewReservaBySalonDto
    {
        public int Id { get; set; }
        public DateOnly FechaAlquiler { get; set; }
        public bool Pagado { get; set; }
        public string NombreReservante { get; set; }
    }
}
