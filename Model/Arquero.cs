using System.ComponentModel.DataAnnotations.Schema;

namespace GestorHeroesRPG.Model;

[Table("archer")]
public class Arquero : Personaje
{
    [Column("precision")]
    public double Precision { get; set; }

    [Column("has_pet")]
    public bool TieneMascota { get; set; }
}
