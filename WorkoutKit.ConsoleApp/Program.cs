using DotnetWorkoutKit.Extensions;
using DotnetWorkoutKit.Models;

void Compare(CustomWorkout workout, string name, string appleFile)
{
    var bin = workout.DataRepresentation();
    var appleBin = File.ReadAllBytes($"/Users/change/src/tmp/workoutbin/{appleFile}.workout");
    var match = bin.Length > 38 && appleBin.Length > 38 &&
                bin[38..].SequenceEqual(appleBin[38..]);
    Console.WriteLine($"{name}: dotnet={bin.Length}b apple={appleBin.Length}b match={match}");
}

Compare(
    new CustomWorkout(CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor, "spd_ms_cur", null,
        [new IntervalBlock([
            new(IntervalStep.PurposeType.Work, new(new DistanceGoal(1, DistanceGoal.DistanceUnit.Kilometers),
                new SpeedRangeAlert(3.5, 4.5, SpeedRangeAlert.SpeedUnit.MetersPerSecond, SpeedRangeAlert.AlertMetric.Current)))
        ], 1)], null),
    "spd_ms_cur", "spd_01_ms_current");

Compare(
    new CustomWorkout(CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor, "spd_ms_avg", null,
        [new IntervalBlock([
            new(IntervalStep.PurposeType.Work, new(new DistanceGoal(1, DistanceGoal.DistanceUnit.Kilometers),
                new SpeedRangeAlert(3.5, 4.5, SpeedRangeAlert.SpeedUnit.MetersPerSecond, SpeedRangeAlert.AlertMetric.Average)))
        ], 1)], null),
    "spd_ms_avg", "spd_02_ms_average");

Compare(
    new CustomWorkout(CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor, "cadence_ref", null,
        [new IntervalBlock([
            new(IntervalStep.PurposeType.Work, new(new DistanceGoal(1, DistanceGoal.DistanceUnit.Kilometers),
                new CadenceRangeAlert(170, 185)))
        ], 1)], null),
    "cadence_ref", "spd_07_cadence_ref");
