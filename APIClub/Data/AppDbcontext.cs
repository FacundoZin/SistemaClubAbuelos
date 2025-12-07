using APIClub.Enums;
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

        public DbSet<ReservaSalon> ReservasSalones { get; set; }
        public DbSet<Cuota> Cuotas { get; set; }
        public DbSet<Salon> Salones { get; set; }
        public DbSet<Socio> Socios { get; set; }
        public DbSet<MontoCuota> MontoCuota { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------
            // Configuraciones básicas
            // ---------------------------
            modelBuilder.Entity<Socio>(entity =>
            {
                entity.Property(s => s.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Dni).HasMaxLength(20);
                entity.HasMany(s => s.HistorialCuotas)
                      .WithOne(c => c.Socio)
                      .HasForeignKey(c => c.SocioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Salon>(entity =>
            {
                entity.Property(s => s.Name).IsRequired().HasMaxLength(150);
                entity.Property(s => s.Direccion).HasMaxLength(250);
            });

            modelBuilder.Entity<MontoCuota>(entity =>
            {
                entity.Property(m => m.MontoCuotaFija).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Cuota>(entity =>
            {
                entity.Property(c => c.Monto).HasColumnType("decimal(18,2)");
                entity.Property(c => c.Anio);
                entity.Property(c => c.Semestre);
            });

            modelBuilder.Entity<ReservaSalon>(entity =>
            {
                entity.Property(r => r.Importe).HasColumnType("decimal(18,2)");
                entity.Property(r => r.TotalPagado).HasColumnType("decimal(18,2)");

                entity.HasOne(r => r.Socio)
                      .WithMany() 
                      .HasForeignKey(r => r.SocioId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Salon)
                      .WithMany()
                      .HasForeignKey(r => r.SalonId)
                      .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<ReservaSalon>()
                .Property(a => a.FechaAlquiler)
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly(0, 0)),
                    v => DateOnly.FromDateTime(v));

            modelBuilder.Entity<Cuota>()
                .Property(c => c.FechaPago)
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly(0, 0)),
                    v => DateOnly.FromDateTime(v));

            // ---------------------------
            // Seed data (datos de prueba)
            // ---------------------------

            // 1) Salones
            modelBuilder.Entity<Salon>().HasData(
                new Salon
                {
                    Id = 1,
                    Name = "Salón Central",
                    Direccion = "Calle Falsa 123"
                },
                new Salon
                {
                    Id = 2,
                    Name = "Salón Norte",
                    Direccion = "Av. Siempre Viva 742"
                }
            );

            // 2) Socios
            modelBuilder.Entity<Socio>().HasData(
                new Socio
                {
                    Id = 1,
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Dni = "12345678",
                    Telefono = "341-1234567",
                    Direcccion = "Mitre 100",
                    Localidad = "Rosario",
                    FechaAsociacion = DateOnly.FromDateTime(new DateTime(2020, 5, 10))
                },
                new Socio
                {
                    Id = 2,
                    Nombre = "María",
                    Apellido = "Gómez",
                    Dni = "87654321",
                    Telefono = "341-7654321",
                    Direcccion = "San Martín 200",
                    Localidad = "Córdoba",
                    FechaAsociacion = DateOnly.FromDateTime(new DateTime(2021, 3, 15))
                }
            );

            // 3) MontoCuota (registro de ejemplo)
            modelBuilder.Entity<MontoCuota>().HasData(
                new MontoCuota
                {
                    Id = 1,
                    MontoCuotaFija = 2500.00m,
                    FechaActualizacion = new DateTime(2025, 01, 01)
                }
            );

            // 4) Cuotas (historial de pagos para socio 1)
            modelBuilder.Entity<Cuota>().HasData(
                new
                {
                    Id = 1,
                    FechaPago = DateOnly.FromDateTime(new DateTime(2024, 3, 1)),
                    Monto = 2500.00m,
                    FormaDePago = (FormasDePago)1, // ajustá si necesitás un valor concreto del enum
                    Anio = 2024,
                    Semestre = 1,
                    SocioId = 1
                },
                new
                {
                    Id = 2,
                    FechaPago = DateOnly.FromDateTime(new DateTime(2024, 9, 1)),
                    Monto = 2500.00m,
                    FormaDePago = (FormasDePago)2,
                    Anio = 2024,
                    Semestre = 2,
                    SocioId = 1
                }
            );

            // 5) Reservas de prueba
            modelBuilder.Entity<ReservaSalon>().HasData(
                new
                {
                    Id = 1,
                    FechaAlquiler = DateOnly.FromDateTime(new DateTime(2025, 5, 20)),
                    Importe = 5000.00m,
                    TotalPagado = 0.00m,
                    SocioId = 1,
                    SalonId = 1
                },
                new
                {
                    Id = 2,
                    FechaAlquiler = DateOnly.FromDateTime(new DateTime(2025, 6, 15)),
                    Importe = 7000.00m,
                    TotalPagado = 7000.00m,
                    SocioId = 2,
                    SalonId = 2
                }
            );
        }
    }
}
