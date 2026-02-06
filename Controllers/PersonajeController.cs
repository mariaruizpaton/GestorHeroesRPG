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

    [HttpPost("guerrero")]
    public async Task<ActionResult<Guerrero>> PostGuerrero(Guerrero guerrero)
    {
        // EF Core se encarga de insertar en 'character' y 'warrior'
        _context.Guerreros.Add(guerrero);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = guerrero.Id }, guerrero);
    }

    [HttpPost("mago")]
    public async Task<ActionResult<Mago>> PostMago(Mago mago)
    {
        _context.Magos.Add(mago);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetPersonajes", new { id = mago.Id }, mago);
    }
}
