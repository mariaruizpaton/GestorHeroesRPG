using GestorHeroesRPG.Data;
using GestorHeroesRPG.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorHeroesRPG.Controllers;

/// <summary>
/// Controlador principal para la gestión de personajes y sus clases derivadas.
/// </summary>
/// <remarks>
/// <b>Autor:</b> Maria
/// </remarks>
[ApiController]
[Route("[controller]")]
public class PersonajeController : ControllerBase
{
    private readonly GameDBContext _context;

    public PersonajeController(GameDBContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Agrupación Polimórfica: Resumen de cantidad de personajes por tipo y sus medias de nivel.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Liviu</para>
    /// </remarks>
    [HttpGet("resumen-tipo")]
    public async Task<IActionResult> GetResumenPorTipo()
    {
        var resumen = await _context.Personajes
            .Select(p => new
            {
                TipoLabel = p is Guerrero ? "Guerrero" :
                            p is Mago ? "Mago" :
                            p is Arquero ? "Arquero" :
                            p is Clerigo ? "Clerigo" : "Personaje",
                p.Nivel
            })
            .GroupBy(x => x.TipoLabel)
            .Select(g => new
            {
                Tipo = g.Key,
                Cantidad = g.Count(),
                MediaNivel = g.Average(x => x.Nivel)
            })
            .ToListAsync();

        return Ok(resumen);
    }

    /// <summary>
    /// Búsqueda Multitabla: Obtener personajes de tipo Clerigo O Mago con nivel > 50 ordenados por fecha.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Liviu</para>
    /// </remarks>
    [HttpGet("filtrado-especial")]
    public async Task<ActionResult<IEnumerable<Personaje>>> GetEspecialistasAltos()
    {
        // EF Core realiza los JOINs automáticos con las tablas 'cleric' y 'mage'
        var personajes = await _context.Personajes
            .Where(p => (p is Clerigo || p is Mago) && p.Nivel > 50)
            .OrderBy(p => p.FechaCreation)
            .ToListAsync();

        return Ok(personajes);
    }

    /// <summary>
    /// Obtiene la lista completa de personajes (polimórfica).
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpGet(Name = "GetCharacter")]
    public async Task<ActionResult<IEnumerable<Personaje>>> GetPersonajes()
    {
        return await _context.Personajes.ToListAsync();
    }

    /// <summary>
    /// Busca un personaje específico por su identificador único.
    /// </summary>
    /// <param name="id">ID del personaje.</param>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpGet("{id}")]
    public async Task<ActionResult<Personaje>> GetPersonajeId(int id)
    {
        var personaje = await _context.Personajes.FindAsync(id);
        if (personaje == null)
        {
            return NotFound($"No se encontró el personaje con ID {id}.");
        }
        return personaje;
    }

    /// <summary>
    /// Endpoint global para crear cualquier tipo de personaje mediante deserialización polimórfica.
    /// </summary>
    /// <param name="personaje">Objeto personaje (Guerrero, Mago, etc.).</param>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPost]
    public async Task<ActionResult<Personaje>> PostPersonaje(Personaje personaje)
    {
        // Usamos el método privado para mantener la coherencia en toda la API
        if (await NombreExiste(personaje.Nombre))
        {
            return BadRequest($"El nombre '{personaje.Nombre}' ya está en uso por otro héroe.");
        }

        _context.Personajes.Add(personaje);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al crear el personaje: {ex.Message}");
        }

        return CreatedAtAction(nameof(GetPersonajeId), new { id = personaje.Id }, personaje);
    }

    /// <summary>
    /// Crea un nuevo personaje de clase Guerrero con validación de nombre único.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPost("guerrero")]
    public async Task<ActionResult<Guerrero>> PostGuerrero(Guerrero guerrero)
    {
        if (await NombreExiste(guerrero.Nombre))
        {
            return BadRequest($"El nombre '{guerrero.Nombre}' ya está en uso por otro mago o personaje.");
        }

        _context.Guerreros.Add(guerrero);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetCharacter", new { id = guerrero.Id }, guerrero);
    }

    /// <summary>
    /// Crea un nuevo personaje de clase Mago con validación de nombre único.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPost("mago")]
    public async Task<ActionResult<Mago>> PostMago(Mago mago)
    {
        if (await NombreExiste(mago.Nombre))
        {
            return BadRequest($"El nombre '{mago.Nombre}' ya está en uso por otro mago o personaje.");
        }

        _context.Magos.Add(mago);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = mago.Id }, mago);
    }

    /// <summary>
    /// Crea un nuevo personaje de clase Arquero con validación de nombre único.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPost("arquero")]
    public async Task<ActionResult<Arquero>> PostArquero(Arquero arquero)
    {
        if (await NombreExiste(arquero.Nombre))
        {
            return BadRequest($"El nombre '{arquero.Nombre}' ya está en uso por otro arquero o personaje.");
        }

        _context.Arqueros.Add(arquero);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = arquero.Id }, arquero);
    }

    /// <summary>
    /// Crea un nuevo personaje de clase Clérigo con validación de nombre único.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPost("clerigo")]
    public async Task<ActionResult<Clerigo>> PostClerigo(Clerigo clerigo)
    {
        if (await NombreExiste(clerigo.Nombre))
        {
            return BadRequest($"El nombre '{clerigo.Nombre}' ya está en uso por otro clérigo o personaje.");
        }

        _context.Clerigos.Add(clerigo);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = clerigo.Id }, clerigo);
    }

    /// <summary>
    /// Actualiza los datos de un personaje existente utilizando una transacción atómica.
    /// </summary>
    /// <param name="id">ID del personaje a modificar.</param>
    /// <param name="personaje">Objeto con los datos actualizados.</param>
    /// <returns>NoContent si tiene éxito, NotFound si el ID no existe o BadRequest si los IDs no coinciden.</returns>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// Se implementa un bloque de transacción para asegurar que, ante cualquier fallo en la base de datos, 
    /// se realice un rollback y no queden datos inconsistentes.
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPersonaje(int id, [FromBody] Personaje personaje)
    {
        if (id != personaje.Id)
        {
            return BadRequest("El ID no coincide con el registro que intentas actualizar.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var existe = await _context.Personajes.AnyAsync(p => p.Id == id);
            if (!existe)
            {
                return NotFound($"No se puede actualizar: El personaje con ID {id} no existe.");
            }

            _context.Entry(personaje).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Error interno al actualizar: {ex.Message}. Se ha revertido la operación.");
        }
    }

    /// <summary>
    /// Elimina un personaje de la base de datos de forma segura.
    /// </summary>
    /// <param name="id">ID del personaje a eliminar.</param>
    /// <returns>NoContent si se elimina, NotFound si no existe.</returns>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// El método incluye una verificación de existencia y un Rollback automático 
    /// si la eliminación falla por restricciones de integridad.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersonaje(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var personaje = await _context.Personajes.FindAsync(id);

            if (personaje == null)
            {
                return NotFound($"No se encontró el personaje con ID {id}. Operación cancelada.");
            }

            _context.Personajes.Remove(personaje);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest($"Error crítico al eliminar: {ex.Message}. La base de datos ha revertido los cambios.");
        }
    }

    /// <summary>
    /// Comprueba si un nombre ya existe en la base de datos (sin importar la clase del personaje).
    /// </summary>
    /// <remarks>
    /// <b>Autor:</b> Maria
    /// </remarks>
    private async Task<bool> NombreExiste(string nombre)
    {
        return await _context.Personajes.AnyAsync(p => p.Nombre.ToLower() == nombre.ToLower());
    }
}