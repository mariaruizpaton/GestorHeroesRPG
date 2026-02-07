using System.ComponentModel.DataAnnotations.Schema;

namespace GestorHeroesRPG.Model;

/// <summary>
/// Representa la clase combatiente especializada en el combate cuerpo a cuerpo y resistencia física.
/// </summary>
/// <remarks>
/// <para><b>Autor:</b> Liviu</para>
/// <para><b>Versión:</b> 1.0</para>
/// <para><b>Estrategia BD:</b> TPT - Mapea a la tabla específica 'warrior', vinculada por Id a 'character'.</para>
/// </remarks>
[Table("warrior")]
public class Guerrero : Personaje
{
    /// <summary>
    /// El arma principal que el guerrero utiliza en combate (ej. Gran Hacha, Espada Larga).
    /// </summary>
    /// <remarks>Propiedad obligatoria que define el estilo de ataque del personaje.</remarks>
    [Column("main_weapon")]
    public string ArmaPrincipal { get; set; } = null!;

    /// <summary>
    /// Recurso acumulable que el guerrero utiliza para ejecutar habilidades especiales.
    /// </summary>
    /// <value>Valor entero que representa la intensidad del estado de combate.</value>
    [Column("fury")]
    public int Furia { get; set; }
}