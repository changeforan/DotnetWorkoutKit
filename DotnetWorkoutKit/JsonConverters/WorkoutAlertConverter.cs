using System.Text.Json;
using System.Text.Json.Serialization;
using DotnetWorkoutKit.Models;

namespace DotnetWorkoutKit.JsonConverters;

public class WorkoutAlertConverter : JsonConverter<WorkoutAlert>
{
    public override WorkoutAlert Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        if (root.TryGetProperty("LowerBound", out _))
        {
            return JsonSerializer.Deserialize<HeartRateRangeAlert>(root.GetRawText(), options)
                ?? throw new JsonException("Failed to deserialize HeartRateRangeAlert.");
        }
        else if (root.TryGetProperty("MinSpeed", out var minSpeed))
        {
            if (minSpeed.ValueKind == JsonValueKind.String)
            {
                return ManualDeserializeSpeedRangeAlert(root);
            }
            else
            {
                return JsonSerializer.Deserialize<SpeedRangeAlert>(root.GetRawText(), options)
                    ?? throw new JsonException("Failed to deserialize SpeedRangeAlert.");
            }
        }
        else
        {
            throw new JsonException("Unknown WorkoutAlert type.");
        }
    }

    public override void Write(Utf8JsonWriter writer, WorkoutAlert value, JsonSerializerOptions options)
    {
        if (value is HeartRateRangeAlert heartRateRangeAlert)
        {
            JsonSerializer.Serialize(writer, heartRateRangeAlert, options);
        }
        else if (value is SpeedRangeAlert speedRangeAlert)
        {
            JsonSerializer.Serialize(writer, speedRangeAlert, options);
        }
        else
        {
            throw new JsonException("Unknown WorkoutAlert type.");
        }
    }

    private static SpeedRangeAlert ManualDeserializeSpeedRangeAlert(JsonElement root)
    {
        var minSpeed = root.GetProperty("MinSpeed").GetString() ?? throw new JsonException("MinSpeed is required.");
        var maxSpeed = root.GetProperty("MaxSpeed").GetString() ?? throw new JsonException("MaxSpeed is required.");
        root.TryGetProperty("Metric", out var jsonElement);
        var speedUnit = jsonElement.ValueKind == JsonValueKind.Undefined ? "Current" : jsonElement.GetString();
        var metric = Enum.TryParse<SpeedRangeAlert.AlertMetric>(speedUnit, out var alertMetric)
            ? alertMetric
            : throw new JsonException("Speed metric must be current or average.");
        return new SpeedRangeAlert(minSpeed, maxSpeed, metric);
    }
}