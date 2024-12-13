namespace Advent_of_Code_2024;

[TestClass]
public class Day13
{
    class ClawMachine
    {
        public Pos<long> ButtonA;
        public Pos<long> ButtonB;
        public Pos<long> Price;

        public bool TryFindMinimumCostToPrize(out long cost)
        {
            cost = long.MaxValue;
            Pos<long> start = new(0, 0);
            var box = new Box<long>(start, Price);
            var visited = new HashSet<(Pos<long> pos, long cost)>();


            var queue = new PriorityQueue<(Pos<long> pos, long cost), long>();
            queue.Enqueue((start, 0), 0);
            var isFound = false;
            while (queue.Count > 0)
            {
                var (pos, currentCost) = queue.Dequeue();
                if (pos == Price)
                {
                    isFound = true;
                    cost = Math.Min(cost, currentCost);
                    continue;
                }

                if (visited.Contains((pos, currentCost))) continue;
                visited.Add((pos, currentCost));

                if (currentCost >= cost) continue;

                if (box.Contains(pos + ButtonA))
                {
                    queue.Enqueue((pos + ButtonA, currentCost + 3), currentCost + 3);
                }

                if (box.Contains(pos + ButtonB))
                {
                    queue.Enqueue((pos + ButtonB, currentCost + 1), currentCost + 1);
                }
            }

            return isFound;
        }

        override public string ToString() => $"ButtonA: {ButtonA}, ButtonB: {ButtonB}, Price: {Price}";
    }
    private static string Part1(IEnumerable<string> input)
    {
        var buttonA = new Pos<long>(0, 0);
        var buttonB = new Pos<long>(0, 0);
        var price = new Pos<long>(0, 0);

        var machines = new List<ClawMachine>();
        var result = new StringBuilder();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;
            var parts = line.Split(": ");
            if (line.StartsWith("Button A"))
            {
                var match = Regex.Match(parts[1], @"X([+-]\d+), Y([+-]\d+)");
                Assert.IsTrue(match.Success);
                buttonA = new Pos<long>(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value));
            } 
            else if (line.StartsWith("Button B"))
            {
                var match = Regex.Match(parts[1], @"X([+-]\d+), Y([+-]\d+)");
                Assert.IsTrue(match.Success);
                buttonB = new Pos<long>(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value));
            }
            else if (line.StartsWith("Prize"))
            {
                var match = Regex.Match(parts[1], @"X=(\d+), Y=(\d+)");
                Assert.IsTrue(match.Success);
                price = new Pos<long>(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value));
                machines.Add(new ClawMachine() { ButtonA = buttonA, ButtonB = buttonB, Price = price });
            }
        }

        Console.WriteLine(string.Join(Environment.NewLine, machines.Select(m => m.ToString())));

        return machines.Select(m => m.TryFindMinimumCostToPrize(out var cost) ? cost : 0).Sum().ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var buttonA = new Pos<long>(0, 0);
        var buttonB = new Pos<long>(0, 0);
        var price = new Pos<long>(0, 0);

        var machines = new List<ClawMachine>();
        var result = new StringBuilder();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;
            var parts = line.Split(": ");
            if (line.StartsWith("Button A"))
            {
                var match = Regex.Match(parts[1], @"X([+-]\d+), Y([+-]\d+)");
                Assert.IsTrue(match.Success);
                buttonA = new Pos<long>(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value));
            }
            else if (line.StartsWith("Button B"))
            {
                var match = Regex.Match(parts[1], @"X([+-]\d+), Y([+-]\d+)");
                Assert.IsTrue(match.Success);
                buttonB = new Pos<long>(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value));
            }
            else if (line.StartsWith("Prize"))
            {
                var match = Regex.Match(parts[1], @"X=(\d+), Y=(\d+)");
                Assert.IsTrue(match.Success);
                var more = new Pos<long>(10000000000000, 10000000000000);

                price = new Pos<long>(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value));
                machines.Add(new ClawMachine() { ButtonA = buttonA, ButtonB = buttonB, Price = price + more});
            }
        }

        Console.WriteLine(string.Join(Environment.NewLine, machines.Select(m => m.ToString())));

        return machines.Select(m => m.TryFindMinimumCostToPrize(out var cost) ? cost : 0).Sum().ToString();

    }

    [TestMethod]
    public void Day13_Part1_Example01()
    {
        var input = """
            Button A: X+94, Y+34
            Button B: X+22, Y+67
            Prize: X=8400, Y=5400

            Button A: X+26, Y+66
            Button B: X+67, Y+21
            Prize: X=12748, Y=12176

            Button A: X+17, Y+86
            Button B: X+84, Y+37
            Prize: X=7870, Y=6450

            Button A: X+69, Y+23
            Button B: X+27, Y+71
            Prize: X=18641, Y=10279
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("480", result);
    }
    
    [TestMethod]
    public void Day13_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day13_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day13), "2024"));
        Assert.AreEqual("28059", result);
    }
    
    [TestMethod]
    public void Day13_Part2_Example01()
    {
        var input = """
            Button A: X+94, Y+34
            Button B: X+22, Y+67
            Prize: X=8400, Y=5400
            
            Button A: X+26, Y+66
            Button B: X+67, Y+21
            Prize: X=12748, Y=12176
            
            Button A: X+17, Y+86
            Button B: X+84, Y+37
            Prize: X=7870, Y=6450
            
            Button A: X+69, Y+23
            Button B: X+27, Y+71
            Prize: X=18641, Y=10279
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day13_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day13_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day13), "2024"));
        Assert.AreEqual("", result);
    }
    
}
