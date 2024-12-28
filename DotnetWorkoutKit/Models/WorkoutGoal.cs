using System.Text.Json.Serialization;
using DotnetWorkoutKit.JsonConverters;

namespace DotnetWorkoutKit.Models;

[JsonConverter(typeof(WorkoutGoalConverter))]
public abstract class WorkoutGoal
{
}
