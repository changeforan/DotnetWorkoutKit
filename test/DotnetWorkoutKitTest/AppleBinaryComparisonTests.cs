using DotnetWorkoutKit.Extensions;
using DotnetWorkoutKit.Models;

namespace DotnetWorkoutKitTest;

public class AppleBinaryComparisonTests
{
    private static readonly string? AppleWorkoutDir = Environment.GetEnvironmentVariable("APPLE_WORKOUT_DIR");

    private static void AssertAppleMatch(string filename, CustomWorkout workout)
    {
        if (AppleWorkoutDir == null)
        {
            Assert.Skip("APPLE_WORKOUT_DIR not set");
            return;
        }

        var path = Path.Combine(AppleWorkoutDir, filename + ".workout");
        if (!File.Exists(path))
        {
            Assert.Skip($"File not found: {path}");
            return;
        }

        var expected = File.ReadAllBytes(path);
        var actual = workout.DataRepresentation();
        Assert.Equal(expected[..2], actual[..2]);
        Assert.Equal(expected[38..], actual[38..]);
    }

    private static CustomWorkout SimpleWorkout(
        CustomWorkout.ActivityType activity, CustomWorkout.LocationType location,
        string displayName, WorkoutGoal? goal = null, WorkoutAlert? alert = null)
    {
        goal ??= new TimeGoal(TimeSpan.FromMinutes(10));
        return new CustomWorkout(activity, location, displayName, null,
            [new IntervalBlock([new(IntervalStep.PurposeType.Work, new(goal, alert))], 1)], null);
    }

    [Theory]
    [InlineData("activity_running", CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor, "running")]
    [InlineData("activity_cycling", CustomWorkout.ActivityType.Cycling, CustomWorkout.LocationType.Outdoor, "cycling")]
    [InlineData("activity_walking", CustomWorkout.ActivityType.Walking, CustomWorkout.LocationType.Outdoor, "walking")]
    [InlineData("activity_hiking", CustomWorkout.ActivityType.Hiking, CustomWorkout.LocationType.Outdoor, "hiking")]
    [InlineData("activity_swimming", CustomWorkout.ActivityType.Swimming, CustomWorkout.LocationType.Indoor, "swimming")]
    [InlineData("activity_strength", CustomWorkout.ActivityType.FunctionalStrengthTraining, CustomWorkout.LocationType.Indoor, "strength")]
    [InlineData("activity_trad_strength", CustomWorkout.ActivityType.TraditionalStrengthTraining, CustomWorkout.LocationType.Indoor, "trad_strength")]
    [InlineData("activity_core", CustomWorkout.ActivityType.CoreTraining, CustomWorkout.LocationType.Indoor, "core")]
    [InlineData("activity_hiit", CustomWorkout.ActivityType.HighIntensityIntervalTraining, CustomWorkout.LocationType.Indoor, "hiit")]
    [InlineData("activity_yoga", CustomWorkout.ActivityType.Yoga, CustomWorkout.LocationType.Indoor, "yoga")]
    [InlineData("activity_elliptical", CustomWorkout.ActivityType.Elliptical, CustomWorkout.LocationType.Indoor, "elliptical")]
    [InlineData("activity_rowing", CustomWorkout.ActivityType.Rowing, CustomWorkout.LocationType.Indoor, "rowing")]
    public void Apple_ActivityType(string filename, CustomWorkout.ActivityType activity,
        CustomWorkout.LocationType location, string displayName)
    {
        AssertAppleMatch(filename, SimpleWorkout(activity, location, displayName));
    }

    [Theory]
    [InlineData("goal_dist_km", 5, DistanceGoal.DistanceUnit.Kilometers, "dist_km")]
    [InlineData("goal_dist_m", 400, DistanceGoal.DistanceUnit.Meters, "dist_m")]
    [InlineData("goal_dist_mi", 3, DistanceGoal.DistanceUnit.Miles, "dist_mi")]
    [InlineData("goal_dist_ft", 1000, DistanceGoal.DistanceUnit.Feet, "dist_ft")]
    [InlineData("goal_dist_yd", 800, DistanceGoal.DistanceUnit.Yards, "dist_yd")]
    public void Apple_DistanceGoal(string filename, double value, DotnetWorkoutKit.Models.DistanceGoal.DistanceUnit unit, string displayName)
    {
        AssertAppleMatch(filename,
            SimpleWorkout(CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
                displayName, new DotnetWorkoutKit.Models.DistanceGoal(value, unit)));
    }

    [Theory]
    [InlineData("goal_time_sec", 90, "time_sec")]
    [InlineData("goal_time_min", 1800, "time_min")]
    [InlineData("goal_time_hr", 3600, "time_hr")]
    public void Apple_TimeGoal(string filename, int totalSeconds, string displayName)
    {
        AssertAppleMatch(filename,
            SimpleWorkout(CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
                displayName, new DotnetWorkoutKit.Models.TimeGoal(TimeSpan.FromSeconds(totalSeconds))));
    }

    [Fact]
    public void Apple_OpenGoal()
    {
        AssertAppleMatch("goal_open",
            SimpleWorkout(CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
                "open_goal", new DotnetWorkoutKit.Models.OpenGoal()));
    }

    [Fact]
    public void Apple_HeartRateAlert()
    {
        AssertAppleMatch("alert_hr",
            SimpleWorkout(CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
                "hr_alert", new DotnetWorkoutKit.Models.DistanceGoal(5, DotnetWorkoutKit.Models.DistanceGoal.DistanceUnit.Kilometers),
                new HeartRateRangeAlert(140, 160)));
    }

    [Theory]
    [InlineData("alert_spd_ms_cur", 3.5, 4.5, SpeedRangeAlert.SpeedUnit.MetersPerSecond, SpeedRangeAlert.AlertMetric.Current, "spd_ms_cur")]
    [InlineData("alert_spd_ms_avg", 3.5, 4.5, SpeedRangeAlert.SpeedUnit.MetersPerSecond, SpeedRangeAlert.AlertMetric.Average, "spd_ms_avg")]
    [InlineData("alert_spd_kmh_cur", 10, 12, SpeedRangeAlert.SpeedUnit.KilometersPerHour, SpeedRangeAlert.AlertMetric.Current, "spd_kmh_cur")]
    [InlineData("alert_spd_kmh_avg", 10, 12, SpeedRangeAlert.SpeedUnit.KilometersPerHour, SpeedRangeAlert.AlertMetric.Average, "spd_kmh_avg")]
    [InlineData("alert_spd_mph_cur", 6, 8, SpeedRangeAlert.SpeedUnit.MilesPerHour, SpeedRangeAlert.AlertMetric.Current, "spd_mph_cur")]
    [InlineData("alert_spd_mph_avg", 6, 8, SpeedRangeAlert.SpeedUnit.MilesPerHour, SpeedRangeAlert.AlertMetric.Average, "spd_mph_avg")]
    public void Apple_SpeedAlert(string filename, double min, double max,
        SpeedRangeAlert.SpeedUnit unit, SpeedRangeAlert.AlertMetric metric, string displayName)
    {
        var distUnit = unit == SpeedRangeAlert.SpeedUnit.MilesPerHour
            ? DotnetWorkoutKit.Models.DistanceGoal.DistanceUnit.Miles
            : DotnetWorkoutKit.Models.DistanceGoal.DistanceUnit.Kilometers;
        AssertAppleMatch(filename,
            SimpleWorkout(CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
                displayName, new DotnetWorkoutKit.Models.DistanceGoal(1, distUnit),
                new SpeedRangeAlert(min, max, unit, metric)));
    }

    [Fact]
    public void Apple_CadenceAlert()
    {
        AssertAppleMatch("alert_cadence",
            SimpleWorkout(CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
                "cadence", new DotnetWorkoutKit.Models.DistanceGoal(1, DotnetWorkoutKit.Models.DistanceGoal.DistanceUnit.Kilometers),
                new CadenceRangeAlert(170, 185)));
    }

    [Theory]
    [InlineData("alert_power_cur", PowerRangeAlert.PowerMetric.Current, "power_cur")]
    [InlineData("alert_power_avg", PowerRangeAlert.PowerMetric.Average, "power_avg")]
    public void Apple_PowerAlert(string filename, PowerRangeAlert.PowerMetric metric, string displayName)
    {
        AssertAppleMatch(filename,
            SimpleWorkout(CustomWorkout.ActivityType.Cycling, CustomWorkout.LocationType.Outdoor,
                displayName, new DotnetWorkoutKit.Models.TimeGoal(TimeSpan.FromMinutes(20)),
                new PowerRangeAlert(200, 250, PowerRangeAlert.PowerUnit.Watts, metric)));
    }

    [Fact]
    public void Apple_WarmupAndCooldown()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
            "warmup_cool",
            new WorkoutStep(new DotnetWorkoutKit.Models.DistanceGoal(1, DotnetWorkoutKit.Models.DistanceGoal.DistanceUnit.Kilometers),
                new HeartRateRangeAlert(130, 145)),
            [new IntervalBlock([
                new(IntervalStep.PurposeType.Work,
                    new(new DotnetWorkoutKit.Models.DistanceGoal(5, DotnetWorkoutKit.Models.DistanceGoal.DistanceUnit.Kilometers),
                        new HeartRateRangeAlert(150, 165)))
            ], 1)],
            new WorkoutStep(new DotnetWorkoutKit.Models.TimeGoal(TimeSpan.FromMinutes(5))));

        AssertAppleMatch("warmup_cooldown", workout);
    }

    [Fact]
    public void Apple_MultipleBlocksAndIterations()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
            "intervals", null,
            [new IntervalBlock([
                new(IntervalStep.PurposeType.Work,
                    new(new DotnetWorkoutKit.Models.DistanceGoal(400, DotnetWorkoutKit.Models.DistanceGoal.DistanceUnit.Meters),
                        new SpeedRangeAlert(3.8, 4.2, SpeedRangeAlert.SpeedUnit.MetersPerSecond))),
                new(IntervalStep.PurposeType.Recovery,
                    new(new DotnetWorkoutKit.Models.TimeGoal(TimeSpan.FromSeconds(90))))
            ], 6)],
            null);

        AssertAppleMatch("multi_intervals", workout);
    }
}
