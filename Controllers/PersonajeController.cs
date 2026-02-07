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
[Route("api/[controller]")]
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
    /// Crea un nuevo personaje de clase Guerrero.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPost("guerrero")]
    public async Task<ActionResult<Guerrero>> PostGuerrero(Guerrero guerrero)
    {
        _context.Guerreros.Add(guerrero);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetCharacter", new { id = guerrero.Id }, guerrero);
    }

    /// <summary>
    /// Crea un nuevo personaje de clase Mago.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPost("mago")]
    public async Task<ActionResult<Mago>> PostMago(Mago mago)
    {
        _context.Magos.Add(mago);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = mago.Id }, mago);
    }

    /// <summary>
    /// Crea un nuevo personaje de clase Arquero.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPost("arquero")]
    public async Task<ActionResult<Arquero>> PostArquero(Arquero arquero)
    {
        _context.Arqueros.Add(arquero);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = arquero.Id }, arquero);
    }

    /// <summary>
    /// Crea un nuevo personaje de clase Clérigo.
    /// </summary>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPost("clerigo")]
    public async Task<ActionResult<Clerigo>> PostClerigo(Clerigo clerigo)
    {
        _context.Clerigos.Add(clerigo);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = clerigo.Id }, clerigo);
    }

    /// <summary>
    /// Actualiza los datos de un personaje existente.
    /// </summary>
    /// <param name="id">ID del personaje a modificar.</param>
    /// <param name="personaje">Datos actualizados.</param>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPersonaje(int id, [FromBody] Personaje personaje)
    {
        if (id != personaje.Id)
        {
            return BadRequest("El ID no coincide");
        }

        _context.Entry(personaje).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Personajes.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    /// <summary>
    /// Elimina un personaje de la base de datos.
    /// </summary>
    /// <param name="id">ID del personaje a eliminar.</param>
    /// <remarks>
    /// <para><b>Autor:</b> Maria</para>
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersonaje(int id)
    {
        var personaje = await _context.Personajes.FindAsync(id);

        if (personaje == null)
        {
            return NotFound($"No se encontró el personaje con ID {id}.");
        }

        _context.Personajes.Remove(personaje);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al eliminar: {ex.Message}");
        }

        return NoContent();
    }
}