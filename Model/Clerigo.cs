using System.ComponentModel.DataAnnotations.Schema;

namespace GestorHeroesRPG.Model;

[Table("cleric")]
public class Clerigo : Personaje
{
    [Column("deity")]
    public string Divinidad { get; set; } = null!;

    [Column("healing_points")]
    public int PuntosSanacion { get; set; }
}
