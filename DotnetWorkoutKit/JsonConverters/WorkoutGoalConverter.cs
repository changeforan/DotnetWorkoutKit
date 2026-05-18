using System.Text.Json;
using System.Text.Json.Serialization;
using DotnetWorkoutKit.Models;

namespace DotnetWorkoutKit.JsonConverters;

public class WorkoutGoalConverter : JsonConverter<WorkoutGoal>
{
    public override WorkoutGoal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        if (root.TryGetProperty("Distance", out _))
        {
            return JsonSerializer.Deserialize<DistanceGoal>(root.GetRawText(), options)
                ?? throw new JsonException("Failed to deserialize DistanceGoal.");
        }
        else if (root.TryGetProperty("Time", out _))
        {
            return JsonSerializer.Deserialize<TimeGoal>(root.GetRawText(), options)
                ?? throw new JsonException("Failed to deserialize TimeGoal.");
        }
        else if (root.TryGetProperty("Open", out _))
        {
            return new OpenGoal();
        }
        else
        {
            throw new JsonException("Unknown WorkoutGoal type.");
        }
    }

    public override void Write(Utf8JsonWriter writer, WorkoutGoal value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case DistanceGoal distanceGoal:
                JsonSerializer.Serialize(writer, distanceGoal, options);
                break;
            case TimeGoal timeGoal:
                JsonSerializer.Serialize(writer, timeGoal, options);
                break;
            case OpenGoal:
                writer.WriteStartObject();
                writer.WriteBoolean("Open", true);
                writer.WriteEndObject();
                break;
            default:
                throw new JsonException("Unknown WorkoutGoal type.");
        }
    }
}
