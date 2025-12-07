namespace APIClub.Dtos.Socios
{
    public class CreateSocioDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? Dni { get; set; }
        public string? Telefono { get; set; }
        public string? Direcccion { get; set; }
        public string? Lote { get; set; }
        public string? Localidad { get; set; }
    }
}
