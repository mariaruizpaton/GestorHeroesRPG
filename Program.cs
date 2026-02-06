using GestorHeroesRPG.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// 1. Obtener la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("GameConn");

// 2. Crear un DataSource que habilite el JSON dinámico
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();

// 3. Inyectar el DbContext usando el DataSource configurado
builder.Services.AddDbContext<GameDBContext>(options =>
    options.UseNpgsql(dataSource));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esto permite que el JSON se lea y escriba correctamente 
        // sin problemas con los tipos dinámicos de System.Text.Json
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
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