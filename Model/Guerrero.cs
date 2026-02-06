using System.ComponentModel.DataAnnotations.Schema;

namespace GestorHeroesRPG.Model;

[Table("warrior")]
public class Guerrero : Personaje
{
    [Column("main_weapon")]
    public string ArmaPrincipal { get; set; } = null!;

    [Column("fury")]
    public int Furia { get; set; }
}