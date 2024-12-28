using System.Text.Json.Serialization;
using static DotnetWorkoutKit.Models.CustomWorkout;

namespace DotnetWorkoutKit.Models;

public class CustomWorkout(ActivityType activity, LocationType location,
    string? displayName, WorkoutStep? warmUp, IntervalBlock[] blocks, WorkoutStep? coolDown)
{

    public ActivityType Activity { get; } = ValidateActivityType(activity)
        ? activity
        : throw new ArgumentException("Only running is supported now.");
    
    public LocationType Location { get; } = location;

    public string? DisplayName { get; } = displayName;

    public WorkoutStep? WarmUp { get; } = warmUp;

    public IntervalBlock[] Blocks { get; } = blocks;

    public WorkoutStep? CoolDown { get; } = coolDown;

    [JsonConverter(typeof(JsonStringEnumConverter<ActivityType>))]
    public enum ActivityType
    {
        Running,
        Cycling,
        Swimming
    }

    [JsonConverter(typeof(JsonStringEnumConverter<LocationType>))]
    public enum LocationType
    {
        Indoor,
        Outdoor
    }

    private static bool ValidateActivityType(ActivityType activityType)
    {
        return activityType == ActivityType.Running;
    }
}