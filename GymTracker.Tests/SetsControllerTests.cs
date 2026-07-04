using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymTracker.Controllers;
using GymTracker.Data;
using GymTracker.Models;

namespace GymTracker.Tests;

public class SetsControllerTests
{
    private AppDbContext GetDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private async Task<AppDbContext> GetSeededDb()
    {
        var db = GetDb();
        db.Exercises.Add(new Exercise { Name = "Bench Press", MuscleGroup = MuscleGroup.Chest });
        await db.SaveChangesAsync();
        return db;
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoSets()
    {
        var db = GetDb();
        var controller = new SetsController(db);

        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var sets = Assert.IsType<List<Set>>(ok.Value);
        Assert.Empty(sets);
    }

    [Fact]
    public async Task Create_AddsSet()
    {
        var db = await GetSeededDb();
        var controller = new SetsController(db);
        var set = new Set { ExerciseId = 1, WeightKg = 60, Reps = 12, Sets = 3 };

        var result = await controller.Create(set);
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(1, await db.Sets.CountAsync());
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenExerciseNotExists()
    {
        var db = await GetSeededDb();
        var controller = new SetsController(db);
        var set = new Set { ExerciseId = 999, WeightKg = 60, Reps = 12, Sets = 3 };

        var result = await controller.Create(set);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetById_ReturnsSet_WhenExists()
    {
        var db = await GetSeededDb();
        db.Sets.Add(new Set { ExerciseId = 1, WeightKg = 60, Reps = 12, Sets = 3 });
        await db.SaveChangesAsync();

        var controller = new SetsController(db);
        var result = await controller.GetById(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<Set>(ok.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var db = GetDb();
        var controller = new SetsController(db);

        var result = await controller.GetById(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_UpdatesSet_WhenExists()
    {
        var db = await GetSeededDb();
        db.Sets.Add(new Set { ExerciseId = 1, WeightKg = 60, Reps = 12, Sets = 3 });
        await db.SaveChangesAsync();

        var controller = new SetsController(db);
        var updated = new Set { ExerciseId = 1, WeightKg = 80, Reps = 8, Sets = 3 };
        var result = await controller.Update(1, updated);

        var ok = Assert.IsType<OkObjectResult>(result);
        var set = Assert.IsType<Set>(ok.Value);
        Assert.Equal(80, set.WeightKg);
    }

    [Fact]
    public async Task Delete_RemovesSet_WhenExists()
    {
        var db = await GetSeededDb();
        db.Sets.Add(new Set { ExerciseId = 1, WeightKg = 60, Reps = 12, Sets = 3 });
        await db.SaveChangesAsync();

        var controller = new SetsController(db);
        var result = await controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(0, await db.Sets.CountAsync());
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        var db = GetDb();
        var controller = new SetsController(db);

        var result = await controller.Delete(999);
        Assert.IsType<NotFoundResult>(result);
    }
}