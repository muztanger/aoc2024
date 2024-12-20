namespace Advent_of_Code_2024;

[TestClass]
public class Day20
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var walls = new HashSet<Pos<int>>();
        var start = Pos<int>.Zero;
        var end = Pos<int>.Zero;
        var y = 0;
        foreach (var line in input)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == 'S')
                {
                    start = new Pos<int>(x, y);
                }
                else if (c == 'E')
                {
                    end = new Pos<int>(x, y);
                }
                else if (c == '#')
                {
                    walls.Add(new Pos<int>(x, y));
                }
                x++;
            }
            y++;
        }
        var box = new Box<int>(walls);

        var oldFastest = int.MaxValue;
        {
            var minMap = new DefaultValueDictionary<Pos<int>, int>(() => int.MaxValue);
            var queue = new Queue<(Pos<int>, int)>();
            queue.Enqueue((start, 0));
            var visited = new HashSet<Pos<int>>();
            foreach (var wall in walls) {
                visited.Add(wall);
            }
            while (queue.Any())
            {
                var (pos, steps) = queue.Dequeue();
                if (pos == end)
                {
                    oldFastest = steps;
                    queue.Clear();
                    break;
                }
                if (minMap[pos] <= steps)
                {
                    continue;
                }
                minMap[pos] = steps;
                foreach (var dir in Pos<int>.CardinalDirections)
                {
                    var newPos = pos + dir;
                    if (visited.Contains(newPos))
                    {
                        continue;
                    }
                    visited.Add(newPos);
                    queue.Enqueue((newPos, steps + 1));
                }
            }
        }

        var savings = new DefaultValueDictionary<int, int>(() => int.MaxValue);
        {
            var minMap = new DefaultValueDictionary<(Pos<int>, Pos<int> cheatPos), int>(() => int.MaxValue);
            var queue = new Queue<(Pos<int>, int cheat, Pos<int> cheatPos, int steps)>();
            queue.Enqueue((start, 1, start, 0));
            var visited = new HashSet<(Pos<int>, Pos<int> cheatPos)>();
            while (queue.Any())
            {
                var (pos, cheat, cheatPos, steps) = queue.Dequeue();
                if (pos == end)
                {
                    int saving = oldFastest - steps;
                    if (saving > 0)
                    {
                        savings[saving]++;
                    }
                    continue;
                }
                if (minMap[(pos, cheatPos)] <= steps)
                {
                    continue;
                }
                minMap[(pos, cheatPos)] = steps;
                foreach (var dir in Pos<int>.CardinalDirections)
                {
                    var newPos = pos + dir;
                    if (visited.Contains((newPos, cheatPos)))
                    {
                        continue;
                    }

                    bool inWall = walls.Contains(newPos);
                    if (cheat > 0 && inWall && box.Contains(newPos))
                    {
                        queue.Enqueue((newPos, cheat - 1, newPos, steps + 1));
                        continue;
                    }
                    if (inWall)
                    {
                        continue;
                    }
                    visited.Add((newPos, cheatPos));
                    queue.Enqueue((newPos, cheat, cheatPos, steps + 1));
                }
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
    public void Day20_Part1_Example01()
    {
        var input = """
            ###############
            #...#...#.....#
            #.#.#.#.#.###.#
            #S#...#.#.#...#
            #######.#.#.###
            #######.#.#...#
            #######.#.###.#
            ###..E#...#...#
            ###.#######.###
            #...###...#...#
            #.#####.#.###.#
            #.#...#.#.#...#
            #.#.#.#.#.#.###
            #...#...#...###
            ###############
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day20_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day20_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day20), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day20_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day20_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day20_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day20), "2024"));
        Assert.AreEqual("", result);
    }
    
}
