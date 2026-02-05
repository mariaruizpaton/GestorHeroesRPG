using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace GestorHeroesRPG.Model;

[Table("character")]
public class Personaje
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Column("name")]
    public string Nombre { get; set; } = string.Empty;

    [Range(1, 100)]
    [Column("level")]
    public int Nivel { get; set; }

    [Required]
    [Column("creation_date")]
    public DateOnly FechaCreation { get; set; }

    [Column("guild")]
    public string Gremio { get; set; } = string.Empty;
}
