using System.Text.Json.Serialization;

namespace DotnetWorkoutKit.Models;

public class CustomWorkout(CustomWorkout.ActivityType activity, CustomWorkout.LocationType location,
    string? displayName, WorkoutStep? warmUp, IntervalBlock[] blocks, WorkoutStep? coolDown)
{

    public ActivityType Activity { get; } = activity;
    
    public LocationType Location { get; } = location;

    public string? DisplayName { get; } = displayName;

    public WorkoutStep? WarmUp { get; } = warmUp;

    public IntervalBlock[] Blocks { get; } = blocks;

    public WorkoutStep? CoolDown { get; } = coolDown;

    [JsonConverter(typeof(JsonStringEnumConverter<ActivityType>))]
    public enum ActivityType
    {
        CrossTraining,
        Cycling,
        Elliptical,
        FunctionalStrengthTraining,
        Hiking,
        Rowing,
        Running,
        StairClimbing,
        Swimming,
        TraditionalStrengthTraining,
        Walking,
        Yoga,
        CoreTraining,
        HighIntensityIntervalTraining
    }

    [JsonConverter(typeof(JsonStringEnumConverter<LocationType>))]
    public enum LocationType
    {
        Indoor,
        Outdoor
    }
}
