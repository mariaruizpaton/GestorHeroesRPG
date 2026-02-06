using GestorHeroesRPG.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
namespace GestorHeroesRPG.Data;


public class GameDBContext : DbContext
{
    public GameDBContext() { }

    public GameDBContext(DbContextOptions<GameDBContext> options) : base(options) { }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5441;Database=DDSoft;SearchPath=game;Username=ADMIN_GAME;Password=Abcd1234");
        }
    }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("game");
    }

    public DbSet<Personaje> Personajes { get; set; }
    public DbSet<Mago> Magos { get; set; }
    public DbSet<Guerrero> Guerreros { get; set; }
    public DbSet<Clerigo> Clerigos { get; set; }
    public DbSet<Arquero> Arqueros { get; set; }
}
