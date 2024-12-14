namespace Advent_of_Code_2024;

[TestClass]
public class Day13
{
    class ClawMachine
    {
        public required Pos<long> ButtonA;
        public required Pos<long> ButtonB;
        public required Pos<long> Price;

        public bool TryFindMinimumCostToPrize(out long cost)
        {
            // Math!
            // Two equations and two unknowns
            // B = y_p * x_a - x_p * y_a / (x_a * y_b - x_b * y_a)
            // A = (x_p - x_b * B) / x_a

            BigInteger B_upper = BigInteger.Multiply(Price.y, ButtonA.x) - BigInteger.Multiply(Price.x, ButtonA.y);
            BigInteger B_lower = BigInteger.Multiply(ButtonA.x, ButtonB.y) - BigInteger.Multiply(ButtonB.x, ButtonA.y);
            BigInteger.DivRem(B_upper, B_lower, out var B_reminder);
            if (B_reminder != BigInteger.Zero)
            {
                cost = 0;
                return false;
            }

            BigInteger B = B_upper / B_lower;

            BigInteger A_upper = Price.x - BigInteger.Multiply(ButtonB.x, B);
            BigInteger.DivRem(A_upper, ButtonA.x, out var A_reminder);
            if (A_reminder != BigInteger.Zero)
            {
                cost = 0;
                return false;
            }

            BigInteger A = A_upper / ButtonA.x;

            cost = (long)(A * 3 + B * 1);

            return true;
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
        Assert.AreEqual("875318608908", result); // well, I assume this is correct
    }
    
    [TestMethod]
    public void Day13_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day13), "2024"));
        Assert.AreEqual("102255878088512", result);
    }
    
}
