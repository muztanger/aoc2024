namespace Advent_of_Code_2024;

[TestClass]
public class Day01
{
    private static string Part1(IEnumerable<string> input)
    {
        var list1 = new List<int>();
        var list2 = new List<int>();
        foreach (var line in input)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            list1.Add(int.Parse(split[0]));
            list2.Add(int.Parse(split[1]));
        }

        return list1.OrderBy(x => x)
                    .Zip(list2.OrderBy(y => y))
                    .Select(v => Math.Abs(v.First - v.Second))
                    .Sum()
                    .ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var list1 = new List<int>();
        var list2 = new List<int>();
        var counts = new DefaultValueDictionary<int, int>(() => 0);
        foreach (var line in input)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            list1.Add(int.Parse(split[0]));
            int v2 = int.Parse(split[1]);
            list2.Add(v2);
            counts[v2] = counts[v2] + 1;
        }

        return list1.Sum(x => x * counts[x]).ToString();
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
