using static DotnetWorkoutKit.Models.CustomWorkout;

namespace DotnetWorkoutKit.Models;

public class CustomWorkout(ActivityType activityType, LocationType locationType,
    string? displayName, WorkoutStep? warmUp, IntervalBlock[] blocks, WorkoutStep? coolDown)
{

    public ActivityType Activity { get; } = ValidateActivityType(activityType)
        ? activityType
        : throw new ArgumentException("Only running is supported now.");
    
    public LocationType Location { get; } = locationType;

    public string? DisplayName { get; } = displayName;

    public WorkoutStep? WarmUp { get; } = warmUp;

    public IntervalBlock[] Blocks { get; } = blocks;

    public WorkoutStep? CoolDown { get; } = coolDown;

    public enum ActivityType
    {
        Running,
        Cycling,
        Swimming
    }

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