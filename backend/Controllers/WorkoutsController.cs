using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymTracker.Data;
using GymTracker.Models;

namespace GymTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await db.Workouts
            .Include(w => w.Sets)
            .ThenInclude(s => s.Exercise)
            .ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var workout = await db.Workouts
            .Include(w => w.Sets)
            .ThenInclude(s => s.Exercise)
            .FirstOrDefaultAsync(w => w.Id == id);
        return workout is null ? NotFound() : Ok(workout);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Workout workout)
    {
        db.Workouts.Add(workout);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = workout.Id }, workout);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Workout updated)
    {
        var workout = await db.Workouts.FindAsync(id);
        if (workout is null) return NotFound();

        workout.Name = updated.Name;
        workout.Date = updated.Date;
        await db.SaveChangesAsync();
        return Ok(workout);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var workout = await db.Workouts.FindAsync(id);
        if (workout is null) return NotFound();
        db.Workouts.Remove(workout);
        await db.SaveChangesAsync();
        return NoContent();
    }
}