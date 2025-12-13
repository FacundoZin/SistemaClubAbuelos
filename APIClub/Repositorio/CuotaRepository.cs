using APIClub.Data;
using APIClub.Domain.Interfaces.Repository;
using APIClub.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace APIClub.Repositorio
{
    public class CuotaRepository : ICuotaRepository
    {
        private readonly AppDbcontext _dbcontext;

        public CuotaRepository(AppDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<decimal> ActualizarValorCuota(decimal valor, DateTime FechaActualizacion)
        {
            var nuevoMonto = new MontoCuota { MontoCuotaFija = valor, FechaActualizacion = FechaActualizacion };

            _dbcontext.MontoCuota.Add(nuevoMonto);
            await _dbcontext.SaveChangesAsync();

            return nuevoMonto.MontoCuotaFija;
        }

        public async Task<decimal> ObtenerValorCuota()
        {
            var valorCuota = await _dbcontext.MontoCuota
                .OrderByDescending(cuota => cuota.FechaActualizacion)
                .Select(cuota => cuota.MontoCuotaFija)
                .FirstOrDefaultAsync();

            return valorCuota;
        }
    }
}
