using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GestorHeroesRPG.Model;

[Table("character")]
public abstract class Personaje
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Column("name")]
    public string Nombre { get; set; } = null!;

    [Range(1, 100)]
    [Column("level")]
    public int Nivel { get; set; }

    [Required]
    [Column("creation_date")]
    public DateTime FechaCreation { get; set; }

    [Column("guild")]
    public string? Gremio { get; set; }

    /*[Column("traits", TypeName = "jsonb")]
    public JsonElement Rasgos { get; set; }*/
}
