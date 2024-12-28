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
        else
        {
            throw new JsonException("Unknown WorkoutGoal type.");
        }
    }

    public override void Write(Utf8JsonWriter writer, WorkoutGoal value, JsonSerializerOptions options)
    {
        if (value is DistanceGoal distanceGoal)
        {
            JsonSerializer.Serialize(writer, distanceGoal, options);
        }
        else if (value is TimeGoal timeGoal)
        {
            JsonSerializer.Serialize(writer, timeGoal, options);
        }
        else
        {
            throw new JsonException("Unknown WorkoutGoal type.");
        }
    }
}
