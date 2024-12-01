namespace Advent_of_Code_2024;

[TestClass]
public class Day01
{
    private static string Part1(IEnumerable<string> input)
    {
        var leftLocations = new List<int>();
        var rightLocations = new List<int>();
        foreach (var line in input)
        {
            var split = line.SplitTrim(' ');
            leftLocations.Add(int.Parse(split[0]));
            rightLocations.Add(int.Parse(split[1]));
        }

        return leftLocations.OrderBy(x => x)
                    .Zip(rightLocations.OrderBy(y => y))
                    .Sum(z => Math.Abs(z.First - z.Second))
                    .ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var leftLocations = new List<int>();
        var rightLocations = new List<int>();
        var rightCounts = new DefaultValueDictionary<int, int>(() => 0);
        foreach (var line in input)
        {
            var split = line.SplitTrim(' ');
            leftLocations.Add(int.Parse(split[0]));
            int rightLocation = int.Parse(split[1]);
            rightLocations.Add(rightLocation);
            rightCounts[rightLocation] = rightCounts[rightLocation] + 1;
        }

        return leftLocations.Sum(x => x * rightCounts[x]).ToString();
    }
    
    [TestMethod]
    public void Day01_Part1_Example01()
    {
        var input = """
            3   4
            4   3
            2   5
            1   3
            3   9
            3   3
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("11", result);
    }
    
    [TestMethod]
    public void Day01_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day01), "2024"));
        Assert.AreEqual("2166959", result);
    }
    
    [TestMethod]
    public void Day01_Part2_Example01()
    {
        var input = """
            3   4
            4   3
            2   5
            1   3
            3   9
            3   3
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("31", result);
    }
    
    [TestMethod]
    public void Day01_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day01), "2024"));
        Assert.AreEqual("23741109", result);
    }
    
}
