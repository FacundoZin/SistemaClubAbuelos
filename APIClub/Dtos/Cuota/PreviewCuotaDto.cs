namespace APIClub.Dtos.Cuota
{
    public class PreviewCuotaDto
    {
        public int Id { get; set; }
        public DateOnly FechaPago { get; set; }
        public decimal Importe { get; set; }
        public string MetodoPago { get; set; }
    }
}
