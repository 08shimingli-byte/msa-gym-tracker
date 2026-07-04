namespace GymTracker.Models;

public class Workout
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Set> Sets { get; set; } = [];
    public DateTime Date { get; set; }
}