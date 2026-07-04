using Microsoft.EntityFrameworkCore;
using GymTracker.Models;

namespace GymTracker.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Set> Sets => Set<Set>();
}