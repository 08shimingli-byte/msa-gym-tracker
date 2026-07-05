using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymTracker.Controllers;
using GymTracker.Data;
using GymTracker.Models;

namespace GymTracker.Tests;

public class WorkoutsControllerTests
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
        db.Workouts.Add(new Workout { Name = "Push Day", Date = DateTime.Now });
        await db.SaveChangesAsync();
        db.Sets.Add(new Set { ExerciseId = 1, WorkoutId = 1, WeightKg = 60, Reps = 12, Sets = 3 });
        await db.SaveChangesAsync();
        return db;
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoWorkouts()
    {
        var db = GetDb();
        var controller = new WorkoutsController(db);

        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var workouts = Assert.IsType<List<Workout>>(ok.Value);
        Assert.Empty(workouts);
    }

    [Fact]
    public async Task GetAll_ReturnsAllWorkouts()
    {
        var db = GetDb();
        db.Workouts.AddRange(
            new Workout { Name = "Push Day", Date = DateTime.Now },
            new Workout { Name = "Pull Day", Date = DateTime.Now }
        );
        await db.SaveChangesAsync();

        var controller = new WorkoutsController(db);
        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var workouts = Assert.IsType<List<Workout>>(ok.Value);
        Assert.Equal(2, workouts.Count);
    }

    [Fact]
    public async Task GetAll_IncludesSetsWithExercises()
    {
        var db = await GetSeededDb();
        var controller = new WorkoutsController(db);

        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var workouts = Assert.IsType<List<Workout>>(ok.Value);

        Assert.Single(workouts);
        Assert.Single(workouts[0].Sets);
        Assert.NotNull(workouts[0].Sets[0].Exercise);
        Assert.Equal("Bench Press", workouts[0].Sets[0].Exercise!.Name);
    }

    [Fact]
    public async Task GetById_ReturnsWorkout_WhenExists()
    {
        var db = GetDb();
        db.Workouts.Add(new Workout { Name = "Push Day", Date = DateTime.Now });
        await db.SaveChangesAsync();

        var controller = new WorkoutsController(db);
        var result = await controller.GetById(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        var workout = Assert.IsType<Workout>(ok.Value);
        Assert.Equal("Push Day", workout.Name);
    }

    [Fact]
    public async Task GetById_IncludesSetsWithExercises()
    {
        var db = await GetSeededDb();
        var controller = new WorkoutsController(db);

        var result = await controller.GetById(1);
        var ok = Assert.IsType<OkObjectResult>(result);
        var workout = Assert.IsType<Workout>(ok.Value);

        Assert.Single(workout.Sets);
        Assert.NotNull(workout.Sets[0].Exercise);
        Assert.Equal("Bench Press", workout.Sets[0].Exercise!.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var db = GetDb();
        var controller = new WorkoutsController(db);

        var result = await controller.GetById(999);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_AddsWorkout()
    {
        var db = GetDb();
        var controller = new WorkoutsController(db);
        var workout = new Workout { Name = "Leg Day", Date = DateTime.Now };

        var result = await controller.Create(workout);
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(1, await db.Workouts.CountAsync());
    }

    [Fact]
    public async Task Update_UpdatesWorkout_WhenExists()
    {
        var db = GetDb();
        db.Workouts.Add(new Workout { Name = "Push Day", Date = DateTime.Now });
        await db.SaveChangesAsync();

        var controller = new WorkoutsController(db);
        var updated = new Workout { Name = "Updated Push Day", Date = DateTime.Now };
        var result = await controller.Update(1, updated);

        Assert.IsType<OkObjectResult>(result);
        var workout = await db.Workouts.FindAsync(1);
        Assert.Equal("Updated Push Day", workout!.Name);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenNotExists()
    {
        var db = GetDb();
        var controller = new WorkoutsController(db);
        var updated = new Workout { Name = "Updated Push Day", Date = DateTime.Now };

        var result = await controller.Update(999, updated);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_RemovesWorkout_WhenExists()
    {
        var db = GetDb();
        db.Workouts.Add(new Workout { Name = "Push Day", Date = DateTime.Now });
        await db.SaveChangesAsync();

        var controller = new WorkoutsController(db);
        var result = await controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(0, await db.Workouts.CountAsync());
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        var db = GetDb();
        var controller = new WorkoutsController(db);

        var result = await controller.Delete(999);
        Assert.IsType<NotFoundResult>(result);
    }
}