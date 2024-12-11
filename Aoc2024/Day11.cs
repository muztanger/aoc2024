namespace Advent_of_Code_2024;

[TestClass]
public class Day11
{
    private static string Part1(IEnumerable<string> input)
    {
        return Blinks(input, 25);
    }

    private static string Part2(IEnumerable<string> input)
    {
        return Blinks(input, 75);
    }

    private static string Blinks(IEnumerable<string> input, int N)
    {
        var stones = new Dictionary<long, long>();
        foreach (var s in input.First().Split(' ').Select(long.Parse))
        {
            stones.TryGetValue(s, out var count);
            stones[s] = count + 1;
        }

        void Blink()
        {
            var nextStones = new Dictionary<long, long>();
            foreach (var (stone, count) in stones)
            {
                // 0 changes to 1
                if (stone == 0)
                {
                    nextStones.TryGetValue(1, out var x);
                    nextStones[1] = x + count;
                    continue;
                }

                // even number of digits -> "first half of digits", "second half of digits" (leeding zeros are removed)
                var digits = stone.ToString().ToCharArray();
                if (digits.Length % 2 == 0)
                {
                    var half = digits.Length / 2;
                    var firstHalf = long.Parse(new string(digits.AsSpan(0, half)));
                    var secondHalf = long.Parse(new string(digits.AsSpan(half)));

                    {
                        nextStones.TryGetValue(firstHalf, out var x);
                        nextStones[firstHalf] = x + count;
                    }
                    {
                        nextStones.TryGetValue(secondHalf, out var x);
                        nextStones[secondHalf] = x + count;
                    }
                    continue;
                }

                // else multiply by 2024
                {
                    nextStones.TryGetValue(stone * 2024, out var x);
                    nextStones[stone * 2024] = x + count;
                }
            }

            stones = nextStones;
        }

        for (var i = 0; i < N; i++)
        {
            Blink();
        }

        return stones.Sum(kv => kv.Value).ToString();
    }
    
    [TestMethod]
    public void Day11_Part1_Example01()
    {
        var input = """
            0 1 10 99 999
            """;
        var result = Blinks(Common.GetLines(input), 1);
        Assert.AreEqual("7", result);
    }
    
    [TestMethod]
    public void Day11_Part1_Example02()
    {
        var input = """
            125 17
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("55312", result);
    }
    
    [TestMethod]
    public void Day11_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day11), "2024"));
        Assert.AreEqual("220999", result);
    }
    
    [TestMethod]
    public void Day11_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day11), "2024"));
        Assert.AreEqual("261936432123724", result);
    }
    
}
