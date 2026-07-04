using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymTracker.Data;
using GymTracker.Models;

namespace GymTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SetsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await db.Sets.Include(s => s.Exercise).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var set = await db.Sets.Include(s => s.Exercise).FirstOrDefaultAsync(s => s.Id == id);
        return set is null ? NotFound() : Ok(set);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Set set)
    {
        db.Sets.Add(set);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = set.Id }, set);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Set updated)
    {
        var set = await db.Sets.FindAsync(id);
        if (set is null) return NotFound();

        var exerciseExists = await db.Exercises.AnyAsync(e => e.Id == updated.ExerciseId);
        if (!exerciseExists) return BadRequest("Exercise does not exist.");

        set.WeightKg = updated.WeightKg;
        set.Reps = updated.Reps;
        set.Sets = updated.Sets;
        set.ExerciseId = updated.ExerciseId;
        await db.SaveChangesAsync();
        return Ok(set);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var set = await db.Sets.FindAsync(id);
        if (set is null) return NotFound();
        db.Sets.Remove(set);
        await db.SaveChangesAsync();
        return NoContent();
    }
}