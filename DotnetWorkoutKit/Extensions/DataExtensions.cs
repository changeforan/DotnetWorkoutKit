using System.Text.Json;
using System.Text.Json.Serialization;
using DotnetWorkoutKit.JsonConverters;
using DotnetWorkoutKit.Protobuf;
using DotnetWorkoutKit.Protobuf.CustomWorkout;
using DotnetWorkoutKit.Protobuf.CustomWorkout.Alert;
using Google.Protobuf;
using static DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutGoal.Types;

namespace DotnetWorkoutKit.Extensions;

public static class DataExtensions
{
    public static byte[] DataRepresentation(this Models.CustomWorkout customWorkout)
    {
        var workoutBin = new WorkoutBinary
        {
            GUID = Guid.NewGuid().ToString().ToUpper(),
            CustomWorkout = ConvertToCustomWorkout(customWorkout)
        };

        var data = workoutBin.ToByteArray();

        // magic trailer
        byte[] endBlock = [
            0xC0, 0x3E, 0x01, 0xD0, 0x3E, 0x05
        ];

        return [.. data, .. endBlock];
    }

    public static string JsonRepresentation(this Models.CustomWorkout customWorkout)
    {
        return JsonSerializer.Serialize(customWorkout, _jsonOptions);
    }

    public static Models.CustomWorkout? LoadFromJson(this string json)
    {
        return JsonSerializer.Deserialize<Models.CustomWorkout>(json);
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private static CustomWorkout ConvertToCustomWorkout(Models.CustomWorkout customWorkout)
    {
        var result = new CustomWorkout
        {
            ActivityType = customWorkout.Activity switch
            {
                Models.CustomWorkout.ActivityType.Running => CustomWorkout.Types.ActivityType.Running,
                _ => throw new ArgumentException("Only running is supported now.")
            },
            LocationType = customWorkout.Location switch
            {
                Models.CustomWorkout.LocationType.Indoor => CustomWorkout.Types.LocationType.Indoor,
                Models.CustomWorkout.LocationType.Outdoor => CustomWorkout.Types.LocationType.Outdoor,
                _ => throw new ArgumentException("Only indoor and outdoor are supported now.")
            },
            DisplayName = customWorkout.DisplayName
        };

        result.Warmup = ConvertToWorkoutStep(customWorkout.WarmUp);
        result.Cooldown = ConvertToWorkoutStep(customWorkout.CoolDown);
        
        foreach (var block in customWorkout.Blocks)
        {
            result.IntervalBlocks.Add(ConvertToIntervalBlock(block));
        }

        return result;
    }

    private static WorkoutStep? ConvertToWorkoutStep(Models.WorkoutStep? workoutStep)
    {
        if (workoutStep == null)
        {
            return null;
        }

        return new WorkoutStep
        {
            WorkoutGoal = ConvertToWorkoutGoal(workoutStep.Goal),
            WorkoutAlert = ConvertToWorkoutAlert(workoutStep.Alert),
            DisplayName = workoutStep.DisplayName ?? ""
        };
    }

    private static WorkoutAlert? ConvertToWorkoutAlert(Models.WorkoutAlert? workoutAlert)
    {
        if (workoutAlert == null)
        {
            return null;
        }

        return workoutAlert switch
        {
            Models.HeartRateRangeAlert heartRateRangeAlert => new WorkoutAlert
            {
                AlertMetric = WorkoutAlert.Types.AlertMetricEnum.CountPerMinute,
                Unknown = 2,
                HeartRateRangeAlert = new HeartRateRangeAlert
                {
                    HeartRateRange = new HeartRateRangeAlert.Types.ClosedRange
                    {
                        LowerBound = new HeartRateRangeAlert.Types.WrapDouble
                        {
                            Value = heartRateRangeAlert.LowerBound
                        },
                        UpperBound = new HeartRateRangeAlert.Types.WrapDouble
                        {
                            Value = heartRateRangeAlert.UpperBound
                        }
                    }
                }
            },
            Models.SpeedRangeAlert speedRangeAlert => new WorkoutAlert
            {
                AlertMetric = speedRangeAlert.Metric switch
                {
                    Models.SpeedRangeAlert.AlertMetric.Average => WorkoutAlert.Types.AlertMetricEnum.Average,
                    Models.SpeedRangeAlert.AlertMetric.Current => WorkoutAlert.Types.AlertMetricEnum.Current,
                    _ => throw new ArgumentException("Speed metric must be current or average.")
                },
                Unknown = 2,
                SpeedAlert = new SpeedAlert
                {
                    SpeedRangeAlert = new SpeedAlert.Types.SpeedRangeAlert
                    {
                        LowerBound = new SpeedAlert.Types.SpeedBound
                        {
                            Speed = new SpeedAlert.Types.Speed
                            {
                                Unit = SpeedAlert.Types.Speed.Types.SpeedUnitEnum.MetersPerSecond,
                                Speed_ = CalculateSpeed(speedRangeAlert.MinSpeed, speedRangeAlert.Unit)
                            },
                            Unknown = new SpeedAlert.Types.Unknown_WrapUInt32_Fixed64
                            {
                                First = 1,
                                Second = 1
                            }
                        },
                        UpperBound = new SpeedAlert.Types.SpeedBound
                        {
                            Speed = new SpeedAlert.Types.Speed
                            {
                                Unit = SpeedAlert.Types.Speed.Types.SpeedUnitEnum.MetersPerSecond,
                                Speed_ = CalculateSpeed(speedRangeAlert.MaxSpeed, speedRangeAlert.Unit)
                            },
                            Unknown = new SpeedAlert.Types.Unknown_WrapUInt32_Fixed64
                            {
                                First = 1,
                                Second = 1
                            }
                        }
                    }
                }
            },
            _ => null
        };
    }

    private static double CalculateSpeed(double speed, Models.SpeedRangeAlert.SpeedUnit speedUnit)
    {
        return speedUnit switch
        {
            Models.SpeedRangeAlert.SpeedUnit.MetersPerSecond => speed,
            Models.SpeedRangeAlert.SpeedUnit.KilometersPerHour => speed / 3.6,
            Models.SpeedRangeAlert.SpeedUnit.MilesPerHour => speed / 2.237,
            _ => throw new ArgumentException("Only meters per second, kilometers per hour, miles per hour, and pace are supported now.")
        };
    }


    private static WorkoutGoal ConvertToWorkoutGoal(Models.WorkoutGoal workoutGoal)
    {
        return new WorkoutGoal
        {
            GoalType = workoutGoal switch
            {
                Models.DistanceGoal => GoalType.Distance,
                Models.TimeGoal => GoalType.Time,
                _ => throw new ArgumentException("Only distance and time are supported now.")
            },
            DistanceGoal = workoutGoal switch
            {
                Models.DistanceGoal distanceGoal => new DistanceGoal
                {
                    UnitType = distanceGoal.Unit switch
                    {
                        Models.DistanceGoal.DistanceUnit.Meters => DistanceGoal.Types.DistanceUnitType.Meters,
                        Models.DistanceGoal.DistanceUnit.Kilometers => DistanceGoal.Types.DistanceUnitType.Kilometers,
                        _ => DistanceGoal.Types.DistanceUnitType.Unspecified
                    },
                    UnitValue = distanceGoal.Distance
                },
                _ => null
            },
            TimeGoal = workoutGoal switch
            {
                Models.TimeGoal timeGoal => new TimeGoal
                {
                    UnitType = timeGoal.Time switch
                    {
                        _ when timeGoal.Time.TotalMinutes > 0 => TimeGoal.Types.TimeUnitType.Minutes,
                        _ when timeGoal.Time.TotalMinutes < 0 => TimeGoal.Types.TimeUnitType.Seconds,
                        _ => TimeGoal.Types.TimeUnitType.Unspecified
                    },
                    UnitValue = timeGoal.Time switch
                    {
                        _ when timeGoal.Time.TotalMinutes > 0 => timeGoal.Time.TotalMinutes,
                        _ when timeGoal.Time.TotalSeconds < 0 => timeGoal.Time.TotalSeconds,
                        _ => 0
                    }
                },
                _ => null
            }
        };
    }

    private static IntervalBlock ConvertToIntervalBlock(Models.IntervalBlock intervalBlock)
    {
        var result = new IntervalBlock
        {
            Iterations = (uint)intervalBlock.Iterations
        };

        foreach (var step in intervalBlock.IntervalSteps)
        {
            result.IntervalSteps.Add(new IntervalBlock.Types.IntervalStep
            {
                Purpose = step.Purpose switch
                {
                    Models.IntervalStep.PurposeType.Work => IntervalBlock.Types.IntervalStep.Types.IntervalPurpose.Work,
                    Models.IntervalStep.PurposeType.Recovery => IntervalBlock.Types.IntervalStep.Types.IntervalPurpose.Recovery,
                    _ => IntervalBlock.Types.IntervalStep.Types.IntervalPurpose.Unspecified
                },
                WorkoutStep = ConvertToWorkoutStep(step.WorkoutStep)
            });
        }

        return result;
    }
}