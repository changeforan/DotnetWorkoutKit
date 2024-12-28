using System.Text.Json.Serialization;

namespace DotnetWorkoutKit.Models;

public class DistanceGoal(double distance, DistanceGoal.DistanceUnit unit) : WorkoutGoal
{
    public double Distance { get; set; } = distance;

    public DistanceUnit Unit { get; set; } = unit;

    [JsonConverter(typeof(JsonStringEnumConverter<DistanceUnit>))]
    public enum DistanceUnit
    {
        Meters,
        Kilometers,
        Miles
    }
}