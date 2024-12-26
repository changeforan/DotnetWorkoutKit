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

## Limitations

- Only support the `CustomWorkout` type.
- Only support outdoor running for now.
- Only support `HeartRateRangeAlert` and `SpeedRangeAlert` for now.

## Using DotnetWorkoutKit

1. Create a `CustomWorkout` and save it as a `.workout` file.

```csharp
using DotnetWorkoutKit.Extensions;
using DotnetWorkoutKit.Models;

var customWorkout = new CustomWorkout(
        CustomWorkout.ActivityType.Running, 
        CustomWorkout.LocationType.Outdoor,
        "E8k Interval",
        new WorkoutStep(new DistanceGoal(2, DistanceGoal.DistanceUnitType.Kilometers), null, "warm up"),
        [
            new IntervalBlock([new (IntervalStep.PurposeType.Work, new (new DistanceGoal(8, DistanceGoal.DistanceUnitType.Kilometers), new HeartRateRangeAlert(144, 153)))], 1),
            new IntervalBlock([
                new (IntervalStep.PurposeType.Work, new (new DistanceGoal(100, DistanceGoal.DistanceUnitType.Meters), new SpeedRangeAlert(11, 12))),
                new (IntervalStep.PurposeType.Recovery, new (new TimeGoal(TimeSpan.FromMinutes(2))))
                ], 8)
        ],
        new WorkoutStep(new DistanceGoal(2, DistanceGoal.DistanceUnitType.Kilometers), new SpeedRangeAlert("5'40\"", "5'20\""), "cool down"));

// Save to file
File.WriteAllBytes("test.workout", customWorkout.DataRepresentation());
```

2. Share the `.workout` file to your iPhone through AirDrop or other methods.

3. You can preview the workout on your iPhone and import it to Apple Watch.

![Preview](https://raw.githubusercontent.com/changeforan/DotnetWorkoutKit/refs/heads/main/IMG_6B672CFD47B3-1.jpeg)

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