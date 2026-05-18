# CLAUDE.md

## Project Overview

DotnetWorkoutKit is a .NET 8.0 library that reverse-engineers Apple's WorkoutKit binary format (`.workout` files) using Protocol Buffers. It enables creating Apple Watch custom workouts from any platform without requiring Xcode or macOS.

## Build & Test Commands

```bash
# Build the library
dotnet build DotnetWorkoutKit

# Run unit tests
dotnet test test/DotnetWorkoutKitTest

# Run tests with Apple binary comparison (macOS only)
# 1. Build the Swift reference tool:
xcodebuild -project tools/WorkoutBinaryGenerator/WorkoutBinaryGenerator.xcodeproj -scheme WorkoutBinaryGenerator -configuration Debug build
# 2. Generate reference binaries:
BUILT_PRODUCTS_DIR=$(xcodebuild -project tools/WorkoutBinaryGenerator/WorkoutBinaryGenerator.xcodeproj -scheme WorkoutBinaryGenerator -configuration Debug -showBuildSettings 2>/dev/null | grep ' BUILT_PRODUCTS_DIR =' | sed 's/.*= //')
"$BUILT_PRODUCTS_DIR/WorkoutBinaryGenerator" /tmp/apple_workouts
# 3. Run tests against Apple output:
APPLE_WORKOUT_DIR=/tmp/apple_workouts dotnet test test/DotnetWorkoutKitTest
```

## Architecture

### Binary Format

The `.workout` file is a Protocol Buffers binary. The proto definitions are in `DotnetWorkoutKit/protobuf/`. C# code is auto-generated from `.proto` files during build (macOS only, via the `GenerateProtoModels` MSBuild target). The generated files live in `DotnetWorkoutKit/protobuf/Models/` and are checked in.

### Key Directories

- `DotnetWorkoutKit/Models/` - Public-facing models (CustomWorkout, goals, alerts)
- `DotnetWorkoutKit/Extensions/DataExtensions.cs` - Core serialization: converts models to protobuf binary and JSON
- `DotnetWorkoutKit/protobuf/` - Proto definitions and auto-generated C# code
- `DotnetWorkoutKit/JsonConverters/` - Custom JSON converters for polymorphic types (WorkoutAlert, WorkoutGoal)
- `WorkoutKit.ConsoleApp/` - CLI for local testing (`generate`, `tojson`, `compare` subcommands that operate on file paths)
- `test/DotnetWorkoutKitTest/` - xUnit v3 tests
- `tools/WorkoutBinaryGenerator/` - Swift/Xcode tool that generates reference `.workout` files using Apple's real WorkoutKit framework

### Data Flow

1. User creates `CustomWorkout` model (or loads from JSON)
2. `DataExtensions.DataRepresentation()` converts model -> protobuf objects -> binary bytes
3. `DataExtensions.JsonRepresentation()` converts model -> JSON string
4. `StringExtensions.LoadFromJson()` converts JSON string -> model

### Binary Structure

The `.workout` binary has this structure:
- Bytes 0-1: protobuf field tag + wire type
- Bytes 2-37: UUID (36 ASCII characters) - unique per serialization, skip in comparisons
- Bytes 38+: workout payload (deterministic for same input)

When comparing binaries, always skip bytes 2-37 (the UUID).

### Speed Unit Conversion

Speed values are stored internally as meters/second. Conversion coefficients match Apple's Foundation UnitSpeed:
- km/h -> m/s: multiply by `0.277778` (Apple's Foundation coefficient, NOT `1/3.6`)
- mph -> m/s: multiply by `0.44704`

### Proto Code Generation

Proto files are compiled to C# only on macOS (via protoc in the build target). The generated `.cs` files in `protobuf/Models/` are committed to the repo so the library builds on all platforms. If you modify a `.proto` file, rebuild on macOS to regenerate.

## Testing

- `DataExtensionsTests.cs` - 33 unit tests with hardcoded expected byte arrays and JSON round-trip verification
- `AppleBinaryComparisonTests.cs` - 33 tests comparing against Apple WorkoutKit output; skipped when `APPLE_WORKOUT_DIR` env var is not set
- CI runs on `macos-15` to enable Apple binary comparison

## Supported Features

### Activity Types (14)
Running, Cycling, Walking, Hiking, Swimming, Elliptical, Rowing, Yoga, CoreTraining, HighIntensityIntervalTraining, FunctionalStrengthTraining, TraditionalStrengthTraining, StairClimbing, CrossTraining

### Goal Types
- `TimeGoal` - duration (stored as minutes internally)
- `DistanceGoal` - distance with unit (Kilometers, Meters, Miles, Feet, Yards)
- `OpenGoal` - no target

### Alert Types
- `HeartRateRangeAlert` - BPM range
- `SpeedRangeAlert` - speed range with unit (MetersPerSecond, KilometersPerHour, MilesPerHour) and metric (Current, Average); also accepts pace strings ("5'00\"")
- `CadenceRangeAlert` - steps/min range
- `PowerRangeAlert` - watts range with metric (Current, Average)
