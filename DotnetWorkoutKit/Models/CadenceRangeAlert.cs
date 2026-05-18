namespace DotnetWorkoutKit.Models;

public class CadenceRangeAlert(int minCadence, int maxCadence) : WorkoutAlert
{
    public int MinCadence { get; } = minCadence > 0
        ? minCadence
        : throw new ArgumentException("Min cadence must be greater than 0.");

    public int MaxCadence { get; } = maxCadence > minCadence
        ? maxCadence
        : throw new ArgumentException("Max cadence must be greater than min cadence.");
}
