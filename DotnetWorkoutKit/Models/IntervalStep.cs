using System.Text.Json.Serialization;

namespace DotnetWorkoutKit.Models;

public class IntervalStep(IntervalStep.PurposeType purpose, WorkoutStep workoutStep)
{

    public PurposeType Purpose { get; } = purpose;

    public WorkoutStep WorkoutStep { get; } = workoutStep;

    [JsonConverter(typeof(JsonStringEnumConverter<PurposeType>))]    
    public enum PurposeType
    {
        Work,
        Recovery
    }
}
