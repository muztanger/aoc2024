namespace Advent_of_Code_2024;

[TestClass]
public class Day19
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = 0;

        var patterns = input.Where(line => line.Contains(",")).First().SplitTrim(",");
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            if (line.Contains(","))
            {
                continue;
            }

            var design = line;
            var valid = false;
            var stack = new Stack<string>();
            stack.Push(design);
            while (stack.Count > 0)
            {
                design = stack.Pop();
                if (design.Length == 0)
                {
                    valid = true;
                    break;
                }
                foreach (var pattern in patterns)
                {
                    if (design.StartsWith(pattern))
                    {
                        stack.Push(design.Substring(pattern.Length));
                    }
                }
            }
            if (valid) result++;
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = BigInteger.Zero;

        var patterns = input.Where(line => line.Contains(",")).First().SplitTrim(",");
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            if (line.Contains(","))
            {
                continue;
            }

            var memo = new Dictionary<string, BigInteger>();
            bool CountCombinations(string str, out BigInteger count)
            {
                if (memo.TryGetValue(str, out count))
                {
                    return true;
                }
                count = 0;
                if (str.Length == 0)
                {
                    return true;
                }
                var isFound = false;
                foreach (var pattern in patterns)
                {
                    if (str == pattern)
                    {
                        count++;
                        isFound = true;
                        continue;
                    }
                    if (str.StartsWith(pattern) && CountCombinations(str.Substring(pattern.Length), out var subCount))
                    {
                        count += subCount;
                        isFound = true;
                    }
                }
                if (!isFound)
                {
                    return false;
                }
                memo[str] = count;
                return true;
            }
            if (CountCombinations(line, out var totalCount))
            {
                result += totalCount;
            }
        }
        return result.ToString();
    }
    
    [TestMethod]
    public void Day19_Part1_Example01()
    {
        var input = """
            r, wr, b, g, bwu, rb, gb, br

            brwrr
            bggr
            gbbr
            rrbgbr
            ubwu
            bwurrg
            brgr
            bbrgwb
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
            r, wr, b, g, bwu, rb, gb, br

            brwrr
            bggr
            gbbr
            rrbgbr
            ubwu
            bwurrg
            brgr
            bbrgwb
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
        Assert.AreNotEqual("57181", result);
        // 468381424464337 too low
        Assert.AreEqual("", result);
    }
    
}
