using DotnetWorkoutKit.Models;

namespace WorkoutKit.WebApp.Models;

public class WorkoutModel
{
    public string? DisplayName { get; set; }
    public CustomWorkout.ActivityType Activity { get; set; } = CustomWorkout.ActivityType.Running;
    public CustomWorkout.LocationType Location { get; set; } = CustomWorkout.LocationType.Outdoor;
    public bool HasWarmUp { get; set; }
    public StepModel WarmUp { get; set; } = new();
    public bool HasCoolDown { get; set; }
    public StepModel CoolDown { get; set; } = new();
    public List<BlockModel> Blocks { get; set; } = new() { new BlockModel() };

    public CustomWorkout ToCustomWorkout()
    {
        var warm = HasWarmUp ? WarmUp.ToWorkoutStep() : null;
        var cool = HasCoolDown ? CoolDown.ToWorkoutStep() : null;
        var blocks = Blocks.Select(b => b.ToIntervalBlock()).ToArray();
        return new CustomWorkout(Activity, Location, string.IsNullOrWhiteSpace(DisplayName) ? null : DisplayName, warm, blocks, cool);
    }
}

public class BlockModel
{
    public int Iterations { get; set; } = 1;
    public List<IntervalStepModel> Steps { get; set; } = new() { new IntervalStepModel() };

    public IntervalBlock ToIntervalBlock()
        => new(Steps.Select(s => s.ToIntervalStep()).ToArray(), Iterations);
}

public class IntervalStepModel
{
    public IntervalStep.PurposeType Purpose { get; set; } = IntervalStep.PurposeType.Work;
    public StepModel Step { get; set; } = new();

    public IntervalStep ToIntervalStep() => new(Purpose, Step.ToWorkoutStep());
}

public class StepModel
{
    public string? DisplayName { get; set; }
    public GoalModel Goal { get; set; } = new();
    public AlertModel Alert { get; set; } = new();

    public WorkoutStep ToWorkoutStep()
    {
        var alert = Alert.Kind == AlertKind.None ? null : Alert.ToAlert();
        return new WorkoutStep(Goal.ToGoal(), alert, string.IsNullOrWhiteSpace(DisplayName) ? null : DisplayName);
    }
}

public enum GoalKind { Open, Time, Distance }

public class GoalModel
{
    public GoalKind Kind { get; set; } = GoalKind.Open;
    public int TimeMinutes { get; set; } = 5;
    public int TimeSeconds { get; set; } = 0;
    public double Distance { get; set; } = 1.0;
    public DistanceGoal.DistanceUnit DistanceUnit { get; set; } = DistanceGoal.DistanceUnit.Kilometers;

    public WorkoutGoal ToGoal() => Kind switch
    {
        GoalKind.Time => new TimeGoal(new TimeSpan(0, TimeMinutes, TimeSeconds)),
        GoalKind.Distance => new DistanceGoal(Distance, DistanceUnit),
        _ => new OpenGoal(),
    };
}

public enum AlertKind { None, HeartRate, Speed, Cadence, Power }

public class AlertModel
{
    public AlertKind Kind { get; set; } = AlertKind.None;

    public int HrLower { get; set; } = 120;
    public int HrUpper { get; set; } = 150;

    public double SpeedMin { get; set; } = 8;
    public double SpeedMax { get; set; } = 12;
    public SpeedRangeAlert.SpeedUnit SpeedUnit { get; set; } = SpeedRangeAlert.SpeedUnit.KilometersPerHour;
    public SpeedRangeAlert.AlertMetric SpeedMetric { get; set; } = SpeedRangeAlert.AlertMetric.Current;

    public int CadenceMin { get; set; } = 70;
    public int CadenceMax { get; set; } = 90;

    public double PowerMin { get; set; } = 150;
    public double PowerMax { get; set; } = 250;
    public PowerRangeAlert.PowerMetric PowerMetric { get; set; } = PowerRangeAlert.PowerMetric.Current;

    public WorkoutAlert ToAlert() => Kind switch
    {
        AlertKind.HeartRate => new HeartRateRangeAlert(HrLower, HrUpper),
        AlertKind.Speed => new SpeedRangeAlert(SpeedMin, SpeedMax, SpeedUnit, SpeedMetric),
        AlertKind.Cadence => new CadenceRangeAlert(CadenceMin, CadenceMax),
        AlertKind.Power => new PowerRangeAlert(PowerMin, PowerMax, PowerRangeAlert.PowerUnit.Watts, PowerMetric),
        _ => throw new InvalidOperationException("No alert"),
    };
}
