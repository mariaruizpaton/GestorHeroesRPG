using System.ComponentModel.DataAnnotations.Schema;

namespace GestorHeroesRPG.Model;

/// <summary>
/// Representa la clase de apoyo espiritual y sanación divina.
/// </summary>
/// <remarks>
/// <para><b>Autor:</b> Liviu</para>
/// <para><b>Versión:</b> 1.0</para>
/// <para><b>Estrategia BD:</b> TPT - Mapea a la tabla específica 'cleric', vinculada por Id a 'character'.</para>
/// </remarks>
[Table("cleric")]
public class Clerigo : Personaje
{
    /// <summary>
    /// La entidad divina o deidad que otorga los poderes sagrados al clérigo.
    /// </summary>
    /// <remarks>Mapeado a la columna 'deity'. Es fundamental para el trasfondo del personaje.</remarks>
    [Column("deity")]
    public string Divinidad { get; set; } = null!;

    /// <summary>
    /// Capacidad base de restauración de salud que posee el personaje.
    /// </summary>
    /// <value>Valor entero que determina la eficiencia de los hechizos de curación.</value>
    [Column("healing_points")]
    public int PuntosSanacion { get; set; }
}