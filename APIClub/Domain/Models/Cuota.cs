using APIClub.Enums;

namespace APIClub.Domain.Models
{
    public class Cuota
    {
        public int Id { get; set; }
        public DateOnly FechaPago { get; set; }
        public decimal Monto { get; set; }
        public FormasDePago FormaDePago { get; set; }
        public int Anio { get; set; }
        public int Semestre { get; set; }


        // Relaciones
        public int SocioId { get; set; }
        public Socio? Socio { get; set; }

    }
 
}
