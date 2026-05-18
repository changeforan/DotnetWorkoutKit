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

        if (root.TryGetProperty("LowerBound", out _) && root.TryGetProperty("UpperBound", out _) &&
            !root.TryGetProperty("MinSpeed", out _) && !root.TryGetProperty("Unit", out _))
        {
            if (root.TryGetProperty("Metric", out _))
            {
                return JsonSerializer.Deserialize<PowerRangeAlert>(root.GetRawText(), options)
                    ?? throw new JsonException("Failed to deserialize PowerRangeAlert.");
            }
            return JsonSerializer.Deserialize<HeartRateRangeAlert>(root.GetRawText(), options)
                ?? throw new JsonException("Failed to deserialize HeartRateRangeAlert.");
        }
        else if (root.TryGetProperty("MinSpeed", out var minSpeed))
        {
            if (minSpeed.ValueKind == JsonValueKind.String)
            {
                return ManualDeserializeSpeedRangeAlert(root);
            }
            return JsonSerializer.Deserialize<SpeedRangeAlert>(root.GetRawText(), options)
                ?? throw new JsonException("Failed to deserialize SpeedRangeAlert.");
        }
        else if (root.TryGetProperty("MinCadence", out _))
        {
            return JsonSerializer.Deserialize<CadenceRangeAlert>(root.GetRawText(), options)
                ?? throw new JsonException("Failed to deserialize CadenceRangeAlert.");
        }
        else if (root.TryGetProperty("MinPower", out _))
        {
            return JsonSerializer.Deserialize<PowerRangeAlert>(root.GetRawText(), options)
                ?? throw new JsonException("Failed to deserialize PowerRangeAlert.");
        }
        else
        {
            throw new JsonException("Unknown WorkoutAlert type.");
        }
    }

    public override void Write(Utf8JsonWriter writer, WorkoutAlert value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case HeartRateRangeAlert heartRateRangeAlert:
                JsonSerializer.Serialize(writer, heartRateRangeAlert, options);
                break;
            case SpeedRangeAlert speedRangeAlert:
                JsonSerializer.Serialize(writer, speedRangeAlert, options);
                break;
            case CadenceRangeAlert cadenceRangeAlert:
                JsonSerializer.Serialize(writer, cadenceRangeAlert, options);
                break;
            case PowerRangeAlert powerRangeAlert:
                JsonSerializer.Serialize(writer, powerRangeAlert, options);
                break;
            default:
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
