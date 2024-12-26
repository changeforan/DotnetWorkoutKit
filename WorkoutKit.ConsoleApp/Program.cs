using DotnetWorkoutKit.Extensions;
using DotnetWorkoutKit.Models;

var customWorkout = new CustomWorkout(
        CustomWorkout.ActivityType.Running, 
        CustomWorkout.LocationType.Outdoor,
        "E8k Interval",
        new WorkoutStep(new DistanceGoal(2, DistanceGoal.DistanceUnitType.Kilometers), null, "warm up"),
        [
            new IntervalBlock([new (IntervalStep.PurposeType.Work, new (new DistanceGoal(8, DistanceGoal.DistanceUnitType.Kilometers), new HeartRateRangeAlert(144, 153)))], 1),
            new IntervalBlock([
                new (IntervalStep.PurposeType.Work, new (new DistanceGoal(100, DistanceGoal.DistanceUnitType.Meters), new SpeedRangeAlert(11, 12))),
                new (IntervalStep.PurposeType.Recovery, new (new TimeGoal(TimeSpan.FromMinutes(2))))
                ], 8)
        ],
        new WorkoutStep(new DistanceGoal(2, DistanceGoal.DistanceUnitType.Kilometers), new SpeedRangeAlert("5'40\"", "5'20\""), "cool down"));

// Save to file
File.WriteAllBytes("test.workout", customWorkout.DataRepresentation());