using DotnetWorkoutKit.Extensions;
using DotnetWorkoutKit.Models;

var customWorkout = new CustomWorkout(
        activity: CustomWorkout.ActivityType.Running, 
        location: CustomWorkout.LocationType.Outdoor,
        displayName: "sample",
        warmUp: new WorkoutStep(new DistanceGoal(3, DistanceGoal.DistanceUnit.Kilometers), new HeartRateRangeAlert(144, 153), "Warm Up"),
        blocks: [
            new IntervalBlock([
                new (IntervalStep.PurposeType.Work, new (new DistanceGoal(3, DistanceGoal.DistanceUnit.Kilometers), new SpeedRangeAlert("4'46\"", "4'38\""))),
                new (IntervalStep.PurposeType.Recovery, new (new TimeGoal(TimeSpan.FromMinutes(2))))
                ], 2),
            new IntervalBlock([
                new (IntervalStep.PurposeType.Work, new (new DistanceGoal(200, DistanceGoal.DistanceUnit.Meters), new SpeedRangeAlert("4'09\"", "3'59\""))),
                new (IntervalStep.PurposeType.Recovery, new (new DistanceGoal(200, DistanceGoal.DistanceUnit.Meters)))
                ], 6)
        ],
        coolDown: new WorkoutStep(new DistanceGoal(3, DistanceGoal.DistanceUnit.Kilometers), new HeartRateRangeAlert(144, 153), "Cool Down"));

// Save as JSON
File.WriteAllText($"{customWorkout.DisplayName}.workout.json", customWorkout.JsonRepresentation());

// Save as binary
File.WriteAllBytes($"{customWorkout.DisplayName}.workout", customWorkout.DataRepresentation());

// Load from JSON
var _ = File.ReadAllText($"{customWorkout.DisplayName}.workout.json").LoadFromJson();

Console.WriteLine("Sample workout created and saved as JSON and binary files.");
