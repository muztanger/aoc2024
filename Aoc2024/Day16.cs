using System.Security.Cryptography;

namespace Advent_of_Code_2024;

[TestClass]
public class Day16
{
    private static string Part1(IEnumerable<string> input)
    {
        return LowestScore(input).ToString();
    }

    private static int LowestScore(IEnumerable<string> input)
    {
        var grid = new List<List<char>>();
        var start = new Pos<int>(int.MinValue, int.MinValue);
        var end = new Pos<int>(int.MinValue, int.MinValue);
        foreach (var line in input)
        {
            grid.Add(line.ToList());
            if (line.Contains('S'))
            {
                start = new Pos<int>(line.IndexOf('S'), grid.Count - 1);
            }
            if (line.Contains('E'))
            {
                end = new Pos<int>(line.IndexOf('E'), grid.Count - 1);
            }
        }
        var box = new Box<int>(grid[0].Count, grid.Count);

        var result = int.MaxValue;
        var visited = new HashSet<(Pos<int> pos, int cost)>();
        var queue = new PriorityQueue<(Pos<int> pos, int dir, int cost), int>();
        queue.Enqueue((start, 0, 0), 0);
        while (queue.Count > 0)
        {
            var (pos, dir, cost) = queue.Dequeue();
            if (cost >= result)
            {
                continue;
            }
            if (pos == end)
            {
                result = Math.Min(result, cost);
            }
            if (visited.Contains((pos, cost)))
            {
                continue;
            }
            visited.Add((pos, cost));

            {
                // turn left
                var nextDir = (dir + 3) % 4;
                var left = pos + Pos<int>.CardinalDirections[nextDir];
                if (box.Contains(left) && grid[left.y][left.x] != '#')
                {
                    int newCost = cost + 1000 + 1;
                    queue.Enqueue((left, nextDir, newCost), newCost);
                }
            }
            {
                // turn right
                var nextDir = (dir + 1) % 4;
                var right = pos + Pos<int>.CardinalDirections[nextDir];
                if (box.Contains(right) && grid[right.y][right.x] != '#')
                {
                    int newCost = cost + 1000 + 1;
                    queue.Enqueue((right, nextDir, newCost), newCost);
                }
            }
            {
                // move forward
                var forward = pos + Pos<int>.CardinalDirections[dir];
                if (box.Contains(forward) && grid[forward.y][forward.x] != '#')
                {
                    int newCost = cost + 1;
                    queue.Enqueue((forward, dir, newCost), newCost);
                }
            }
        }

        return result;
    }

    private static string ComputeHash(string rawData)
    {
        using (var hash = SHA1.Create())
        {
            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private static string Part2(IEnumerable<string> input)
    {
        int bestPathScore = LowestScore(input); //TODO reuse parse etc.

        var grid = new List<List<char>>();
        var start = new Pos<int>(int.MinValue, int.MinValue);
        var end = new Pos<int>(int.MinValue, int.MinValue);
        foreach (var line in input)
        {
            grid.Add(line.ToList());
            if (line.Contains('S'))
            {
                start = new Pos<int>(line.IndexOf('S'), grid.Count - 1);
            }
            if (line.Contains('E'))
            {
                end = new Pos<int>(line.IndexOf('E'), grid.Count - 1);
            }
        }
        var box = new Box<int>(grid[0].Count, grid.Count);

        var result = new HashSet<Pos<int>>();
        var visited = new HashSet<string>();
        var stack = new Stack<(Pos<int> pos, int dir, int cost, List<Pos<int>> path)>();
        stack.Push((start, 0, 0, new List<Pos<int>> { start }));
        while (stack.Count > 0)
        {
            var (pos, dir, cost, path) = stack.Pop();
            if (cost > bestPathScore)
            {
                continue;
            }
            if (pos == end)
            {
                path.ForEach(p => result.Add(p));
            }
            string pathString = ComputeHash(string.Concat(path));
            if (visited.Contains(pathString))
            {
                continue;
            }
            visited.Add(pathString);

            {
                // turn left
                var nextDir = (dir + 3) % 4;
                var left = pos + Pos<int>.CardinalDirections[nextDir];
                if (box.Contains(left) && grid[left.y][left.x] != '#' && !path.Contains(left))
                {
                    int newCost = cost + 1000 + 1;
                    var newPath = new List<Pos<int>>(path) { left };
                    stack.Push((left, nextDir, newCost, newPath));
                }
            }
            {
                // turn right
                var nextDir = (dir + 1) % 4;
                var right = pos + Pos<int>.CardinalDirections[nextDir];
                if (box.Contains(right) && grid[right.y][right.x] != '#' && !path.Contains(right))
                {
                    int newCost = cost + 1000 + 1;
                    var newPath = new List<Pos<int>>(path) { right };
                    stack.Push((right, nextDir, newCost, newPath));
                }
            }
            {
                // move forward
                var forward = pos + Pos<int>.CardinalDirections[dir];
                if (box.Contains(forward) && grid[forward.y][forward.x] != '#' && !path.Contains(forward))
                {
                    int newCost = cost + 1;
                    var newPath = new List<Pos<int>>(path) { forward };
                    stack.Push((forward, dir, newCost, newPath));
                }
            }
        }
        return result.Count.ToString();
    }
    
    [TestMethod]
    public void Day16_Part1_Example01()
    {
        var input = """
            ###############
            #.......#....E#
            #.#.###.#.###.#
            #.....#.#...#.#
            #.###.#####.#.#
            #.#.#.......#.#
            #.#.#####.###.#
            #...........#.#
            ###.#.#####.#.#
            #...#.....#.#.#
            #.#.#.###.#.#.#
            #.....#...#.#.#
            #.###.#.#.#.#.#
            #S..#.....#...#
            ###############
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("7036", result);
    }
    
    [TestMethod]
    public void Day16_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day16_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day16), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day16_Part2_Example01()
    {
        var input = """
            ###############
            #.......#....E#
            #.#.###.#.###.#
            #.....#.#...#.#
            #.###.#####.#.#
            #.#.#.......#.#
            #.#.#####.###.#
            #...........#.#
            ###.#.#####.#.#
            #...#.....#.#.#
            #.#.#.###.#.#.#
            #.....#...#.#.#
            #.###.#.#.#.#.#
            #S..#.....#...#
            ###############
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("45", result);
    }
    
    [TestMethod]
    public void Day16_Part2_Example02()
    {
        var input = """
            #################
            #...#...#...#..E#
            #.#.#.#.#.#.#.#.#
            #.#.#.#...#...#.#
            #.#.#.#.###.#.#.#
            #...#.#.#.....#.#
            #.#.#.#.#.#####.#
            #.#...#.#.#.....#
            #.#.#####.#.###.#
            #.#.#.......#...#
            #.#.###.#####.###
            #.#.#...#.....#.#
            #.#.#.#####.###.#
            #.#.#.........#.#
            #.#.#.#########.#
            #S#.............#
            #################
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("64", result);
    }
    
    [TestMethod]
    public void Day16_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day16), "2024"));
        Assert.AreEqual("", result);
    }
    
}
