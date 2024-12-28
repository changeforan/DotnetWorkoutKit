using System.Text.Json.Serialization;
using DotnetWorkoutKit.JsonConverters;

namespace DotnetWorkoutKit.Models;

[JsonConverter(typeof(WorkoutAlertConverter))]
public abstract class WorkoutAlert
{
}
