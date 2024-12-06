using System.ComponentModel.DataAnnotations;

namespace Advent_of_Code_2024;

[TestClass]
public class Day05
{
    class Rule
    {
        int Left { init; get; }
        int Right { init; get; }

        internal static Rule Parse(string line)
        {
            var split = line.Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var result = new Rule()
            {
                Left = int.Parse(split[0]),
                Right = int.Parse(split[1])
            };
            return result;
        }

    }

    class Update
    {
        List<int> Pages { init; get; } = new();

        internal static Update Parse(string line)
        {
            var split = line.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var result = new Update()
            {
                Pages = split.Select(int.Parse).ToList()
            };
            return result;
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var rules = new List<Rule>();
        var updates = new List<Update>();
        var isRules = true;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line) && updates.Any())
            {
                isRules = false;
                continue;
            }
            if (isRules)
            {
                var rule = Rule.Parse(line);
                rules.Add(rule);
            }
            else
            {
                var update = Update.Parse(line);
                updates.Add(update);
            }
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
    public void Day05_Part1_Example01()
    {
        var input = """
            47|53
            97|13
            97|61
            97|47
            75|29
            61|13
            75|53
            29|13
            97|29
            53|29
            61|53
            97|53
            61|29
            47|13
            75|47
            97|75
            47|61
            75|61
            47|29
            75|13
            53|13

            75,47,61,53,29
            97,61,53,29,13
            75,29,13
            75,97,47,61,53
            61,13,29
            97,13,75,29,47
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day05), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day05), "2024"));
        Assert.AreEqual("", result);
    }
    
}
