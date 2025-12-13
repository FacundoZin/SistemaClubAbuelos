namespace APIClub.Domain.Interfaces.Repository
{
    public interface ICuotaRepository
    {
        Task<decimal> ObtenerValorCuota();
        Task<decimal> ActualizarValorCuota(decimal valor, DateTime FechaActualizacion);
    }
}
