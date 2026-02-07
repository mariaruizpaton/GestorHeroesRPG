using System.ComponentModel.DataAnnotations.Schema;

namespace GestorHeroesRPG.Model;

/// <summary>
/// Representa la clase especializada en el uso de artes místicas y control elemental.
/// </summary>
/// <remarks>
/// <para><b>Autor:</b> Liviu</para>
/// <para><b>Versión:</b> 1.0</para>
/// <para><b>Estrategia BD:</b> TPT - Mapea a la tabla específica 'mage', vinculada por Id a 'character'.</para>
/// </remarks>
[Table("mage")]
public class Mago : Personaje
{
    /// <summary>
    /// Reserva de energía mágica disponible para el lanzamiento de hechizos.
    /// </summary>
    /// <value>Valor entero que determina la capacidad de casteo.</value>
    [Column("mana")]
    public int Mana { get; set; }

    /// <summary>
    /// Atributo elemental en el que el mago se especializa (ej. Fuego, Hielo, Rayo).
    /// </summary>
    /// <remarks>Este campo es obligatorio para la lógica de bonificadores elementales.</remarks>
    [Column("main_element")]
    public string ElementoPrincipal { get; set; } = null!;
}