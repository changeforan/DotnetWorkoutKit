using DotnetWorkoutKit.Extensions;
using DotnetWorkoutKit.Models;

namespace DotnetWorkoutKitTest;

public class DataExtensionsTests
{
    private static void AssertBinaryMatch(byte[] expected, byte[] actual)
    {
        Assert.Equal(expected[..2], actual[..2]);
        Assert.Equal(expected[38..], actual[38..]);
    }

    [Fact]
    public void CustomWorkout_Empty()
    {
        var workout = new CustomWorkout(
            activity: CustomWorkout.ActivityType.Running,
            location: CustomWorkout.LocationType.Outdoor,
            displayName: null,
            warmUp: null,
            blocks: [],
            coolDown: null
        );
        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x37, 0x36, 0x30, 0x32, 0x37, 0x37, 0x42, 0x38, 0x2D, 0x32,
            0x44, 0x31, 0x41, 0x2D, 0x34, 0x45, 0x43, 0x45, 0x2D, 0x41, 0x34, 0x35,
            0x37, 0x2D, 0x41, 0x42, 0x39, 0x43, 0x32, 0x34, 0x30, 0x33, 0x45, 0x42,
            0x35, 0x46, 0x5A, 0x04, 0x08, 0x25, 0x10, 0x03, 0xC0, 0x3E, 0x01, 0xD0,
            0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var actual_json = workout.JsonRepresentation();
        var expect_json = """
            {
              "Activity": "Running",
              "Location": "Outdoor",
              "Blocks": []
            }
            """;
        Assert.Equal(expect_json, actual_json);

        var load_from_json_binary = actual_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_Empty_HasName()
    {
        var workout = new CustomWorkout(
            activity: CustomWorkout.ActivityType.Running,
            location: CustomWorkout.LocationType.Outdoor,
            displayName: "My Workout",
            warmUp: null,
            blocks: [],
            coolDown: null
        );
        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x39, 0x34, 0x30, 0x42, 0x45, 0x35, 0x44, 0x43, 0x2D, 0x31,
            0x36, 0x46, 0x33, 0x2D, 0x34, 0x36, 0x36, 0x39, 0x2D, 0x42, 0x32, 0x44,
            0x36, 0x2D, 0x46, 0x34, 0x34, 0x31, 0x33, 0x31, 0x30, 0x41, 0x38, 0x45,
            0x41, 0x44, 0x5A, 0x10, 0x08, 0x25, 0x10, 0x03, 0x1A, 0x0A, 0x4D, 0x79,
            0x20, 0x57, 0x6F, 0x72, 0x6B, 0x6F, 0x75, 0x74, 0xC0, 0x3E, 0x01, 0xD0,
            0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var actual_json = workout.JsonRepresentation();
        var expect_json = """
            {
              "Activity": "Running",
              "Location": "Outdoor",
              "DisplayName": "My Workout",
              "Blocks": []
            }
            """;
        Assert.Equal(expect_json, actual_json);

        var load_from_json_binary = actual_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_OnlyWarmup_TimeGoal_NoAlert_NoDisplayName()
    {
        var workout = new CustomWorkout(
            activity: CustomWorkout.ActivityType.Running,
            location: CustomWorkout.LocationType.Outdoor,
            displayName: null,
            warmUp: new WorkoutStep(new TimeGoal(TimeSpan.FromMinutes(10)), null, null),
            blocks: [],
            coolDown: null
        );

        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x38, 0x45, 0x38, 0x31, 0x44, 0x44, 0x34, 0x38, 0x2D, 0x38,
            0x36, 0x34, 0x42, 0x2D, 0x34, 0x37, 0x35, 0x31, 0x2D, 0x41, 0x44, 0x39,
            0x37, 0x2D, 0x44, 0x37, 0x38, 0x41, 0x37, 0x39, 0x31, 0x37, 0x35, 0x42,
            0x39, 0x44, 0x5A, 0x17, 0x08, 0x25, 0x10, 0x03, 0x22, 0x11, 0x0A, 0x0F,
            0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x24, 0x40, 0xC0, 0x3E, 0x01, 0xD0, 0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var actual_json = workout.JsonRepresentation();
        var expect_json =
"""
{
  "Activity": "Running",
  "Location": "Outdoor",
  "WarmUp": {
    "Goal": {
      "Time": "00:10:00"
    }
  },
  "Blocks": []
}
""";

        Assert.Equal(expect_json, actual_json);

        var load_from_json_binary = actual_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_OnlyWarmup_DistanceGoal_NoAlert_NoDisplayName()
    {
        var workout = new CustomWorkout(
            activity: CustomWorkout.ActivityType.Running,
            location: CustomWorkout.LocationType.Outdoor,
            displayName: null,
            warmUp: new WorkoutStep(new DistanceGoal(1000, DistanceGoal.DistanceUnit.Meters), null, null),
            blocks: [],
            coolDown: null
        );

        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x43, 0x34, 0x41, 0x46, 0x35, 0x46, 0x42, 0x35, 0x2D, 0x41,
            0x32, 0x45, 0x33, 0x2D, 0x34, 0x44, 0x34, 0x37, 0x2D, 0x39, 0x36, 0x35,
            0x38, 0x2D, 0x35, 0x43, 0x37, 0x42, 0x35, 0x34, 0x44, 0x41, 0x44, 0x42,
            0x46, 0x32, 0x5A, 0x17, 0x08, 0x25, 0x10, 0x03, 0x22, 0x11, 0x0A, 0x0F,
            0x08, 0x03, 0x22, 0x0B, 0x08, 0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x40, 0x8F, 0x40, 0xC0, 0x3E, 0x01, 0xD0, 0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var actual_json = workout.JsonRepresentation();
        var expect_json =
"""
{
  "Activity": "Running",
  "Location": "Outdoor",
  "WarmUp": {
    "Goal": {
      "Distance": 1000,
      "Unit": "Meters"
    }
  },
  "Blocks": []
}
""";

        Assert.Equal(expect_json, actual_json);

        var load_from_json_binary = actual_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_OnlyWarmup_TimeGoal_HeartRateRangeAlert_NoDisplayName()
    {
        var workout = new CustomWorkout(
            activity: CustomWorkout.ActivityType.Running,
            location: CustomWorkout.LocationType.Outdoor,
            displayName: null,
            warmUp: new WorkoutStep(
                new TimeGoal(TimeSpan.FromMinutes(10)),
                new HeartRateRangeAlert(120, 150),
                null
            ),
            blocks: [],
            coolDown: null
        );

        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x33, 0x33, 0x41, 0x44, 0x36, 0x33, 0x43, 0x43, 0x2D, 0x45,
            0x41, 0x38, 0x32, 0x2D, 0x34, 0x36, 0x33, 0x46, 0x2D, 0x42, 0x42, 0x37,
            0x45, 0x2D, 0x44, 0x41, 0x33, 0x43, 0x41, 0x45, 0x36, 0x46, 0x31, 0x30,
            0x45, 0x38, 0x5A, 0x37, 0x08, 0x25, 0x10, 0x03, 0x22, 0x31, 0x0A, 0x0F,
            0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x24, 0x40, 0x12, 0x1E, 0x08, 0x05, 0x10, 0x02, 0x3A, 0x18, 0x12,
            0x16, 0x0A, 0x09, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5E, 0x40,
            0x12, 0x09, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x62, 0x40, 0xC0,
            0x3E, 0x01, 0xD0, 0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var actual_json = workout.JsonRepresentation();
        var expect_json =
"""
{
  "Activity": "Running",
  "Location": "Outdoor",
  "WarmUp": {
    "Goal": {
      "Time": "00:10:00"
    },
    "Alert": {
      "LowerBound": 120,
      "UpperBound": 150
    }
  },
  "Blocks": []
}
""";

        Assert.Equal(expect_json, actual_json);

        var load_from_json_binary = actual_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_OnlyWarmup_TimeGoal_SpeedRangeAlert_NoDisplayName()
    {
        var workout = new CustomWorkout(
            activity: CustomWorkout.ActivityType.Running,
            location: CustomWorkout.LocationType.Outdoor,
            displayName: null,
            warmUp: new WorkoutStep(
                new TimeGoal(TimeSpan.FromMinutes(10)),
                new SpeedRangeAlert(5, 10),
                null
            ),
            blocks: [],
            coolDown: null
        );

        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x39, 0x37, 0x36, 0x46, 0x35, 0x33, 0x43, 0x39, 0x2D, 0x36,
            0x30, 0x33, 0x36, 0x2D, 0x34, 0x30, 0x35, 0x37, 0x2D, 0x38, 0x31, 0x30,
            0x46, 0x2D, 0x43, 0x42, 0x41, 0x42, 0x37, 0x35, 0x32, 0x35, 0x46, 0x31,
            0x43, 0x43, 0x5A, 0x59, 0x08, 0x25, 0x10, 0x03, 0x22, 0x53, 0x0A, 0x0F,
            0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x24, 0x40, 0x12, 0x40, 0x08, 0x02, 0x10, 0x02, 0x22, 0x3A, 0x12,
            0x38, 0x0A, 0x1A, 0x0A, 0x0B, 0x08, 0x01, 0x11, 0xF1, 0xDC, 0x7B, 0xB8,
            0xE4, 0x38, 0xF6, 0x3F, 0x12, 0x0B, 0x08, 0x01, 0x11, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0xF0, 0x3F, 0x12, 0x1A, 0x0A, 0x0B, 0x08, 0x01, 0x11,
            0xF1, 0xDC, 0x7B, 0xB8, 0xE4, 0x38, 0x06, 0x40, 0x12, 0x0B, 0x08, 0x01,
            0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0xC0, 0x3E, 0x01,
            0xD0, 0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var actual_json = workout.JsonRepresentation();
        var expect_json =
"""
{
  "Activity": "Running",
  "Location": "Outdoor",
  "WarmUp": {
    "Goal": {
      "Time": "00:10:00"
    },
    "Alert": {
      "MinSpeed": 5,
      "MaxSpeed": 10,
      "Unit": "KilometersPerHour",
      "Metric": "Current"
    }
  },
  "Blocks": []
}
""";

        Assert.Equal(expect_json, actual_json);

        var load_from_json_binary = actual_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_OnlyWarmup_TimeGoal_SpeedRangeAlert_PaceString_NoDisplayName()
    {
        var workout = new CustomWorkout(
            activity: CustomWorkout.ActivityType.Running,
            location: CustomWorkout.LocationType.Outdoor,
            displayName: null,
            warmUp: new WorkoutStep(
                new TimeGoal(TimeSpan.FromMinutes(10)),
                new SpeedRangeAlert("5'15\"", "5'00\""),
                null
            ),
            blocks: [],
            coolDown: null
        );

        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x30, 0x32, 0x30, 0x37, 0x44, 0x35, 0x37, 0x39, 0x2D, 0x30,
            0x38, 0x39, 0x41, 0x2D, 0x34, 0x33, 0x45, 0x38, 0x2D, 0x38, 0x41, 0x33,
            0x45, 0x2D, 0x41, 0x39, 0x39, 0x44, 0x38, 0x30, 0x33, 0x37, 0x37, 0x39,
            0x35, 0x46, 0x5A, 0x59, 0x08, 0x25, 0x10, 0x03, 0x22, 0x53, 0x0A, 0x0F,
            0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x24, 0x40, 0x12, 0x40, 0x08, 0x02, 0x10, 0x02, 0x22, 0x3A, 0x12,
            0x38, 0x0A, 0x1A, 0x0A, 0x0B, 0x08, 0x01, 0x11, 0x59, 0x96, 0x65, 0x59,
            0x96, 0x65, 0x09, 0x40, 0x12, 0x0B, 0x08, 0x01, 0x11, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0xF0, 0x3F, 0x12, 0x1A, 0x0A, 0x0B, 0x08, 0x01, 0x11,
            0xAB, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0x0A, 0x40, 0x12, 0x0B, 0x08, 0x01,
            0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0xC0, 0x3E, 0x01,
            0xD0, 0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var pace_json =
"""
{
  "Activity": "Running",
  "Location": "Outdoor",
  "WarmUp": {
    "Goal": {
      "Time": "00:10:00"
    },
    "Alert": {
      "MinSpeed": "5'15\"",
      "MaxSpeed": "5'00\"",
      "Metric": "Current"
    }
  },
  "Blocks": []
}
""";
        var load_from_json_binary = pace_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_Warmup_and_CoolDown_TimeGoal_DistanceGoal_NoAlert_NoDisplayName()
    {
        var workout = new CustomWorkout(
            activity: CustomWorkout.ActivityType.Running,
            location: CustomWorkout.LocationType.Outdoor,
            displayName: null,
            warmUp: new WorkoutStep(new TimeGoal(TimeSpan.FromMinutes(10)), null, null),
            blocks: [],
            coolDown: new WorkoutStep(new DistanceGoal(1000, DistanceGoal.DistanceUnit.Meters), null, null)
        );

        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x32, 0x46, 0x35, 0x35, 0x41, 0x46, 0x35, 0x46, 0x2D, 0x44,
            0x38, 0x36, 0x44, 0x2D, 0x34, 0x41, 0x46, 0x42, 0x2D, 0x38, 0x45, 0x41,
            0x32, 0x2D, 0x31, 0x36, 0x43, 0x38, 0x37, 0x30, 0x41, 0x37, 0x36, 0x46,
            0x46, 0x39, 0x5A, 0x2A, 0x08, 0x25, 0x10, 0x03, 0x22, 0x11, 0x0A, 0x0F,
            0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x24, 0x40, 0x32, 0x11, 0x0A, 0x0F, 0x08, 0x03, 0x22, 0x0B, 0x08,
            0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x8F, 0x40, 0xC0, 0x3E,
            0x01, 0xD0, 0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var actual_json = workout.JsonRepresentation();
        var expect_json =
"""
{
  "Activity": "Running",
  "Location": "Outdoor",
  "WarmUp": {
    "Goal": {
      "Time": "00:10:00"
    }
  },
  "Blocks": [],
  "CoolDown": {
    "Goal": {
      "Distance": 1000,
      "Unit": "Meters"
    }
  }
}
""";

        Assert.Equal(expect_json, actual_json);

        var load_from_json_binary = actual_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_OnlyBlocks_MultipleIntervalBlocks()
    {
        var workout = new CustomWorkout(
            activity: CustomWorkout.ActivityType.Running,
            location: CustomWorkout.LocationType.Outdoor,
            displayName: null,
            warmUp: null,
            blocks: [
                new IntervalBlock([
                    new (IntervalStep.PurposeType.Work, new (new DistanceGoal(3, DistanceGoal.DistanceUnit.Kilometers), new HeartRateRangeAlert(140, 160))),
                    new (IntervalStep.PurposeType.Recovery, new (new TimeGoal(TimeSpan.FromMinutes(2))))
                    ], 2),
                new IntervalBlock([
                    new (IntervalStep.PurposeType.Work, new (new DistanceGoal(200, DistanceGoal.DistanceUnit.Meters), new SpeedRangeAlert("4'09\"", "3'59\""))),
                    new (IntervalStep.PurposeType.Recovery, new (new DistanceGoal(200, DistanceGoal.DistanceUnit.Meters)))
                    ], 6)
            ],
            coolDown: null
        );

        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x44, 0x33, 0x42, 0x41, 0x42, 0x41, 0x46, 0x32, 0x2D, 0x35,
            0x33, 0x44, 0x44, 0x2D, 0x34, 0x46, 0x30, 0x45, 0x2D, 0x41, 0x38, 0x44,
            0x43, 0x2D, 0x38, 0x34, 0x39, 0x34, 0x42, 0x30, 0x35, 0x34, 0x41, 0x46,
            0x42, 0x44, 0x5A, 0xCA, 0x01, 0x08, 0x25, 0x10, 0x03, 0x2A, 0x50, 0x0A,
            0x35, 0x08, 0x01, 0x12, 0x31, 0x0A, 0x0F, 0x08, 0x03, 0x22, 0x0B, 0x08,
            0x02, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x40, 0x12, 0x1E,
            0x08, 0x05, 0x10, 0x02, 0x3A, 0x18, 0x12, 0x16, 0x0A, 0x09, 0x09, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x80, 0x61, 0x40, 0x12, 0x09, 0x09, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x64, 0x40, 0x0A, 0x15, 0x08, 0x02, 0x12, 0x11,
            0x0A, 0x0F, 0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x40, 0x10, 0x02, 0x2A, 0x72, 0x0A, 0x57, 0x08,
            0x01, 0x12, 0x53, 0x0A, 0x0F, 0x08, 0x03, 0x22, 0x0B, 0x08, 0x01, 0x11,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69, 0x40, 0x12, 0x40, 0x08, 0x02,
            0x10, 0x02, 0x22, 0x3A, 0x12, 0x38, 0x0A, 0x1A, 0x0A, 0x0B, 0x08, 0x01,
            0x11, 0xF8, 0x47, 0x0A, 0x26, 0x73, 0x10, 0x10, 0x40, 0x12, 0x0B, 0x08,
            0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x12, 0x1A,
            0x0A, 0x0B, 0x08, 0x01, 0x11, 0x12, 0x01, 0xF1, 0xD1, 0x84, 0xBC, 0x10,
            0x40, 0x12, 0x0B, 0x08, 0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xF0, 0x3F, 0x0A, 0x15, 0x08, 0x02, 0x12, 0x11, 0x0A, 0x0F, 0x08, 0x03,
            0x22, 0x0B, 0x08, 0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69,
            0x40, 0x10, 0x06, 0xC0, 0x3E, 0x01, 0xD0, 0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var actual_json = workout.JsonRepresentation();
        var expect_json =
"""
{
  "Activity": "Running",
  "Location": "Outdoor",
  "Blocks": [
    {
      "IntervalSteps": [
        {
          "Purpose": "Work",
          "WorkoutStep": {
            "Goal": {
              "Distance": 3,
              "Unit": "Kilometers"
            },
            "Alert": {
              "LowerBound": 140,
              "UpperBound": 160
            }
          }
        },
        {
          "Purpose": "Recovery",
          "WorkoutStep": {
            "Goal": {
              "Time": "00:02:00"
            }
          }
        }
      ],
      "Iterations": 2
    },
    {
      "IntervalSteps": [
        {
          "Purpose": "Work",
          "WorkoutStep": {
            "Goal": {
              "Distance": 200,
              "Unit": "Meters"
            },
            "Alert": {
              "MinSpeed": 4.016064257028113,
              "MaxSpeed": 4.184100418410042,
              "Unit": "MetersPerSecond",
              "Metric": "Current"
            }
          }
        },
        {
          "Purpose": "Recovery",
          "WorkoutStep": {
            "Goal": {
              "Distance": 200,
              "Unit": "Meters"
            }
          }
        }
      ],
      "Iterations": 6
    }
  ]
}
""";

        Assert.Equal(expect_json, actual_json);

        var load_from_json_binary = actual_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_Sample()
    {
        var workout = new CustomWorkout(
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

        var actual_binary = workout.DataRepresentation();
        var expect_binary = new byte[] {
            0x4A, 0x24, 0x38, 0x36, 0x46, 0x37, 0x39, 0x43, 0x39, 0x42, 0x2D, 0x33,
            0x30, 0x34, 0x37, 0x2D, 0x34, 0x43, 0x30, 0x38, 0x2D, 0x41, 0x34, 0x30,
            0x43, 0x2D, 0x33, 0x38, 0x43, 0x39, 0x43, 0x42, 0x31, 0x37, 0x34, 0x41,
            0x43, 0x37, 0x5A, 0xEE, 0x02, 0x08, 0x25, 0x10, 0x03, 0x1A, 0x06, 0x73,
            0x61, 0x6D, 0x70, 0x6C, 0x65, 0x22, 0x3A, 0x0A, 0x0F, 0x08, 0x03, 0x22,
            0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x40,
            0x12, 0x1E, 0x08, 0x05, 0x10, 0x02, 0x3A, 0x18, 0x12, 0x16, 0x0A, 0x09,
            0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x62, 0x40, 0x12, 0x09, 0x09,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x63, 0x40, 0x1A, 0x07, 0x57, 0x61,
            0x72, 0x6D, 0x20, 0x55, 0x70, 0x2A, 0x72, 0x0A, 0x57, 0x08, 0x01, 0x12,
            0x53, 0x0A, 0x0F, 0x08, 0x03, 0x22, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x08, 0x40, 0x12, 0x40, 0x08, 0x02, 0x10, 0x02,
            0x22, 0x3A, 0x12, 0x38, 0x0A, 0x1A, 0x0A, 0x0B, 0x08, 0x01, 0x11, 0x48,
            0xA8, 0x3E, 0xD3, 0xD6, 0xF8, 0x0B, 0x40, 0x12, 0x0B, 0x08, 0x01, 0x11,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x12, 0x1A, 0x0A, 0x0B,
            0x08, 0x01, 0x11, 0x2A, 0xDB, 0xBB, 0x0E, 0xE8, 0xC6, 0x0C, 0x40, 0x12,
            0x0B, 0x08, 0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F,
            0x0A, 0x15, 0x08, 0x02, 0x12, 0x11, 0x0A, 0x0F, 0x08, 0x01, 0x12, 0x0B,
            0x08, 0x02, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x10,
            0x02, 0x2A, 0x72, 0x0A, 0x57, 0x08, 0x01, 0x12, 0x53, 0x0A, 0x0F, 0x08,
            0x03, 0x22, 0x0B, 0x08, 0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x69, 0x40, 0x12, 0x40, 0x08, 0x02, 0x10, 0x02, 0x22, 0x3A, 0x12, 0x38,
            0x0A, 0x1A, 0x0A, 0x0B, 0x08, 0x01, 0x11, 0xF8, 0x47, 0x0A, 0x26, 0x73,
            0x10, 0x10, 0x40, 0x12, 0x0B, 0x08, 0x01, 0x11, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0xF0, 0x3F, 0x12, 0x1A, 0x0A, 0x0B, 0x08, 0x01, 0x11, 0x12,
            0x01, 0xF1, 0xD1, 0x84, 0xBC, 0x10, 0x40, 0x12, 0x0B, 0x08, 0x01, 0x11,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x0A, 0x15, 0x08, 0x02,
            0x12, 0x11, 0x0A, 0x0F, 0x08, 0x03, 0x22, 0x0B, 0x08, 0x01, 0x11, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x69, 0x40, 0x10, 0x06, 0x32, 0x3C, 0x0A,
            0x0F, 0x08, 0x03, 0x22, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x08, 0x40, 0x12, 0x1E, 0x08, 0x05, 0x10, 0x02, 0x3A, 0x18,
            0x12, 0x16, 0x0A, 0x09, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x62,
            0x40, 0x12, 0x09, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x63, 0x40,
            0x1A, 0x09, 0x43, 0x6F, 0x6F, 0x6C, 0x20, 0x44, 0x6F, 0x77, 0x6E, 0xC0,
            0x3E, 0x01, 0xD0, 0x3E, 0x05
        };

        AssertBinaryMatch(expect_binary, actual_binary);

        var actual_json = workout.JsonRepresentation();
        var expect_json =
"""
{
  "Activity": "Running",
  "Location": "Outdoor",
  "DisplayName": "sample",
  "WarmUp": {
    "Goal": {
      "Distance": 3,
      "Unit": "Kilometers"
    },
    "Alert": {
      "LowerBound": 144,
      "UpperBound": 153
    },
    "DisplayName": "Warm Up"
  },
  "Blocks": [
    {
      "IntervalSteps": [
        {
          "Purpose": "Work",
          "WorkoutStep": {
            "Goal": {
              "Distance": 3,
              "Unit": "Kilometers"
            },
            "Alert": {
              "MinSpeed": 3.4965034965034967,
              "MaxSpeed": 3.597122302158273,
              "Unit": "MetersPerSecond",
              "Metric": "Current"
            }
          }
        },
        {
          "Purpose": "Recovery",
          "WorkoutStep": {
            "Goal": {
              "Time": "00:02:00"
            }
          }
        }
      ],
      "Iterations": 2
    },
    {
      "IntervalSteps": [
        {
          "Purpose": "Work",
          "WorkoutStep": {
            "Goal": {
              "Distance": 200,
              "Unit": "Meters"
            },
            "Alert": {
              "MinSpeed": 4.016064257028113,
              "MaxSpeed": 4.184100418410042,
              "Unit": "MetersPerSecond",
              "Metric": "Current"
            }
          }
        },
        {
          "Purpose": "Recovery",
          "WorkoutStep": {
            "Goal": {
              "Distance": 200,
              "Unit": "Meters"
            }
          }
        }
      ],
      "Iterations": 6
    }
  ],
  "CoolDown": {
    "Goal": {
      "Distance": 3,
      "Unit": "Kilometers"
    },
    "Alert": {
      "LowerBound": 144,
      "UpperBound": 153
    },
    "DisplayName": "Cool Down"
  }
}
""";

        Assert.Equal(expect_json, actual_json);

        var load_from_json_binary = actual_json.LoadFromJson()?.DataRepresentation();
        Assert.NotNull(load_from_json_binary);
        AssertBinaryMatch(expect_binary, load_from_json_binary);
    }

    [Fact]
    public void CustomWorkout_OpenGoal()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
            null, new WorkoutStep(new OpenGoal()), [], null);

        var bin = workout.DataRepresentation();
        Assert.True(bin.Length > 38);

        var json = workout.JsonRepresentation();
        Assert.Contains("\"Open\": true", json);

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        Assert.IsType<OpenGoal>(loaded.WarmUp?.Goal);

        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Fact]
    public void CustomWorkout_CadenceRangeAlert()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
            null, null,
            [new IntervalBlock([
                new(IntervalStep.PurposeType.Work,
                    new(new DistanceGoal(1, DistanceGoal.DistanceUnit.Kilometers),
                        new CadenceRangeAlert(170, 185)))
            ], 1)], null);

        var bin = workout.DataRepresentation();
        Assert.True(bin.Length > 38);

        var json = workout.JsonRepresentation();
        Assert.Contains("\"MinCadence\": 170", json);
        Assert.Contains("\"MaxCadence\": 185", json);

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        var alert = loaded.Blocks[0].IntervalSteps[0].WorkoutStep.Alert as CadenceRangeAlert;
        Assert.NotNull(alert);
        Assert.Equal(170, alert.MinCadence);
        Assert.Equal(185, alert.MaxCadence);

        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Fact]
    public void CustomWorkout_PowerRangeAlert_Current()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Cycling, CustomWorkout.LocationType.Outdoor,
            null, null,
            [new IntervalBlock([
                new(IntervalStep.PurposeType.Work,
                    new(new TimeGoal(TimeSpan.FromMinutes(20)),
                        new PowerRangeAlert(200, 250, PowerRangeAlert.PowerUnit.Watts, PowerRangeAlert.PowerMetric.Current)))
            ], 1)], null);

        var bin = workout.DataRepresentation();
        Assert.True(bin.Length > 38);

        var json = workout.JsonRepresentation();
        Assert.Contains("\"MinPower\": 200", json);
        Assert.Contains("\"MaxPower\": 250", json);
        Assert.Contains("\"Metric\": \"Current\"", json);

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        var alert = loaded.Blocks[0].IntervalSteps[0].WorkoutStep.Alert as PowerRangeAlert;
        Assert.NotNull(alert);
        Assert.Equal(200, alert.MinPower);
        Assert.Equal(250, alert.MaxPower);
        Assert.Equal(PowerRangeAlert.PowerMetric.Current, alert.Metric);

        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Fact]
    public void CustomWorkout_PowerRangeAlert_Average()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Cycling, CustomWorkout.LocationType.Outdoor,
            null, null,
            [new IntervalBlock([
                new(IntervalStep.PurposeType.Work,
                    new(new TimeGoal(TimeSpan.FromMinutes(20)),
                        new PowerRangeAlert(200, 250, PowerRangeAlert.PowerUnit.Watts, PowerRangeAlert.PowerMetric.Average)))
            ], 1)], null);

        var bin = workout.DataRepresentation();
        var json = workout.JsonRepresentation();
        Assert.Contains("\"Metric\": \"Average\"", json);

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        var alert = loaded.Blocks[0].IntervalSteps[0].WorkoutStep.Alert as PowerRangeAlert;
        Assert.NotNull(alert);
        Assert.Equal(PowerRangeAlert.PowerMetric.Average, alert.Metric);

        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Fact]
    public void CustomWorkout_DistanceGoal_Miles()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
            null, new WorkoutStep(new DistanceGoal(1, DistanceGoal.DistanceUnit.Miles)), [], null);

        var json = workout.JsonRepresentation();
        Assert.Contains("\"Unit\": \"Miles\"", json);

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        var goal = loaded.WarmUp?.Goal as DistanceGoal;
        Assert.NotNull(goal);
        Assert.Equal(DistanceGoal.DistanceUnit.Miles, goal.Unit);

        var bin = workout.DataRepresentation();
        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Fact]
    public void CustomWorkout_DistanceGoal_Yards()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Swimming, CustomWorkout.LocationType.Indoor,
            null, new WorkoutStep(new DistanceGoal(100, DistanceGoal.DistanceUnit.Yards)), [], null);

        var json = workout.JsonRepresentation();
        Assert.Contains("\"Unit\": \"Yards\"", json);

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        var goal = loaded.WarmUp?.Goal as DistanceGoal;
        Assert.NotNull(goal);
        Assert.Equal(DistanceGoal.DistanceUnit.Yards, goal.Unit);

        var bin = workout.DataRepresentation();
        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Fact]
    public void CustomWorkout_DistanceGoal_Feet()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Walking, CustomWorkout.LocationType.Outdoor,
            null, new WorkoutStep(new DistanceGoal(5280, DistanceGoal.DistanceUnit.Feet)), [], null);

        var json = workout.JsonRepresentation();
        Assert.Contains("\"Unit\": \"Feet\"", json);

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        var goal = loaded.WarmUp?.Goal as DistanceGoal;
        Assert.NotNull(goal);
        Assert.Equal(DistanceGoal.DistanceUnit.Feet, goal.Unit);

        var bin = workout.DataRepresentation();
        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Fact]
    public void CustomWorkout_TimeGoal_Seconds()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
            null, new WorkoutStep(new TimeGoal(TimeSpan.FromSeconds(90))), [], null);

        var bin = workout.DataRepresentation();
        var json = workout.JsonRepresentation();

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Fact]
    public void CustomWorkout_TimeGoal_Hours()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
            null, new WorkoutStep(new TimeGoal(TimeSpan.FromHours(1))), [], null);

        var bin = workout.DataRepresentation();
        var json = workout.JsonRepresentation();

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Theory]
    [InlineData(CustomWorkout.ActivityType.Cycling)]
    [InlineData(CustomWorkout.ActivityType.Swimming)]
    [InlineData(CustomWorkout.ActivityType.Walking)]
    [InlineData(CustomWorkout.ActivityType.Hiking)]
    [InlineData(CustomWorkout.ActivityType.Yoga)]
    [InlineData(CustomWorkout.ActivityType.HighIntensityIntervalTraining)]
    [InlineData(CustomWorkout.ActivityType.CoreTraining)]
    [InlineData(CustomWorkout.ActivityType.Elliptical)]
    [InlineData(CustomWorkout.ActivityType.Rowing)]
    [InlineData(CustomWorkout.ActivityType.StairClimbing)]
    [InlineData(CustomWorkout.ActivityType.FunctionalStrengthTraining)]
    [InlineData(CustomWorkout.ActivityType.TraditionalStrengthTraining)]
    [InlineData(CustomWorkout.ActivityType.CrossTraining)]
    public void CustomWorkout_ActivityTypes(CustomWorkout.ActivityType activityType)
    {
        var workout = new CustomWorkout(activityType, CustomWorkout.LocationType.Outdoor, null, null, [], null);
        var bin = workout.DataRepresentation();
        Assert.True(bin.Length > 38);

        var json = workout.JsonRepresentation();
        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        Assert.Equal(activityType, loaded.Activity);

        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }

    [Fact]
    public void CustomWorkout_SpeedAlert_MilesPerHour()
    {
        var workout = new CustomWorkout(
            CustomWorkout.ActivityType.Running, CustomWorkout.LocationType.Outdoor,
            null, null,
            [new IntervalBlock([
                new(IntervalStep.PurposeType.Work,
                    new(new DistanceGoal(1, DistanceGoal.DistanceUnit.Miles),
                        new SpeedRangeAlert(6, 8, SpeedRangeAlert.SpeedUnit.MilesPerHour)))
            ], 1)], null);

        var bin = workout.DataRepresentation();
        var json = workout.JsonRepresentation();
        Assert.Contains("\"Unit\": \"MilesPerHour\"", json);

        var loaded = json.LoadFromJson();
        Assert.NotNull(loaded);
        var rebin = loaded.DataRepresentation();
        Assert.Equal(bin[38..], rebin[38..]);
    }
}
