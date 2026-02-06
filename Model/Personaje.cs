using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GestorHeroesRPG.Model;

/// <summary>
/// Clase base abstracta que define la estructura principal de un personaje en el sistema RPG.
/// </summary>
/// <remarks>
/// <para><b>Autor:</b> Liviu</para>
/// <para><b>Versión:</b> 1.0 (Compatible con .NET 9)</para>
/// <para><b>Estrategia BD:</b> TPT (Table Per Type) - Mapea a la tabla principal 'character'.</para>
/// </remarks>
[Table("character")]
// Configuración de Polimorfismo: Permite a la API serializar/deserializar subclases automáticamente
/// <remarks>
/// <para><b>Autor:</b> María</para>
/// <para><b>Versión:</b> 1.0 (Compatible con .NET 9)</para>
/// </remarks>
[JsonDerivedType(typeof(Guerrero), typeDiscriminator: "guerrero")]
[JsonDerivedType(typeof(Mago), typeDiscriminator: "mago")]
[JsonDerivedType(typeof(Arquero), typeDiscriminator: "arquero")]
[JsonDerivedType(typeof(Clerigo), typeDiscriminator: "clerigo")]
public abstract class Personaje
{
    /// <summary>
    /// Identificador único del personaje.
    /// </summary>
    /// <value>Valor entero autoincremental generado por la base de datos.</value>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Nombre del personaje.
    /// </summary>
    /// <remarks>Longitud máxima de 50 caracteres. Es un campo obligatorio.</remarks>
    [Required]
    [StringLength(50)]
    [Column("name")]
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Nivel actual del personaje.
    /// </summary>
    /// <remarks>Rango permitido: 1 a 100.</remarks>
    [Range(1, 100)]
    [Column("level")]
    public int Nivel { get; set; }

    /// <summary>
    /// Fecha y hora en la que el personaje fue creado.
    /// </summary>
    /// <value>Almacenado como Timestamp en PostgreSQL.</value>
    [Required]
    [Column("creation_date")]
    public DateTime FechaCreation { get; set; }

    /// <summary>
    /// Nombre del gremio al que pertenece el personaje (opcional).
    /// </summary>
    /// <value>Cadena de texto o nulo si no tiene gremio.</value>
    [Column("guild")]
    public string? Gremio { get; set; }

    /// <summary>
    /// Campo dinámico para almacenar rasgos específicos y variables.
    /// </summary>
    /// <remarks>
    /// Mapeado como tipo <b>jsonb</b> en PostgreSQL. 
    /// Permite guardar estructuras diferentes para cada personaje sin necesidad de migrar la base de datos.
    /// </remarks>
    /// <example>
    /// <code>
    /// { "fuerza": 15, "habilidades": ["salto", "sigilo"] }
    /// </code>
    /// </example>
    [Column("traits", TypeName = "jsonb")]
    public JsonNode? Rasgos { get; set; }
}