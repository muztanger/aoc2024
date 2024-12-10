namespace Advent_of_Code_2024;

[TestClass]
public class Day10
{
    class TrailHead
    {
        public required Pos<int> Pos { get; set; }
        public int Score { get; set; }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var grid = new List<List<int>>();
        var trailHeads = new List<TrailHead>();
        foreach (var line in input)
        {
            var row = new List<int>();
            foreach (var c in line)
            {
                int x = c == '.' ? -1 : int.Parse(c.ToString());
                row.Add(x);
                if (x == 0)
                {
                    trailHeads.Add(new TrailHead { Pos = new Pos<int>(row.Count - 1, grid.Count), Score = 0 });
                }
            }
            grid.Add(row);
        }
        var box = new Box<int>(grid[0].Count, grid.Count);

        foreach (var head in trailHeads)
        {
            var stack = new Stack<(Pos<int> pos, int step, Pos<int> dir)>();
            var visited = new HashSet<(Pos<int> pos, Pos<int> dir)>();
            foreach (var dir in Pos<int>.CardinalDirections)
            {
                var newPos = head.Pos + dir;
                var newStep = 1;
                if (box.Contains(newPos))
                {
                    stack.Push((newPos, newStep, dir));
                }
            }
            var scorePoints = new HashSet<Pos<int>>();
            while (stack.Count > 0)
            {
                var (pos, step, dir) = stack.Pop();
                if (visited.Contains((pos, dir)))
                {
                    continue;
                }
                visited.Add((pos, dir));
                if (grid[pos.y][pos.x] != step)
                {
                    continue;
                }
                if (step == 9)
                {
                    scorePoints.Add(pos);
                    continue;
                }
                foreach (var dp in Pos<int>.CardinalDirections)
                {
                    var newPos = pos + dp;
                    var newStep = step + 1;
                    if (box.Contains(newPos))
                    {
                        stack.Push((newPos, newStep, dp));
                    }
                }
            }
            head.Score = scorePoints.Count;
        }

        return trailHeads.Sum(x => x.Score).ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var grid = new List<List<int>>();
        var trailHeads = new List<TrailHead>();
        foreach (var line in input)
        {
            var row = new List<int>();
            foreach (var c in line)
            {
                int x = c == '.' ? -1 : int.Parse(c.ToString());
                row.Add(x);
                if (x == 0)
                {
                    trailHeads.Add(new TrailHead { Pos = new Pos<int>(row.Count - 1, grid.Count), Score = 0 });
                }
            }
            grid.Add(row);
        }
        var box = new Box<int>(grid[0].Count, grid.Count);

        foreach (var head in trailHeads)
        {
            var stack = new Stack<(Pos<int> pos, int step, string path)>();
            var visited = new HashSet<(Pos<int> pos, string path)>();
            foreach (var dir in Pos<int>.CardinalDirections)
            {
                var newPos = head.Pos + dir;
                var newStep = 1;
                var path = head.Pos.ToString() + ";";
                if (box.Contains(newPos))
                {
                    stack.Push((newPos, newStep, path));
                }
            }
            while (stack.Count > 0)
            {
                var (pos, step, path) = stack.Pop();
                if (visited.Contains((pos, path)))
                {
                    continue;
                }
                path += pos.ToString() + ";";
                visited.Add((pos, path));
                if (grid[pos.y][pos.x] != step)
                {
                    continue;
                }
                if (step == 9)
                {
                    head.Score++;
                    continue;
                }
                foreach (var dp in Pos<int>.CardinalDirections)
                {
                    var newPos = pos + dp;
                    var newStep = step + 1;
                    if (box.Contains(newPos))
                    {
                        stack.Push((newPos, newStep, path));
                    }
                }
            }
        }

        return trailHeads.Sum(x => x.Score).ToString();
    }
    
    [TestMethod]
    public void Day10_Part1_Example01()
    {
        var input = """
            89010123
            78121874
            87430965
            96549874
            45678903
            32019012
            01329801
            10456732
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("36", result);
    }
    
    [TestMethod]
    public void Day10_Part1_Example02()
    {
        var input = """
            ...0...
            ...1...
            ...2...
            6543456
            7.....7
            8.....8
            9.....9
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("2", result);
    }

    [TestMethod]
    public void Day10_Part1_Example03()
    {
        var input = """
            ..90..9
            ...1.98
            ...2..7
            6543456
            765.987
            876....
            987....
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("4", result);
    }

    [TestMethod]
    public void Day10_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day10), "2024"));
        Assert.AreEqual("786", result);
    }
    
    [TestMethod]
    public void Day10_Part2_Example01()
    {
        var input = """
            012345
            123456
            234567
            345678
            4.6789
            56789.
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("227", result);
    }
    
    [TestMethod]
    public void Day10_Part2_Example02()
    {
        var input = """
            89010123
            78121874
            87430965
            96549874
            45678903
            32019012
            01329801
            10456732
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("81", result);
    }
    
    [TestMethod]
    public void Day10_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day10), "2024"));
        Assert.AreEqual("1722", result);
    }
    
}
