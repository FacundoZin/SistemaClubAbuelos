using APIClub.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace APIClub.Data
{
    public class AppDbcontext : DbContext
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options)
            : base(options)
        {
        }

        public DbSet<ReservaSalon> AlquileresSalones { get; set; }
        public DbSet<Cuota> Cuotas { get; set; }
        public DbSet<Salon> Salones { get; set; }
        public DbSet<Socio> Socios { get; set; }
        public DbSet<MontoCuota> MontoCuota { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Podés configurar relaciones, nombres de tablas, índices, etc.
            modelBuilder.Entity<Socio>()
                .HasMany(s => s.HistorialCuotas)
                .WithOne(c => c.Socio)
                .HasForeignKey(c => c.SocioId);

            modelBuilder.Entity<ReservaSalon>()
                .HasOne(a => a.Socio)
                .WithMany()  // suponiendo que no definiste collection ahí
                .HasForeignKey(a => a.SocioId);

            modelBuilder.Entity<ReservaSalon>()
                .HasOne(a => a.Salon)
                .WithMany()
                .HasForeignKey(a => a.SalonId);

            // Si querés podés mapear fecha como DateOnly (EF Core 6+ lo soporta con conversores)
            modelBuilder.Entity<ReservaSalon>()
                .Property(a => a.FechaAlquiler)
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly()),
                    v => DateOnly.FromDateTime(v));

            modelBuilder.Entity<Cuota>()
                .Property(c => c.FechaPago)
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly()),
                    v => DateOnly.FromDateTime(v));
        }
    }
}
