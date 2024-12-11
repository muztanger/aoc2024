namespace Advent_of_Code_2024;

[TestClass]
public class Day11
{
    class Stone
    {
        public long Number { get; set; }
        public Stone? Left { get; set; }
        public Stone? Right { get; set; }

        public override string ToString() => $"{Left?.Number}<{Number}>{Right?.Number}";
    }

    private static string Part1(IEnumerable<string> input)
    {
        var stones = input.First().Split(' ').Select(x => new Stone { Number = long.Parse(x)}).ToArray();
        for (var i = 0; i < stones.Length; i++)
        {
            if (i > 0) stones[i].Left = stones[i - 1];
            if (i < stones.Length - 1) stones[i].Right = stones[i + 1];
        }

        string StoneString()
        {
            var result = new StringBuilder();
            var stone = stones.First();
            while (stone is not null)
            {
                if (result.Length != 0)
                {
                    result.Append(' ');
                }
                result.Append(stone.Number);
                stone = stone.Right;
            }
            return result.ToString();
        }

        int StoneCount()
        {
            var count = 0;
            var stone = stones.First();
            while (stone is not null)
            {
                count++;
                stone = stone.Right;
            }
            return count;
        }

        void Blink()
        {
            var stone = stones.First();

            while (stone is not null)
            {
                // 0 changes to 1
                if (stone.Number == 0)
                {
                    stone.Number = 1;
                    stone = stone.Right;
                    continue;
                }

                // even number of digits -> "first half of digits", "second half of digits" (leeding zeros are removed)
                var digits = stone.Number.ToString().ToCharArray();
                if (digits.Length % 2 == 0)
                {
                    var half = digits.Length / 2;
                    var firstHalf = long.Parse(new string(digits.AsSpan(0, half)));
                    var secondHalf = long.Parse(new string(digits.AsSpan(half)));
                    stone.Number = firstHalf;

                    var newStone = new Stone { Number = secondHalf, Left = stone, Right = stone.Right };
                    if (stone.Right is not null)
                    {
                        stone.Right.Left = newStone;
                    }
                    stone.Right = newStone;

                    stone = newStone.Right;
                    continue;
                }
                stone.Number *= 2024;
                stone = stone.Right;
            }
        }

        for (var i = 0; i < 25; i++)
        {
            Blink();
        }

        return StoneCount().ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var stones = input.First().Split(' ').Select(x => new Stone { Number = long.Parse(x) }).ToArray();
        for (var i = 0; i < stones.Length; i++)
        {
            if (i > 0) stones[i].Left = stones[i - 1];
            if (i < stones.Length - 1) stones[i].Right = stones[i + 1];
        }

        string StoneString()
        {
            var result = new StringBuilder();
            var stone = stones.First();
            while (stone is not null)
            {
                if (result.Length != 0)
                {
                    result.Append(' ');
                }
                result.Append(stone.Number);
                stone = stone.Right;
            }
            return result.ToString();
        }

        int StoneCount()
        {
            var count = 0;
            var stone = stones.First();
            while (stone is not null)
            {
                count++;
                stone = stone.Right;
            }
            return count;
        }

        void Blink()
        {
            var stone = stones.First();

            while (stone is not null)
            {
                // 0 changes to 1
                if (stone.Number == 0)
                {
                    stone.Number = 1;
                    stone = stone.Right;
                    continue;
                }

                // even number of digits -> "first half of digits", "second half of digits" (leeding zeros are removed)
                var digits = stone.Number.ToString().ToCharArray();
                if (digits.Length % 2 == 0)
                {
                    var half = digits.Length / 2;
                    var firstHalf = long.Parse(new string(digits.AsSpan(0, half)));
                    var secondHalf = long.Parse(new string(digits.AsSpan(half)));
                    stone.Number = firstHalf;

                    var newStone = new Stone { Number = secondHalf, Left = stone, Right = stone.Right };
                    if (stone.Right is not null)
                    {
                        stone.Right.Left = newStone;
                    }
                    stone.Right = newStone;

                    stone = newStone.Right;
                    continue;
                }
                stone.Number *= 2024;
                stone = stone.Right;
            }
        }

        for (var i = 0; i < 75; i++)
        {
            Blink();
        }

        return StoneCount().ToString();
    }
    
    [TestMethod]
    public void Day11_Part1_Example01()
    {
        var input = """
            0 1 10 99 999
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
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
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day11_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day11_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day11_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day11), "2024"));
        Assert.AreEqual("", result);
    }
    
}
