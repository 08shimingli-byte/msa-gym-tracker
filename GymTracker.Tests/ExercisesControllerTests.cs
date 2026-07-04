using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymTracker.Controllers;
using GymTracker.Data;
using GymTracker.Models;

namespace GymTracker.Tests;

public class ExercisesControllerTests
{
    private AppDbContext GetDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoExercises()
    {
        var db = GetDb();
        var controller = new ExercisesController(db);

        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var exercises = Assert.IsType<List<Exercise>>(ok.Value);
        Assert.Empty(exercises);
    }

    [Fact]
    public async Task GetAll_ReturnsAllExercises()
    {
        var db = GetDb();
        db.Exercises.AddRange(
            new Exercise { Name = "Bench Press", MuscleGroup = MuscleGroup.Chest },
            new Exercise { Name = "Squat", MuscleGroup = MuscleGroup.Legs }
        );
        await db.SaveChangesAsync();

        var controller = new ExercisesController(db);
        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var exercises = Assert.IsType<List<Exercise>>(ok.Value);
        Assert.Equal(2, exercises.Count);
    }

    [Fact]
    public async Task GetById_ReturnsExercise_WhenExists()
    {
        var db = GetDb();
        db.Exercises.Add(new Exercise { Name = "Bench Press", MuscleGroup = MuscleGroup.Chest });
        await db.SaveChangesAsync();

        var controller = new ExercisesController(db);
        var result = await controller.GetById(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        var exercise = Assert.IsType<Exercise>(ok.Value);
        Assert.Equal("Bench Press", exercise.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var db = GetDb();
        var controller = new ExercisesController(db);

        var result = await controller.GetById(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_AddsExercise()
    {
        var db = GetDb();
        var controller = new ExercisesController(db);
        var exercise = new Exercise { Name = "Deadlift", MuscleGroup = MuscleGroup.Back };

        var result = await controller.Create(exercise);
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(1, await db.Exercises.CountAsync());
    }

    [Fact]
    public async Task Update_UpdatesExercise_WhenExists()
    {
        var db = GetDb();
        db.Exercises.Add(new Exercise { Name = "Bench Press", MuscleGroup = MuscleGroup.Chest });
        await db.SaveChangesAsync();

        var controller = new ExercisesController(db);
        var updated = new Exercise { Name = "Incline Bench Press", MuscleGroup = MuscleGroup.Chest };
        var result = await controller.Update(1, updated);

        var ok = Assert.IsType<OkObjectResult>(result);
        var exercise = Assert.IsType<Exercise>(ok.Value);
        Assert.Equal("Incline Bench Press", exercise.Name);
    }

    [Fact]
    public async Task Delete_RemovesExercise_WhenExists()
    {
        var db = GetDb();
        db.Exercises.Add(new Exercise { Name = "Bench Press", MuscleGroup = MuscleGroup.Chest });
        await db.SaveChangesAsync();

        var controller = new ExercisesController(db);
        var result = await controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(0, await db.Exercises.CountAsync());
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        var db = GetDb();
        var controller = new ExercisesController(db);

        var result = await controller.Delete(999);
        Assert.IsType<NotFoundResult>(result);
    }
}