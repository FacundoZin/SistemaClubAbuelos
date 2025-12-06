namespace APIClub.Dtos.Socios
{
    public class PreviewSocioDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Dni { get; set; }
        public string? Telefono { get; set; }
        public string? Direcccion { get; set; }
        public string? Lote { get; set; }
        public string? Localidad { get; set; }
        public DateOnly? FechaAsociacion { get; set; }

        public bool AdeudaCuotas { get; set; }

    }
}
