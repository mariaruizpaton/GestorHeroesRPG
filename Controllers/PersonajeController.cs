using GestorHeroesRPG.Data;
using GestorHeroesRPG.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorHeroesRPG.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonajeController : ControllerBase
{
    private readonly GameDBContext _context;

    public PersonajeController(GameDBContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetCharacter")]
    public async Task<ActionResult<IEnumerable<Personaje>>> GetPersonajes()
    {
        return await _context.Personajes.ToListAsync();
    }

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

    [HttpPost("guerrero")]
    public async Task<ActionResult<Guerrero>> PostGuerrero(Guerrero guerrero)
    {
        _context.Guerreros.Add(guerrero);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetCharacter", new { id = guerrero.Id }, guerrero);
    }

    [HttpPost("mago")]
    public async Task<ActionResult<Mago>> PostMago(Mago mago)
    {
        _context.Magos.Add(mago);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = mago.Id }, mago);
    }

    [HttpPost("arquero")]
    public async Task<ActionResult<Arquero>> PostArquero(Arquero arquero)
    {
        _context.Arqueros.Add(arquero);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = arquero.Id }, arquero);
    }

    [HttpPost("clerigo")]
    public async Task<ActionResult<Clerigo>> PostClerigo(Clerigo clerigo)
    {
        _context.Clerigos.Add(clerigo);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = clerigo.Id }, clerigo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPersonaje(int id, [FromBody] Personaje personaje)
    {
        // Verificamos que el ID de la URL coincida con el ID del cuerpo
        if (id != personaje.Id)
        {
            return BadRequest("El ID no coincide");
        }

        // Marcamos la entidad como modificada. EF Core detectará el tipo real (Guerrero, etc.)
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

        return NoContent(); // 204: Éxito sin contenido de retorno
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersonaje(int id)
    {
        // Buscamos el personaje en la base de datos
        var personaje = await _context.Personajes.FindAsync(id);

        // Si no existe, devolvemos 404
        if (personaje == null)
        {
            return NotFound($"No se encontró el personaje con ID {id}.");
        }

        // Eliminamos la entidad. EF Core detecta si es Guerrero, Mago, etc.
        _context.Personajes.Remove(personaje);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al eliminar: {ex.Message}");
        }

        return NoContent(); // Respuesta estándar 204 para eliminaciones exitosas
    }
}
