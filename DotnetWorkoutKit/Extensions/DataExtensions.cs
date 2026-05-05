using System.Text.Json;
using System.Text.Json.Serialization;
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
            CustomWorkout = ConvertToCustomWorkout(customWorkout),
            Version = 1,
            Format = 5
        };

        return workoutBin.ToByteArray();
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
                Models.CustomWorkout.ActivityType.CrossTraining => CustomWorkout.Types.ActivityType.CrossTraining,
                Models.CustomWorkout.ActivityType.Cycling => CustomWorkout.Types.ActivityType.Cycling,
                Models.CustomWorkout.ActivityType.Elliptical => CustomWorkout.Types.ActivityType.Elliptical,
                Models.CustomWorkout.ActivityType.FunctionalStrengthTraining => CustomWorkout.Types.ActivityType.FunctionalStrengthTraining,
                Models.CustomWorkout.ActivityType.Hiking => CustomWorkout.Types.ActivityType.Hiking,
                Models.CustomWorkout.ActivityType.Rowing => CustomWorkout.Types.ActivityType.Rowing,
                Models.CustomWorkout.ActivityType.Running => CustomWorkout.Types.ActivityType.Running,
                Models.CustomWorkout.ActivityType.StairClimbing => CustomWorkout.Types.ActivityType.StairClimbing,
                Models.CustomWorkout.ActivityType.Swimming => CustomWorkout.Types.ActivityType.Swimming,
                Models.CustomWorkout.ActivityType.TraditionalStrengthTraining => CustomWorkout.Types.ActivityType.TraditionalStrengthTraining,
                Models.CustomWorkout.ActivityType.Walking => CustomWorkout.Types.ActivityType.Walking,
                Models.CustomWorkout.ActivityType.Yoga => CustomWorkout.Types.ActivityType.Yoga,
                Models.CustomWorkout.ActivityType.CoreTraining => CustomWorkout.Types.ActivityType.CoreTraining,
                Models.CustomWorkout.ActivityType.HighIntensityIntervalTraining => CustomWorkout.Types.ActivityType.HighIntensityIntervalTraining,
                _ => throw new ArgumentException($"Unsupported activity type: {customWorkout.Activity}")
            },
            LocationType = customWorkout.Location switch
            {
                Models.CustomWorkout.LocationType.Indoor => CustomWorkout.Types.LocationType.Indoor,
                Models.CustomWorkout.LocationType.Outdoor => CustomWorkout.Types.LocationType.Outdoor,
                _ => throw new ArgumentException($"Unsupported location type: {customWorkout.Location}")
            },
        };

        if (customWorkout.DisplayName != null)
            result.DisplayName = customWorkout.DisplayName;

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

        var step = new WorkoutStep
        {
            WorkoutGoal = ConvertToWorkoutGoal(workoutStep.Goal),
            WorkoutAlert = ConvertToWorkoutAlert(workoutStep.Alert),
        };

        if (workoutStep.DisplayName != null)
            step.DisplayName = workoutStep.DisplayName;

        return step;
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
                            TimeUnit = new SpeedAlert.Types.TimeUnit
                            {
                                Unit = 1,
                                Value = 1
                            }
                        },
                        UpperBound = new SpeedAlert.Types.SpeedBound
                        {
                            Speed = new SpeedAlert.Types.Speed
                            {
                                Unit = SpeedAlert.Types.Speed.Types.SpeedUnitEnum.MetersPerSecond,
                                Speed_ = CalculateSpeed(speedRangeAlert.MaxSpeed, speedRangeAlert.Unit)
                            },
                            TimeUnit = new SpeedAlert.Types.TimeUnit
                            {
                                Unit = 1,
                                Value = 1
                            }
                        }
                    }
                }
            },
            Models.CadenceRangeAlert cadenceRangeAlert => new WorkoutAlert
            {
                AlertMetric = WorkoutAlert.Types.AlertMetricEnum.Cadence,
                Unknown = 2,
                CadenceAlert = new CadenceAlert
                {
                    CadenceRangeAlert = new CadenceAlert.Types.CadenceRangeAlert
                    {
                        LowerBound = new CadenceAlert.Types.CadenceBound
                        {
                            Cadence = (uint)cadenceRangeAlert.MinCadence,
                            TimeUnit = new CadenceAlert.Types.TimeUnit { Unit = 2, Value = 1 }
                        },
                        UpperBound = new CadenceAlert.Types.CadenceBound
                        {
                            Cadence = (uint)cadenceRangeAlert.MaxCadence,
                            TimeUnit = new CadenceAlert.Types.TimeUnit { Unit = 2, Value = 1 }
                        }
                    }
                }
            },
            Models.PowerRangeAlert powerRangeAlert => new WorkoutAlert
            {
                AlertMetric = powerRangeAlert.Metric switch
                {
                    Models.PowerRangeAlert.PowerMetric.Current => WorkoutAlert.Types.AlertMetricEnum.PowerCurrent,
                    Models.PowerRangeAlert.PowerMetric.Average => WorkoutAlert.Types.AlertMetricEnum.PowerAverage,
                    _ => throw new ArgumentException("Power metric must be current or average.")
                },
                Unknown = 2,
                PowerAlert = new PowerAlert
                {
                    PowerRangeAlert = new PowerAlert.Types.PowerRangeAlert
                    {
                        LowerBound = new PowerAlert.Types.PowerBound
                        {
                            Unit = 1,
                            Power = powerRangeAlert.MinPower
                        },
                        UpperBound = new PowerAlert.Types.PowerBound
                        {
                            Unit = 1,
                            Power = powerRangeAlert.MaxPower
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
            Models.SpeedRangeAlert.SpeedUnit.KilometersPerHour => speed * 0.277778,
            Models.SpeedRangeAlert.SpeedUnit.MilesPerHour => speed * 0.44704,
            _ => throw new ArgumentException($"Unsupported speed unit: {speedUnit}")
        };
    }


    private static WorkoutGoal ConvertToWorkoutGoal(Models.WorkoutGoal workoutGoal)
    {
        return workoutGoal switch
        {
            Models.OpenGoal => new WorkoutGoal
            {
                GoalType = GoalType.Open
            },
            Models.DistanceGoal distanceGoal => new WorkoutGoal
            {
                GoalType = GoalType.Distance,
                DistanceGoal = new DistanceGoal
                {
                    UnitType = distanceGoal.Unit switch
                    {
                        Models.DistanceGoal.DistanceUnit.Meters => DistanceGoal.Types.DistanceUnitType.Meters,
                        Models.DistanceGoal.DistanceUnit.Kilometers => DistanceGoal.Types.DistanceUnitType.Kilometers,
                        Models.DistanceGoal.DistanceUnit.Feet => DistanceGoal.Types.DistanceUnitType.Feet,
                        Models.DistanceGoal.DistanceUnit.Yards => DistanceGoal.Types.DistanceUnitType.Yards,
                        Models.DistanceGoal.DistanceUnit.Miles => DistanceGoal.Types.DistanceUnitType.Miles,
                        _ => DistanceGoal.Types.DistanceUnitType.Unspecified
                    },
                    UnitValue = distanceGoal.Distance
                }
            },
            Models.TimeGoal timeGoal => new WorkoutGoal
            {
                GoalType = GoalType.Time,
                TimeGoal = ConvertTimeGoal(timeGoal)
            },
            _ => throw new ArgumentException($"Unsupported goal type: {workoutGoal.GetType().Name}")
        };
    }

    private static TimeGoal ConvertTimeGoal(Models.TimeGoal timeGoal)
    {
        if (timeGoal.Time.TotalHours >= 1 && IsWholeNumber(timeGoal.Time.TotalHours))
        {
            return new TimeGoal
            {
                UnitType = TimeGoal.Types.TimeUnitType.Hours,
                UnitValue = timeGoal.Time.TotalHours
            };
        }
        if (timeGoal.Time.TotalMinutes >= 1 && IsWholeNumber(timeGoal.Time.TotalMinutes))
        {
            return new TimeGoal
            {
                UnitType = TimeGoal.Types.TimeUnitType.Minutes,
                UnitValue = timeGoal.Time.TotalMinutes
            };
        }
        return new TimeGoal
        {
            UnitType = TimeGoal.Types.TimeUnitType.Seconds,
            UnitValue = timeGoal.Time.TotalSeconds
        };
    }

    private static bool IsWholeNumber(double value) =>
        Math.Abs(value - Math.Round(value)) < 1e-9;

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
