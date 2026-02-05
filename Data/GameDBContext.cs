using Microsoft.EntityFrameworkCore;
using System;
namespace GestorHeroesRPG.Data;

public class GameDBContext : DbContext
{
    public GameDBContext(DbContextOptions<GameDBContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5441;Database=D&DSoft;SearchPath=game;Username=ADMIN_GAME;Password=Abcd1234");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.HasDefaultSchema("game");

        base.OnModelCreating(modelBuilder);
    }
}
