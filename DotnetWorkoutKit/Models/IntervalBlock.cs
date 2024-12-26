namespace DotnetWorkoutKit.Models;

public class IntervalBlock(IntervalStep[] intervalSteps, int iterations)
{
    public IntervalStep[] IntervalSteps { get; } = ValidateIntervalSteps(intervalSteps) 
        ? intervalSteps
        : throw new ArgumentException("Interval steps must not be empty.");

    public int Iterations { get; } = ValidateIterations(iterations)
        ? iterations
        : throw new ArgumentException("Iterations must be greater than 0.");

    private static bool ValidateIntervalSteps(IntervalStep[] intervalSteps)
    {
        return intervalSteps.Length > 0;
    }

    private static bool ValidateIterations(int iterations)
    {
        return iterations > 0;
    }
}