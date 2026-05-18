using DotnetWorkoutKit.Extensions;
using DotnetWorkoutKit.Models;

return args switch
{
    ["generate", var input, var output] => Generate(input, output),
    ["tojson", var input, var output] => ToJson(input, output),
    ["compare", var a, var b] => Compare(a, b),
    _ => Usage()
};

static int Usage()
{
    Console.Error.WriteLine("""
        Usage:
          WorkoutKit.ConsoleApp generate <input.json> <output.workout>
              Serialize a workout JSON file to an Apple .workout binary.

          WorkoutKit.ConsoleApp tojson <input.json> <output.json>
              Round-trip a workout JSON file (deserialize then re-serialize).

          WorkoutKit.ConsoleApp compare <a.workout> <b.workout>
              Compare two .workout binaries (skipping the UUID bytes 2-37).
        """);
    return 1;
}

static int Generate(string inputPath, string outputPath)
{
    if (!File.Exists(inputPath))
    {
        Console.Error.WriteLine($"Input file not found: {inputPath}");
        return 1;
    }

    var json = File.ReadAllText(inputPath);
    var workout = json.LoadFromJson();
    if (workout == null)
    {
        Console.Error.WriteLine("Failed to deserialize workout JSON.");
        return 1;
    }

    var bytes = workout.DataRepresentation();
    File.WriteAllBytes(outputPath, bytes);
    Console.WriteLine($"Wrote {bytes.Length} bytes to {outputPath}");
    return 0;
}

static int ToJson(string inputPath, string outputPath)
{
    if (!File.Exists(inputPath))
    {
        Console.Error.WriteLine($"Input file not found: {inputPath}");
        return 1;
    }

    var json = File.ReadAllText(inputPath);
    var workout = json.LoadFromJson();
    if (workout == null)
    {
        Console.Error.WriteLine("Failed to deserialize workout JSON.");
        return 1;
    }

    File.WriteAllText(outputPath, workout.JsonRepresentation());
    Console.WriteLine($"Wrote JSON to {outputPath}");
    return 0;
}

static int Compare(string pathA, string pathB)
{
    if (!File.Exists(pathA))
    {
        Console.Error.WriteLine($"File not found: {pathA}");
        return 1;
    }
    if (!File.Exists(pathB))
    {
        Console.Error.WriteLine($"File not found: {pathB}");
        return 1;
    }

    var a = File.ReadAllBytes(pathA);
    var b = File.ReadAllBytes(pathB);

    if (a.Length <= 38 || b.Length <= 38)
    {
        Console.WriteLine($"a={a.Length}b b={b.Length}b match=false (file too small)");
        return 1;
    }

    var match = a.Length == b.Length && a[38..].SequenceEqual(b[38..]);
    Console.WriteLine($"a={a.Length}b b={b.Length}b match={match}");

    if (!match)
    {
        var min = Math.Min(a.Length, b.Length);
        for (var i = 38; i < min; i++)
        {
            if (a[i] != b[i])
            {
                Console.WriteLine($"First diff at byte {i}: a=0x{a[i]:X2} b=0x{b[i]:X2}");
                break;
            }
        }
        return 1;
    }
    return 0;
}
