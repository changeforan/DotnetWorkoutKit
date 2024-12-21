using Google.Protobuf;
using DotnetWorkoutKit.Protobuf;
using DotnetWorkoutKit.Protobuf.CustomWorkout;
using DotnetWorkoutKit.Protobuf.CustomWorkout.Alert;

var workoutBin = new WorkoutBinary
{
    GUID = Guid.NewGuid().ToString().ToUpper(),
    CustomWorkout = CreateCustomWorkout()
};

var data = workoutBin.ToByteArray();

// magic trailer
byte[] endBlock = [
	0xC0, 0x3E, 0x01, 0xD0, 0x3E, 0x05
];

data = [.. data, .. endBlock];

// Save to file
File.WriteAllBytes("test.workout", data);

static CustomWorkout CreateCustomWorkout()
{
    var result = new CustomWorkout
    {
        ActivityType = CustomWorkout.Types.ActivityType.Running,
        LocationType = CustomWorkout.Types.LocationType.Outdoor,
        DisplayName = "My Workout"
    };

    // optional warmup
    result.Warmup = new()
    {
        WorkoutGoal = new()
        {
            GoalType = WorkoutGoal.Types.GoalType.Time,
            TimeGoal = new()
            {
                UnitType = WorkoutGoal.Types.TimeGoal.Types.TimeUnitType.Minutes,
                UnitValue = 5
            }
        },
        WorkoutAlert = new()
        {
            AlertMetric = WorkoutAlert.Types.AlertMetricEnum.CountPerMinute,
            Unknown = 2,
            HeartRateRangeAlert = new()
            {
                HeartRateRange = new()
                {
                    LowerBound = new () {Value = 130},
                    UpperBound = new () {Value = 154}
                }
            }
        },
        DisplayName = "warmup",
    };

    // optional cooldown
    result.Cooldown = new()
    {
        WorkoutGoal = new()
        {
            GoalType = WorkoutGoal.Types.GoalType.Distance,
            DistanceGoal = new()
            {
                UnitType = WorkoutGoal.Types.DistanceGoal.Types.DistanceUnitType.Kilometers,
                UnitValue = 10
            }
        },
        WorkoutAlert = new()
        {
            AlertMetric = WorkoutAlert.Types.AlertMetricEnum.CountPerMinute,
            Unknown = 2,
            HeartRateRangeAlert = new()
            {
                HeartRateRange = new()
                {
                    LowerBound = new () {Value = 123},
                    UpperBound = new () {Value = 145}
                }
            }
        }
    };

    result.IntervalBlocks.Add(
        CreateIntervalBlock()
    );

    return result;
}

static IntervalBlock CreateIntervalBlock()
{
    var result = new IntervalBlock
    {
        Iterations = 7,
    };

    result.IntervalSteps.Add(
        new IntervalBlock.Types.IntervalStep
        {
            Purpose = IntervalBlock.Types.IntervalStep.Types.IntervalPurpose.Work,
            WorkoutStep = new()
            {
                WorkoutGoal = new()
                {
                    GoalType = WorkoutGoal.Types.GoalType.Distance,
                    DistanceGoal = new()
                    {
                        UnitType = WorkoutGoal.Types.DistanceGoal.Types.DistanceUnitType.Kilometers,
                        UnitValue = 8
                    }
                },
                WorkoutAlert = new()
                {
                    AlertMetric = WorkoutAlert.Types.AlertMetricEnum.Current,
                    Unknown = 2,
                    SpeedAlert = new()
                    {
                        SpeedRangeAlert = new()
                        {
                            LowerBound = new () { Speed = new () { Unit = SpeedAlert.Types.Speed.Types.SpeedUnitEnum.MetersPerSecond, Speed_ = 5.1 }, Unknown = new () { First = 1, Second =1 } },
                            UpperBound = new () { Speed = new () { Unit = SpeedAlert.Types.Speed.Types.SpeedUnitEnum.MetersPerSecond, Speed_ = 6.1 }, Unknown = new () { First = 1, Second =1 } }
                        }
                    }
                }
            }
        }
    );

    result.IntervalSteps.Add(
        new IntervalBlock.Types.IntervalStep
        {
            Purpose = IntervalBlock.Types.IntervalStep.Types.IntervalPurpose.Recovery,
            WorkoutStep = new()
            {
                WorkoutGoal = new()
                {
                    GoalType = WorkoutGoal.Types.GoalType.Time,
                    TimeGoal = new()
                    {
                        UnitType = WorkoutGoal.Types.TimeGoal.Types.TimeUnitType.Minutes,
                        UnitValue = 2
                    }
                },
                DisplayName = "break",
            } 
        }
    );

    return result;
}