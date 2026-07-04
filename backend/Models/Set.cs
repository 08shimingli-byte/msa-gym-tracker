namespace GymTracker.Models;

public class Set
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    public float WeightKg { get; set; }
    public int Reps { get; set; }
    public int Sets { get; set; }
}