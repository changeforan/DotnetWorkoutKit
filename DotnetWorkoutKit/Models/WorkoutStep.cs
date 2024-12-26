namespace DotnetWorkoutKit.Models;

public class WorkoutStep(WorkoutGoal goal, WorkoutAlert? alert = null, string? displayName = null)
{
    public WorkoutGoal Goal { get; } = goal;

    public WorkoutAlert? Alert { get; } = alert;

    public string? DisplayName { get; } = displayName;
}