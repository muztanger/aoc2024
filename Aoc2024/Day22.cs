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

            //To mix a value into the secret number, calculate the bitwise XOR of the given value and the secret number.
            //Then, the secret number becomes the result of that operation. (If the secret number is 42 and you were to mix 15 into the secret number, the secret number would become 37.)
            long Mix(long value, long secretNumber) => value ^ secretNumber;

            Assert.AreEqual(37, Mix(42, 15));

            //To prune the secret number, calculate the value of the secret number modulo 16777216.Then, the secret number becomes the result of that operation.
            //(If the secret number is 100000000 and you were to prune the secret number, the secret number would become 16113920.)
            long Prune(long secretNumber) => secretNumber & 0xFFFFFF;
            Assert.AreEqual(16113920, Prune(100000000));

            long x = initialSecretNumber;
            for (long i = 0; i < 2000; i++)
            {
                //Calculate the result of multiplying the secret number by 64.
                //Then, mix this result into the secret number.
                //Finally, prune the secret number.
                x = Prune(Mix(x << 6, x));

                //Calculate the result of dividing the secret number by 32.
                //Round the result down to the nearest integer.
                //Then, mix this result into the secret number.
                //Finally, prune the secret number.
                x = Prune(Mix(x >> 5, x));

                //Calculate the result of multiplying the secret number by 2048.
                //Then, mix this result into the secret number.
                //Finally, prune the secret number.
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
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day22_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day22), "2024"));
        Assert.AreEqual("", result);
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
