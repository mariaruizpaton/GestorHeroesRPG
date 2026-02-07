using System.ComponentModel.DataAnnotations.Schema;

namespace GestorHeroesRPG.Model;

/// <summary>
/// Representa la clase de combatiente a distancia especializada en ataques de largo alcance y rastreo.
/// </summary>
/// <remarks>
/// <para><b>Autor:</b> Liviu</para>
/// <para><b>Versión:</b> 1.0</para>
/// <para><b>Estrategia BD:</b> TPT - Mapea a la tabla específica 'archer', vinculada por Id a 'character'.</para>
/// </remarks>
[Table("archer")]
public class Arquero : Personaje
{
    /// <summary>
    /// Porcentaje o índice de acierto del arquero en ataques a distancia.
    /// </summary>
    /// <value>Valor decimal (double) que representa la exactitud del personaje.</value>
    [Column("precision")]
    public double Precision { get; set; }

    /// <summary>
    /// Indica si el arquero posee un compañero animal que le asiste en combate.
    /// </summary>
    /// <remarks>Mapeado como un valor booleano en la base de datos.</remarks>
    [Column("has_pet")]
    public bool TieneMascota { get; set; }
}