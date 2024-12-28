using System.Text.Json.Serialization;

namespace DotnetWorkoutKit.Models;

public class SpeedRangeAlert : WorkoutAlert
{
    public double MinSpeed { get; }
    public double MaxSpeed { get; }
    public SpeedUnit Unit { get; }
    public AlertMetric Metric { get; }

    [JsonConstructor]
    public SpeedRangeAlert(double minSpeed, double maxSpeed, SpeedUnit unit = SpeedUnit.KilometersPerHour, AlertMetric metric = AlertMetric.Current)
    {
        MinSpeed = minSpeed;
        MaxSpeed = maxSpeed;
        Unit = unit;
        Metric = metric;
        ValidateSpeeds();
    }

    /// <summary>
    /// Create a speed range alert with the given min and max paces.
    ///     A valid pace is in the format mm'ss" where mm is minutes and ss is seconds.
    /// </summary>
    /// <param name="minPace"></param>
    /// <param name="maxPace"></param>
    /// <param name="metric"></param>
    public SpeedRangeAlert(string minPace, string maxPace, AlertMetric metric = AlertMetric.Current)
    {
        MinSpeed = ConvertPaceToSpeed(minPace);
        MaxSpeed = ConvertPaceToSpeed(maxPace);
        Unit = SpeedUnit.MetersPerSecond;
        Metric = metric;
        ValidateSpeeds();
    }

    private static double ConvertPaceToSpeed(string pace)
    {
        // check if pace is in the format mm'ss"
        if (!pace.Contains('\'') || !pace.EndsWith('"'))
        {
            throw new ArgumentException("Pace must be in the format mm'ss\".");
        }

        var paceParts = pace.Split('\'', '"');
        var minutes = double.Parse(paceParts[0]);
        var seconds = double.Parse(paceParts[1]);
        return 1000 / (minutes * 60 + seconds);
    }

    private void ValidateSpeeds()
    {
        if (MinSpeed < 0)
        {
            throw new ArgumentException("Min speed must be greater than or equal to 0.");
        }

        if (MaxSpeed < 0)
        {
            throw new ArgumentException("Max speed must be greater than or equal to 0.");
        }

        if (MinSpeed > MaxSpeed)
        {
            throw new ArgumentException("Min speed must be less than or equal to max speed.");
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter<SpeedUnit>))]
    public enum SpeedUnit
    {
        MetersPerSecond,
        KilometersPerHour,
        MilesPerHour
    }

    [JsonConverter(typeof(JsonStringEnumConverter<AlertMetric>))]
    public enum AlertMetric
    {
        Current,
        Average
    }
}

