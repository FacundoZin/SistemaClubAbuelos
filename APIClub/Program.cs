using APIClub.Data;
using APIClub.Domain.Interfaces.Repository;
using APIClub.Domain.Services;
using APIClub.Repositorio;
using APIClub.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=club.db";


builder.Services.AddDbContext<AppDbcontext>(options =>
    options.UseSqlite(connectionString));

//registrar servicios
builder.Services.AddScoped<ISociosManagmentService,SociosManagmentService>();
builder.Services.AddScoped<ICuotasService,CuotasService>();
builder.Services.AddScoped<IReservasServices,ReservasServices>();
builder.Services.AddScoped<ICobranzasServices,CobranzasService>();

// registrar repositorios
builder.Services.AddScoped<ISocioRepository,SociosRepository>();
builder.Services.AddScoped<ICuotaRepository,CuotaRepository>();
builder.Services.AddScoped<IReservasRepository,ReservasRepository>();


// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
