namespace DotnetWorkoutKit.Models;

public class HeartRateRangeAlert(int lowerBound, int upperBound) : WorkoutAlert
{
    public int LowerBound { get; } = ValidateLowerBound(lowerBound)
        ? lowerBound
        : throw new ArgumentException("Lower bound must be greater than 0.");
    
    public int UpperBound { get; } = ValidateUpperBound(upperBound, lowerBound)
        ? upperBound
        : throw new ArgumentException("Upper bound must be greater than lower bound.");

    private static bool ValidateLowerBound(int lowerBound)
    {
        return lowerBound > 0;
    }

    private static bool ValidateUpperBound(int upperBound, int lowerBound)
    {
        return upperBound > lowerBound;
    }
}
