namespace DotnetWorkoutKit.Models;

public class IntervalStep(IntervalStep.PurposeType purpose, WorkoutStep workoutStep)
{

    public PurposeType Purpose { get; } = purpose;

    public WorkoutStep WorkoutStep { get; } = workoutStep;
    
    public enum PurposeType
    {
        Work,
        Recovery
    }
}
