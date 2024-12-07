namespace Advent_of_Code_2024;

[TestClass]
public class Day07
{
    private static string Part1(IEnumerable<string> input)
    {
        var operations = new Dictionary<char, Func<long, long, long>>
        {
            ['+'] = (a, b) => a + b,
            ['*'] = (a, b) => a * b,
        };
        var result = 0L;
        foreach (var line in input)
        {
            var split = line.Split(": ");
            var answer = long.Parse(split[0]);
            var numbers = split[1].SplitTrim(" ").Select(long.Parse).ToList();

            var stack = new Stack<string>();
            stack.Push("+");
            stack.Push("*");
            var visited = new HashSet<string>();
            while (stack.Any())
            {
                var current = stack.Pop();
                if (visited.Contains(current))
                {
                    continue;
                }
                visited.Add(current);

                if (current.Length == numbers.Count)
                {
                    var sum = numbers[0];
                    for (var i = 1; i < numbers.Count; i++)
                    {
                        sum = operations[current[i - 1]](sum, numbers[i]);
                    }
                    if (sum == answer)
                    {
                        result += answer;
                        break;
                    }
                    continue;
                }

                stack.Push(current + "+");
                stack.Push(current + "*");
            }
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var operations = new Dictionary<char, Func<long, long, long>>
        {
            ['+'] = (a, b) => a + b,
            ['*'] = (a, b) => a * b,
        };
        var result = 0L;
        foreach (var line in input)
        {
            var split = line.Split(": ");
            var answer = long.Parse(split[0]);
            var numbers = split[1].SplitTrim(" ").Select(long.Parse).ToList();

            var stack = new Stack<string>();
            stack.Push("+");
            stack.Push("*");
            var visited = new HashSet<string>();
            while (stack.Any())
            {
                var current = stack.Pop();
                if (visited.Contains(current))
                {
                    continue;
                }
                visited.Add(current);

                if (current.Length == numbers.Count)
                {
                    var sum = numbers[0];
                    for (var i = 1; i < numbers.Count; i++)
                    {
                        sum = operations[current[i - 1]](sum, numbers[i]);
                    }
                    if (sum == answer)
                    {
                        result += answer;
                        break;
                    }
                    continue;
                }

                stack.Push(current + "+");
                stack.Push(current + "*");
            }
        }
        return result.ToString();
    }

    [TestMethod]
    public void Day07_Part1_Example01()
    {
        var input = """
            190: 10 19
            3267: 81 40 27
            83: 17 5
            156: 15 6
            7290: 6 8 6 15
            161011: 16 10 13
            192: 17 8 14
            21037: 9 7 18 13
            292: 11 6 16 20
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day07), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day07), "2024"));
        Assert.AreEqual("", result);
    }
    
}
