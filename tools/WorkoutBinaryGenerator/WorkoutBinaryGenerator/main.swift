import Foundation
import HealthKit
import WorkoutKit

let outputDir = CommandLine.arguments.count > 1
    ? CommandLine.arguments[1]
    : FileManager.default.currentDirectoryPath + "/output"

try FileManager.default.createDirectory(atPath: outputDir, withIntermediateDirectories: true)

func save(_ workout: CustomWorkout, name: String) throws {
    let plan = WorkoutPlan(.custom(workout))
    let data = try plan.dataRepresentation
    try data.write(to: URL(filePath: "\(outputDir)/\(name).workout"))
    print("saved \(name).workout (\(data.count) bytes)")
}

func simpleWorkout(activity: HKWorkoutActivityType, location: HKWorkoutSessionLocationType,
                   displayName: String, goal: WorkoutGoal = .time(10, .minutes),
                   alert: (any WorkoutAlert)? = nil) -> CustomWorkout {
    let step = IntervalStep(.work, goal: goal, alert: alert)
    let block = IntervalBlock(steps: [step], iterations: 1)
    return CustomWorkout(activity: activity, location: location,
                         displayName: displayName, blocks: [block])
}

// MARK: - Activity Types

try save(simpleWorkout(activity: .running, location: .outdoor, displayName: "running"), name: "activity_running")
try save(simpleWorkout(activity: .cycling, location: .outdoor, displayName: "cycling"), name: "activity_cycling")
try save(simpleWorkout(activity: .walking, location: .outdoor, displayName: "walking"), name: "activity_walking")
try save(simpleWorkout(activity: .hiking, location: .outdoor, displayName: "hiking"), name: "activity_hiking")
try save(simpleWorkout(activity: .swimming, location: .indoor, displayName: "swimming"), name: "activity_swimming")
try save(simpleWorkout(activity: .functionalStrengthTraining, location: .indoor, displayName: "strength"), name: "activity_strength")
try save(simpleWorkout(activity: .traditionalStrengthTraining, location: .indoor, displayName: "trad_strength"), name: "activity_trad_strength")
try save(simpleWorkout(activity: .coreTraining, location: .indoor, displayName: "core"), name: "activity_core")
try save(simpleWorkout(activity: .highIntensityIntervalTraining, location: .indoor, displayName: "hiit"), name: "activity_hiit")
try save(simpleWorkout(activity: .yoga, location: .indoor, displayName: "yoga"), name: "activity_yoga")
try save(simpleWorkout(activity: .elliptical, location: .indoor, displayName: "elliptical"), name: "activity_elliptical")
try save(simpleWorkout(activity: .rowing, location: .indoor, displayName: "rowing"), name: "activity_rowing")
try save(simpleWorkout(activity: .wheelchairRunPace, location: .outdoor, displayName: "wheelchair_run"), name: "activity_wheelchair_run")
try save(simpleWorkout(activity: .wheelchairWalkPace, location: .outdoor, displayName: "wheelchair_walk"), name: "activity_wheelchair_walk")

// MARK: - Distance Goals

do {
    let step = IntervalStep(.work, goal: .distance(5, .kilometers))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "dist_km", blocks: [block]),
             name: "goal_dist_km")
}
do {
    let step = IntervalStep(.work, goal: .distance(400, .meters))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "dist_m", blocks: [block]),
             name: "goal_dist_m")
}
do {
    let step = IntervalStep(.work, goal: .distance(3, .miles))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "dist_mi", blocks: [block]),
             name: "goal_dist_mi")
}
do {
    let step = IntervalStep(.work, goal: .distance(1000, .feet))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "dist_ft", blocks: [block]),
             name: "goal_dist_ft")
}
do {
    let step = IntervalStep(.work, goal: .distance(800, .yards))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "dist_yd", blocks: [block]),
             name: "goal_dist_yd")
}

// MARK: - Time Goals

do {
    let step = IntervalStep(.work, goal: .time(90, .seconds))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "time_sec", blocks: [block]),
             name: "goal_time_sec")
}
do {
    let step = IntervalStep(.work, goal: .time(30, .minutes))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "time_min", blocks: [block]),
             name: "goal_time_min")
}
do {
    let step = IntervalStep(.work, goal: .time(1, .hours))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "time_hr", blocks: [block]),
             name: "goal_time_hr")
}

// MARK: - Open Goal

do {
    let step = IntervalStep(.work, goal: .open)
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "open_goal", blocks: [block]),
             name: "goal_open")
}

// MARK: - Heart Rate Alert

do {
    let step = IntervalStep(.work, goal: .distance(5, .kilometers),
                            alert: .heartRate(140...160))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "hr_alert", blocks: [block]),
             name: "alert_hr")
}

// MARK: - Speed Alerts

do {
    let step = IntervalStep(.work, goal: .distance(1, .kilometers),
                            alert: .speed(3.5...4.5, unit: .metersPerSecond, metric: .current))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "spd_ms_cur", blocks: [block]),
             name: "alert_spd_ms_cur")
}
do {
    let step = IntervalStep(.work, goal: .distance(1, .kilometers),
                            alert: .speed(3.5...4.5, unit: .metersPerSecond, metric: .average))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "spd_ms_avg", blocks: [block]),
             name: "alert_spd_ms_avg")
}
do {
    let step = IntervalStep(.work, goal: .distance(1, .kilometers),
                            alert: .speed(10...12, unit: .kilometersPerHour, metric: .current))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "spd_kmh_cur", blocks: [block]),
             name: "alert_spd_kmh_cur")
}
do {
    let step = IntervalStep(.work, goal: .distance(1, .kilometers),
                            alert: .speed(10...12, unit: .kilometersPerHour, metric: .average))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "spd_kmh_avg", blocks: [block]),
             name: "alert_spd_kmh_avg")
}
do {
    let step = IntervalStep(.work, goal: .distance(1, .miles),
                            alert: .speed(6...8, unit: .milesPerHour, metric: .current))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "spd_mph_cur", blocks: [block]),
             name: "alert_spd_mph_cur")
}
do {
    let step = IntervalStep(.work, goal: .distance(1, .miles),
                            alert: .speed(6...8, unit: .milesPerHour, metric: .average))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "spd_mph_avg", blocks: [block]),
             name: "alert_spd_mph_avg")
}

// MARK: - Cadence Alert

do {
    let step = IntervalStep(.work, goal: .distance(1, .kilometers),
                            alert: .cadence(170...185))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "cadence", blocks: [block]),
             name: "alert_cadence")
}

// MARK: - Power Alert

do {
    let step = IntervalStep(.work, goal: .time(20, .minutes),
                            alert: .power(200...250, unit: .watts, metric: .current))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .cycling, location: .outdoor,
                           displayName: "power_cur", blocks: [block]),
             name: "alert_power_cur")
}
do {
    let step = IntervalStep(.work, goal: .time(20, .minutes),
                            alert: .power(200...250, unit: .watts, metric: .average))
    let block = IntervalBlock(steps: [step], iterations: 1)
    try save(CustomWorkout(activity: .cycling, location: .outdoor,
                           displayName: "power_avg", blocks: [block]),
             name: "alert_power_avg")
}

// MARK: - Warmup + Cooldown

do {
    let step = IntervalStep(.work, goal: .distance(5, .kilometers),
                            alert: .heartRate(150...165))
    let block = IntervalBlock(steps: [step], iterations: 1)
    let warmup = WorkoutStep(goal: .distance(1, .kilometers), alert: .heartRate(130...145))
    let cooldown = WorkoutStep(goal: .time(5, .minutes))
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "warmup_cool",
                           warmup: warmup, blocks: [block], cooldown: cooldown),
             name: "warmup_cooldown")
}

// MARK: - Multiple Blocks + Iterations

do {
    let workStep = IntervalStep(.work, goal: .distance(400, .meters),
                                alert: .speed(3.8...4.2, unit: .metersPerSecond, metric: .current))
    let restStep = IntervalStep(.recovery, goal: .time(90, .seconds))
    var block = IntervalBlock(steps: [workStep, restStep])
    block.iterations = 6
    try save(CustomWorkout(activity: .running, location: .outdoor,
                           displayName: "intervals", blocks: [block]),
             name: "multi_intervals")
}

print("\nAll workouts saved to: \(outputDir)")
