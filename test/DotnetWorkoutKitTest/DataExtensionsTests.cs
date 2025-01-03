using DotnetWorkoutKit.Extensions;
using DotnetWorkoutKit.Models;

namespace DotnetWorkoutKitTest;

public class DataExtensionsTests
{
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
            0x4A, 0x24, 0x41, 0x35, 0x45, 0x30, 0x31, 0x42, 0x33, 0x42, 0x2D, 0x37,
            0x30, 0x41, 0x42, 0x2D, 0x34, 0x46, 0x37, 0x37, 0x2D, 0x42, 0x33, 0x36,
            0x30, 0x2D, 0x45, 0x45, 0x43, 0x32, 0x45, 0x35, 0x41, 0x37, 0x39, 0x39,
            0x39, 0x42, 0x5A, 0x06, 0x08, 0x25, 0x10, 0x03, 0x1A, 0x00, 0xC0, 0x3E,
            0x01, 0xD0, 0x3E, 0x05
        };

        // skip the random GUID
        Assert.Equal(expect_binary[..2], actual_binary[..2]);
        Assert.Equal(expect_binary[^14..], actual_binary[^14..]);

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
        Assert.Equal(expect_binary[..2], load_from_json_binary[..2]);
        Assert.Equal(expect_binary[^14..], load_from_json_binary[^14..]);
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
            0x4A, 0x24, 0x32, 0x43, 0x32, 0x34, 0x31, 0x41, 0x30, 0x44, 0x2D, 0x45,
            0x33, 0x42, 0x30, 0x2D, 0x34, 0x45, 0x33, 0x46, 0x2D, 0x38, 0x45, 0x45,
            0x45, 0x2D, 0x35, 0x39, 0x39, 0x33, 0x35, 0x45, 0x41, 0x39, 0x30, 0x37,
            0x43, 0x39, 0x5A, 0x10, 0x08, 0x25, 0x10, 0x03, 0x1A, 0x0A, 0x4D, 0x79,
            0x20, 0x57, 0x6F, 0x72, 0x6B, 0x6F, 0x75, 0x74, 0xC0, 0x3E, 0x01, 0xD0,
            0x3E, 0x05
        };

        // skip the random GUID
        Assert.Equal(expect_binary[..2], actual_binary[..2]);
        Assert.Equal(expect_binary[^24..], actual_binary[^24..]);

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
        Assert.Equal(expect_binary[..2], load_from_json_binary[..2]);
        Assert.Equal(expect_binary[^24..], load_from_json_binary[^24..]);
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
            0x4A, 0x24, 0x33, 0x31, 0x34, 0x35, 0x35, 0x31, 0x45, 0x37, 0x2D, 0x31,
            0x34, 0x32, 0x30, 0x2D, 0x34, 0x46, 0x45, 0x36, 0x2D, 0x38, 0x41, 0x32,
            0x32, 0x2D, 0x36, 0x43, 0x39, 0x36, 0x36, 0x34, 0x42, 0x45, 0x45, 0x44,
            0x37, 0x45, 0x5A, 0x1B, 0x08, 0x25, 0x10, 0x03, 0x1A, 0x00, 0x22, 0x13,
            0x0A, 0x0F, 0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x24, 0x40, 0x1A, 0x00, 0xC0, 0x3E, 0x01, 0xD0, 0x3E,
            0x05
        };

        Assert.Equal(expect_binary[..2], actual_binary[..2]);
        Assert.Equal(expect_binary[39..], actual_binary[39..]);

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
        Assert.Equal(expect_binary[..2], load_from_json_binary[..2]);
        Assert.Equal(expect_binary[38..], load_from_json_binary[38..]);
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
            0x4A, 0x24, 0x42, 0x46, 0x38, 0x42, 0x39, 0x30, 0x32, 0x42, 0x2D, 0x46,
            0x43, 0x35, 0x37, 0x2D, 0x34, 0x42, 0x45, 0x46, 0x2D, 0x38, 0x41, 0x42,
            0x43, 0x2D, 0x32, 0x31, 0x34, 0x36, 0x42, 0x36, 0x46, 0x45, 0x46, 0x42,
            0x42, 0x46, 0x5A, 0x1B, 0x08, 0x25, 0x10, 0x03, 0x1A, 0x00, 0x22, 0x13,
            0x0A, 0x0F, 0x08, 0x03, 0x22, 0x0B, 0x08, 0x01, 0x11, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x40, 0x8F, 0x40, 0x1A, 0x00, 0xC0, 0x3E, 0x01, 0xD0, 0x3E,
            0x05
        };

        Assert.Equal(expect_binary[..2], actual_binary[..2]);
        Assert.Equal(expect_binary[38..], actual_binary[38..]);

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
        Assert.Equal(expect_binary[..2], load_from_json_binary[..2]);
        Assert.Equal(expect_binary[38..], load_from_json_binary[38..]);
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
            0x4A, 0x24, 0x43, 0x42, 0x38, 0x32, 0x45, 0x46, 0x41, 0x33, 0x2D, 0x38,
            0x43, 0x39, 0x31, 0x2D, 0x34, 0x37, 0x36, 0x38, 0x2D, 0x39, 0x41, 0x43,
            0x34, 0x2D, 0x31, 0x36, 0x44, 0x38, 0x39, 0x31, 0x32, 0x39, 0x35, 0x43,
            0x38, 0x46, 0x5A, 0x3B, 0x08, 0x25, 0x10, 0x03, 0x1A, 0x00, 0x22, 0x33,
            0x0A, 0x0F, 0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x24, 0x40, 0x12, 0x1E, 0x08, 0x05, 0x10, 0x02, 0x3A,
            0x18, 0x12, 0x16, 0x0A, 0x09, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x5E, 0x40, 0x12, 0x09, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x62,
            0x40, 0x1A, 0x00, 0xC0, 0x3E, 0x01, 0xD0, 0x3E, 0x05
        };

        Assert.Equal(expect_binary[..2], actual_binary[..2]);
        Assert.Equal(expect_binary[39..], actual_binary[39..]);

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
        Assert.Equal(expect_binary[..2], load_from_json_binary[..2]);
        Assert.Equal(expect_binary[38..], load_from_json_binary[38..]);
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
            0x4A, 0x24, 0x32, 0x31, 0x32, 0x36, 0x44, 0x37, 0x41, 0x34, 0x2D, 0x44,
            0x36, 0x38, 0x37, 0x2D, 0x34, 0x43, 0x32, 0x37, 0x2D, 0x41, 0x46, 0x45,
            0x45, 0x2D, 0x45, 0x32, 0x45, 0x36, 0x38, 0x43, 0x33, 0x41, 0x36, 0x35,
            0x30, 0x42, 0x5A, 0x5D, 0x08, 0x25, 0x10, 0x03, 0x1A, 0x00, 0x22, 0x55,
            0x0A, 0x0F, 0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x24, 0x40, 0x12, 0x40, 0x08, 0x02, 0x10, 0x02, 0x22,
            0x3A, 0x12, 0x38, 0x0A, 0x1A, 0x0A, 0x0B, 0x08, 0x01, 0x11, 0x8E, 0xE3,
            0x38, 0x8E, 0xE3, 0x38, 0xF6, 0x3F, 0x12, 0x0B, 0x08, 0x01, 0x11, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x12, 0x1A, 0x0A, 0x0B, 0x08,
            0x01, 0x11, 0x8E, 0xE3, 0x38, 0x8E, 0xE3, 0x38, 0x06, 0x40, 0x12, 0x0B,
            0x08, 0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x1A,
            0x00, 0xC0, 0x3E, 0x01, 0xD0, 0x3E, 0x05
        };

        Assert.Equal(expect_binary[..2], actual_binary[..2]);
        Assert.Equal(expect_binary[39..], actual_binary[39..]);

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
        Assert.Equal(expect_binary[..2], load_from_json_binary[..2]);
        Assert.Equal(expect_binary[38..], load_from_json_binary[38..]);
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
            0x4A, 0x24, 0x30, 0x38, 0x37, 0x37, 0x36, 0x41, 0x30, 0x37, 0x2D, 0x38,
            0x46, 0x38, 0x35, 0x2D, 0x34, 0x42, 0x34, 0x45, 0x2D, 0x38, 0x34, 0x41,
            0x43, 0x2D, 0x32, 0x32, 0x32, 0x35, 0x34, 0x35, 0x38, 0x32, 0x30, 0x33,
            0x39, 0x41, 0x5A, 0x5D, 0x08, 0x25, 0x10, 0x03, 0x1A, 0x00, 0x22, 0x55,
            0x0A, 0x0F, 0x08, 0x01, 0x12, 0x0B, 0x08, 0x02, 0x11, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x24, 0x40, 0x12, 0x40, 0x08, 0x02, 0x10, 0x02, 0x22,
            0x3A, 0x12, 0x38, 0x0A, 0x1A, 0x0A, 0x0B, 0x08, 0x01, 0x11, 0x59, 0x96,
            0x65, 0x59, 0x96, 0x65, 0x09, 0x40, 0x12, 0x0B, 0x08, 0x01, 0x11, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x12, 0x1A, 0x0A, 0x0B, 0x08,
            0x01, 0x11, 0xAB, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0x0A, 0x40, 0x12, 0x0B,
            0x08, 0x01, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x1A,
            0x00, 0xC0, 0x3E, 0x01, 0xD0, 0x3E, 0x05
        };

        Assert.Equal(expect_binary[..2], actual_binary[..2]);
        Assert.Equal(expect_binary[39..], actual_binary[39..]);

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
        Assert.Equal(expect_binary[..2], load_from_json_binary[..2]);
        Assert.Equal(expect_binary[38..], load_from_json_binary[38..]);
    }

}
