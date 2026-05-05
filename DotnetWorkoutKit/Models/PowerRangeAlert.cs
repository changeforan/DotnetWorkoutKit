using System.Text.Json.Serialization;

namespace DotnetWorkoutKit.Models;

public class PowerRangeAlert(double minPower, double maxPower,
    PowerRangeAlert.PowerUnit unit = PowerRangeAlert.PowerUnit.Watts,
    PowerRangeAlert.PowerMetric metric = PowerRangeAlert.PowerMetric.Current) : WorkoutAlert
{
    public double MinPower { get; } = minPower >= 0
        ? minPower
        : throw new ArgumentException("Min power must be greater than or equal to 0.");

    public double MaxPower { get; } = maxPower > minPower
        ? maxPower
        : throw new ArgumentException("Max power must be greater than min power.");

    public PowerUnit Unit { get; } = unit;

    public PowerMetric Metric { get; } = metric;

    [JsonConverter(typeof(JsonStringEnumConverter<PowerUnit>))]
    public enum PowerUnit
    {
        Watts
    }

    [JsonConverter(typeof(JsonStringEnumConverter<PowerMetric>))]
    public enum PowerMetric
    {
        Current,
        Average
    }
}
