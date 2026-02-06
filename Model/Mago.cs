using System.ComponentModel.DataAnnotations.Schema;

namespace GestorHeroesRPG.Model;

[Table("mage")]
public class Mago : Personaje
{
    [Column("mana")]
    public int Mana { get; set; }

    [Column("main_element")]
    public string ElementoPrincipal { get; set; } = null!;
}
