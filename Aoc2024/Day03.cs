namespace Advent_of_Code_2024;

[TestClass]
public class Day03
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = 0;
        foreach (var line in input)
        {
           var matches = Regex.Matches(line, @"mul\(([0-9]+), *([0-9]+)\)");
           foreach (Match match in matches)
            {
                var a = int.Parse(match.Groups[1].Value);
                var b = int.Parse(match.Groups[2].Value);
                result += a * b;
            }
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = 0;
        var isMultipy = true;
        foreach (var line in input)
        {
            var matches = Regex.Matches(line, @"mul\(([0-9]+), *([0-9]+)\)|do\(\)|don't\(\)");
            foreach (Match match in matches)
            {
                if (match.Groups[0].Value == "do()")
                {
                    isMultipy = true;
                    continue;
                }
                if (match.Groups[0].Value == "don't()")
                {
                    isMultipy = false;
                    continue;
                }
                if (!isMultipy)
                {
                    continue;
                }
                var a = int.Parse(match.Groups[1].Value);
                var b = int.Parse(match.Groups[2].Value);
                result += a * b;
            }
        }
        return result.ToString();
    }
    
    [TestMethod]
    public void Day03_Part1_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day03_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day03_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day03), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day03_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day03_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day03_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day03), "2024"));
        Assert.AreEqual("", result);
    }
    
}
