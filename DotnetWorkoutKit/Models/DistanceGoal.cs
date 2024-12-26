using static DotnetWorkoutKit.Models.DistanceGoal;

namespace DotnetWorkoutKit.Models;

public class DistanceGoal(double distance, DistanceUnitType distanceUnitType) : WorkoutGoal
{
    public double Distance { get; } = distance;

    public DistanceUnitType UnitType { get; } = distanceUnitType;

    public enum DistanceUnitType
    {
        Meters,
        Kilometers,
        Miles
    }
}