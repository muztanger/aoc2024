namespace Advent_of_Code_2024;

[TestClass]
public class Day19
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        foreach (var line in input)
        {
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
    public void Day19_Part1_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day19), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day19), "2024"));
        Assert.AreEqual("", result);
    }
    
}
