using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GestorHeroesRPG.Model;

[Table("character")]
[JsonDerivedType(typeof(Guerrero), typeDiscriminator: "guerrero")]
[JsonDerivedType(typeof(Mago), typeDiscriminator: "mago")]
[JsonDerivedType(typeof(Arquero), typeDiscriminator: "arquero")]
[JsonDerivedType(typeof(Clerigo), typeDiscriminator: "clerigo")]
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

    [Column("traits", TypeName = "jsonb")]
    public JsonNode? Rasgos { get; set; }
}
