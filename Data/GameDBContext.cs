using GestorHeroesRPG.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestorHeroesRPG.Data;

/// <summary>
/// Contexto de base de datos para la gestión del MMORPG de D&DSoft.
/// </summary>
/// <remarks>
/// <b>Autor:</b> Maria
/// </remarks>
public class GameDBContext : DbContext
{
    /// <summary>
    /// Constructor por defecto del contexto.
    /// </summary>
    public GameDBContext() { }

    /// <summary>
    /// Constructor que acepta opciones de configuración para el contexto.
    /// </summary>
    /// <param name="options">Opciones del DbContext.</param>
    public GameDBContext(DbContextOptions<GameDBContext> options) : base(options) { }

    /// <summary>
    /// Configuración del modelo de datos, jerarquía de herencia y mapeo de tablas.
    /// </summary>
    /// <param name="modelBuilder">Constructor de modelos para EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Establece el esquema por defecto en PostgreSQL como "game"
        modelBuilder.HasDefaultSchema("game");

        /// <summary>
        /// Configuración de Estrategia TPT (Table Per Type).
        /// Cada clase derivada se mapea a su propia tabla física.
        /// </summary>
        modelBuilder.Entity<Personaje>().ToTable("character");
        modelBuilder.Entity<Guerrero>().ToTable("warrior");
        modelBuilder.Entity<Mago>().ToTable("mage");
        modelBuilder.Entity<Arquero>().ToTable("archer");
        modelBuilder.Entity<Clerigo>().ToTable("cleric");

        /// <summary>
        /// Mapeo de la propiedad 'Rasgos' como tipo nativo jsonb de PostgreSQL.
        /// Permite el almacenamiento de estructuras de datos dinámicas.
        /// </summary>
        modelBuilder.Entity<Personaje>()
            .Property(p => p.Rasgos)
            .HasColumnType("jsonb");
    }

    /// <summary>
    /// Colección polimórfica de todos los personajes del juego.
    /// </summary>
    public DbSet<Personaje> Personajes { get; set; }

    /// <summary>
    /// Colección específica de personajes de tipo Mago.
    /// </summary>
    public DbSet<Mago> Magos { get; set; }

    /// <summary>
    /// Colección específica de personajes de tipo Guerrero.
    /// </summary>
    public DbSet<Guerrero> Guerreros { get; set; }

    /// <summary>
    /// Colección específica de personajes de tipo Clérigo.
    /// </summary>
    public DbSet<Clerigo> Clerigos { get; set; }

    /// <summary>
    /// Colección específica de personajes de tipo Arquero.
    /// </summary>
    public DbSet<Arquero> Arqueros { get; set; }
}