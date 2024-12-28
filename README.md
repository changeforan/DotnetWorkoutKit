# DotnetWorkoutKit

## What is DotnetWorkoutKit?

DotnetWorkoutKit is a cross-platform .NET library that provides a way to create custom workout files for Apple Watch.

## Goals

Apple announced the library [WorkoutKit](https://developer.apple.com/documentation/workoutkit) in WWDC 2023, which allows developers to create and preview workouts in your iOS and watchOS apps. Refer to the [WWDC23 session 10016](https://developer.apple.com/videos/play/wwdc2023/10016/)(timecode: 09:43), the `.workout` file can be in both binary and JSON format.

However, Apple changed their decision, when the `WorkoutKit` was officially released, the `.workout` file is in binary format only. See this [reply](https://developer.apple.com/forums/thread/750557?answerId=790229022#790229022) for more information. This means that developers can't create custom workouts outside of Xcode and MacOS.

So I decoded the binary format of the `.workout` file and created this library to provide a way to create custom workout files.

## Features

- Create a `CustomWorkout` with C# code.
- Save `CustomWorkout` as a `.workout` file, which can be previewed on your iPhone and imported to Apple Watch.
- Save `CustomWorkout` as a JSON file, which can be shared with others.
- Load a `CustomWorkout` from a JSON file.

## Limitations

- Only support the `CustomWorkout` type.
- Only support outdoor running for now.
- Only support `HeartRateRangeAlert` and `SpeedRangeAlert` for now.

## Using DotnetWorkoutKit

1. Create a `CustomWorkout` or load it from a JSON file.

```csharp
// Create a custom workout
var customWorkout = new CustomWorkout(
        activity: CustomWorkout.ActivityType.Running, 
        location: CustomWorkout.LocationType.Outdoor,
        displayName: "sample",
        warmUp: new WorkoutStep(new DistanceGoal(3, DistanceGoal.DistanceUnit.Kilometers), new HeartRateRangeAlert(144, 153), "Warm Up"),
        blocks: [
            new IntervalBlock([
                new (IntervalStep.PurposeType.Work, new (new DistanceGoal(3, DistanceGoal.DistanceUnit.Kilometers), new SpeedRangeAlert("4'46\"", "4'38\""))), // The speed can be defined as pace.
                new (IntervalStep.PurposeType.Recovery, new (new TimeGoal(TimeSpan.FromMinutes(2))))
                ], 2),
            new IntervalBlock([
                new (IntervalStep.PurposeType.Work, new (new DistanceGoal(200, DistanceGoal.DistanceUnit.Meters), new SpeedRangeAlert("4'09\"", "3'59\""))),
                new (IntervalStep.PurposeType.Recovery, new (new DistanceGoal(200, DistanceGoal.DistanceUnit.Meters)))
                ], 6)
        ],
        coolDown: new WorkoutStep(new DistanceGoal(3, DistanceGoal.DistanceUnit.Kilometers), new HeartRateRangeAlert(144, 153), "Cool Down"));

// Or load from JSON
var json = """
{
  "Activity": "Running",
  "Location": "Outdoor",
  "DisplayName": "simple",
  "WarmUp": {
    "Goal": {
      "Distance": 3,
      "Unit": "Kilometers"
    },
    "Alert": {
      "MinSpeed": "5'40\"",
      "MaxSpeed": "5'00\""
    },
    "DisplayName": "Warm Up"
  },
  "Blocks": []
}
"""

var customWorkout = json.LoadFromJson();
```

> You can find the example in the project `WorkoutKit.ConsoleApp` which is for local testing.

2. Save the `CustomWorkout` as a `.workout` file or a JSON file.

```csharp
// Save as JSON
File.WriteAllText($"{customWorkout.DisplayName}.workout.json", customWorkout.JsonRepresentation());

// Save as binary
File.WriteAllBytes($"{customWorkout.DisplayName}.workout", customWorkout.DataRepresentation());
```

3. Share the `.workout` file to your iPhone through AirDrop or other methods.

4. You can preview the workout on your iPhone and import it to Apple Watch.

<img src="https://raw.githubusercontent.com/changeforan/DotnetWorkoutKit/refs/heads/main/IMG_6B672CFD47B3-2.jpeg" alt="Import" style="zoom:50%;" />

## License

```text
MIT License

Copyright (C) 2013-2024 .NET Foundation and Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
```