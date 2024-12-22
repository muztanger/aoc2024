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
        var result = new StringBuilder();
        foreach (var line in input)
        {
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
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day22_Part2_Example02()
    {
        var input = """
            <TODO>
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
