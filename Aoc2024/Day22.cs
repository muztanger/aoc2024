using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Advent_of_Code_2024;

[TestClass]
public class Day22
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = 0L;
        foreach (var line in input)
        {
            var initialSecretNumber = long.Parse(line);

            long Mix(long value, long secretNumber) => value ^ secretNumber;
            Assert.AreEqual(37, Mix(42, 15));

            long Prune(long secretNumber) => secretNumber & 0xFFFFFF;
            Assert.AreEqual(16113920, Prune(100000000));

            long x = initialSecretNumber;
            for (long i = 0; i < 2000; i++)
            {
                x = Prune(Mix(x << 6, x));
                x = Prune(Mix(x >> 5, x));
                x = Prune(Mix(x << 11, x));
                //Console.WriteLine($"{initialSecretNumber}: {x}");
            }
            result += x;
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = long.MinValue;
        var initialValues = input.Select(long.Parse).ToArray();

        // Brute forcing all possible sequences of 4 numbers...Takes to long to run
        // TODO find all sequences of 4 numbers that exists in the input
        var sequences = new DefaultValueDictionary<string, long[]>(() => [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
        {
            foreach (var initialSecretNumber in initialValues)
            {
                long[] lastFourDiffs = [int.MinValue, int.MinValue, int.MinValue, int.MinValue];
                long lastX = initialSecretNumber;
                long x = initialSecretNumber;
                var diffIndex = 0;
                for (long i = 0; i < 2000; i++)
                {
                    x = ((x << 6) ^ x) & 0xFFFFFF;
                    x = ((x >> 5) ^ x) & 0xFFFFFF;
                    x = ((x << 11) ^ x) & 0xFFFFFF;
                    lastFourDiffs[diffIndex] = x % 10 - lastX % 10; //TODO faster way to get last digit?
                    diffIndex = (diffIndex + 1) & 3;
                    // compare lastFourDiffs with sequence
                    if (i >= 3)
                    {
                        long[] sequence = [
                            lastFourDiffs[(diffIndex + 1) & 3],
                            lastFourDiffs[(diffIndex + 2) & 3],
                            lastFourDiffs[(diffIndex + 3) & 3],
                            lastFourDiffs[diffIndex]];
                        var key = string.Join(',', sequence);
                        sequences[key][x % 10]++;
                    }
                    lastX = x;
                    //Console.WriteLine($"{initialSecretNumber}: {x}");
                }
            }
        }

        foreach (var sequenceString in sequences.Inner.OrderByDescending(kv => {
            var sum = 0L;
            for (int i = 0; i < 10; i++) { 
                sum += kv.Value[i] * i;
            }
            return sum;
        }).Select(kv => kv.Key))
        {
            long[] sequence = sequenceString.Split(',').Select(long.Parse).ToArray();
            long sum = 0L;

            var secretIndex = 0;
            foreach (var initialSecretNumber in initialValues)
            {
                long[] lastFourDiffs = [int.MinValue, int.MinValue, int.MinValue, int.MinValue];
                long lastX = initialSecretNumber;
                long x = initialSecretNumber;
                var diffIndex = 0;
                for (long i = 0; i < 2000; i++)
                {
                    x = ((x << 6) ^ x) & 0xFFFFFF;
                    x = ((x >> 5) ^ x) & 0xFFFFFF;
                    x = ((x << 11) ^ x) & 0xFFFFFF;
                    lastFourDiffs[diffIndex] = x % 10 - lastX % 10; //TODO faster way to get last digit?
                    diffIndex = (diffIndex + 1) & 3;
                    // compare lastFourDiffs with sequence
                    if (i >= 3)
                    {
                        var match = true;
                        for (int j = 0; j < 4; j++)
                        {
                            int k = (diffIndex + j) & 3;
                            if (lastFourDiffs[k] != sequence[j])
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            sum += x % 10;
                            break;
                        }
                    }
                    
                    lastX = x;
                    //Console.WriteLine($"{initialSecretNumber}: {x}");
                }
                if (result > sum + (initialValues.Length - secretIndex - 1) * 9)
                {
                    // No need to continue
                    break;
                }
                secretIndex++;
            }

            result = Math.Max(result, sum);
        }
        return result.ToString();
    }

    [TestMethod]
    public void Day22_Part1_Example01()
    {
        var input = """
            1
            10
            100
            2024
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("37327623", result);
    }
    
    [TestMethod]
    public void Day22_Part1_Example02()
    {
        var input = """
            123
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("1110806", result); // For consistency
    }
    
    [TestMethod]
    public void Day22_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day22), "2024"));
        Assert.AreEqual("17163502021", result);
    }
    
    [TestMethod]
    public void Day22_Part2_Example01()
    {
        var input = """
            1
            2
            3
            2024
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("23", result);
    }
    
    [TestMethod]
    public void Day22_Part2_Example02()
    {
        var input = """
            123
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day22_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day22), "2024"));
        Assert.AreEqual("", result);
    }
    
}
