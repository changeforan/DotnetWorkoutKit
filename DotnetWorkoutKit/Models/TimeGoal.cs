namespace DotnetWorkoutKit.Models;

public class TimeGoal(TimeSpan time) : WorkoutGoal
{
    public TimeSpan Time { get; } = time;
}