namespace Advent_of_Code_2024;

[TestClass]
public class Day01
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = 0;
        var list1 = new List<int>();
        var list2 = new List<int>();
        foreach (var line in input)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            list1.Add(int.Parse(split[0]));
            list2.Add(int.Parse(split[1]));
        }
        list1.Sort();
        list2.Sort();
        for (int i = 0; i < list1.Count; i++)
        {
            result += Math.Abs(list1[i] - list2[i]);
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = 0;
        var list1 = new List<int>();
        var list2 = new List<int>();
        foreach (var line in input)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            list1.Add(int.Parse(split[0]));
            list2.Add(int.Parse(split[1]));
        }
        for (int i = 0; i < list1.Count; i++)
        {
            result += list1[i] *= list2.Count(x => x == list1[i]);
        }
        return result.ToString();
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
