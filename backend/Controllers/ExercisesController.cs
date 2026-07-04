using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymTracker.Data;
using GymTracker.Models;

namespace GymTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await db.Exercises.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var exercise = await db.Exercises.FindAsync(id);
        return exercise is null ? NotFound() : Ok(exercise);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Exercise exercise)
    {
        db.Exercises.Add(exercise);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = exercise.Id }, exercise);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Exercise updated)
    {
        var exercise = await db.Exercises.FindAsync(id);
        if (exercise is null) return NotFound();

        exercise.Name = updated.Name;
        exercise.MuscleGroup = updated.MuscleGroup;
        await db.SaveChangesAsync();
        return Ok(exercise);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var exercise = await db.Exercises.FindAsync(id);
        if (exercise is null) return NotFound();
        db.Exercises.Remove(exercise);
        await db.SaveChangesAsync();
        return NoContent();
    }
}