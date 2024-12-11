using Aoc2024.Commons;

namespace Advent_of_Code_2024;

[TestClass]
public class Day06
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        byte y = 0;
        var start = new Pos<byte>(0, 0);
        var box = new Box<byte>(start);
        var obstructions = new List<Pos<byte>>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;
            byte x = 0;
            foreach (var c in line)
            {
                if (c == '#')
                {
                    box.IncreaseToPoint(new Pos<byte>(x, y));
                    obstructions.Add(new Pos<byte>(x, y));
                }
                else if (c == '^')
                {
                    start = new Pos<byte>(x, y);
                }
                x++;
            }
            y++;
        }
        var directionIndex = 3;
        var pos = start;
        var traversed = new List<Pos<byte>>() { pos };
        while (box.Contains(pos))
        {
            var nextPos = pos + Pos<byte>.CardinalDirections[directionIndex];
            if (!box.Contains(nextPos))
            {
                break;
            }

            if (obstructions.Contains(nextPos))
            {
                directionIndex = (directionIndex + 1) % Pos<int>.CardinalDirections.Count;
            }
            else
            {
                pos = nextPos;
                traversed.Add(pos);
            }
        }
        return traversed.Select(x => x).Distinct().Count().ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = 0;
        byte y = 0;
        var start = new Pos<byte>(0, 0);
        var box = new Box<byte>(start);
        var obstructions = new HashSet<Pos<byte>>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;
            byte x = 0;
            foreach (var c in line)
            {
                if (c == '#')
                {
                    box.IncreaseToPoint(new Pos<byte>(x, y));
                    obstructions.Add(new Pos<byte>(x, y));
                }
                else if (c == '^')
                {
                    start = new Pos<byte>(x, y);
                }
                x++;
            }
            y++;
        }
        var profiler = new Profiler();
        profiler.Start();
        var visited = new HashSet<(byte, byte, byte)>() {};
        foreach (var newObstruction in box.GetPositions())
        {
            if (obstructions.Contains(newObstruction) || start == newObstruction)
            {
                continue;
            }

            byte directionIndex = 3;
            var pos = start;
            visited.Clear();
            visited.Add((pos.x, pos.y, directionIndex));
            (byte, byte, byte) nextVisit = (0, 0, 0);
            while (box.Contains(pos))
            {
                var nextPos2d = pos + Pos<byte>.CardinalDirections[directionIndex];
                nextVisit = (nextPos2d.x, nextPos2d.y, directionIndex);
                if (!box.Contains(nextVisit))
                {
                    break;
                }

                if (visited.Contains(nextVisit))
                {
                    result++;
                    break;
                }

                if (obstructions.Contains(nextPos2d) || newObstruction == nextPos2d)
                {
                    directionIndex = (byte) ( (directionIndex + 1) % Pos<byte>.CardinalDirections.Count);
                }
                else
                {
                    pos = nextPos2d;
                    visited.Add(nextVisit);
                }
            }
        }
        profiler.Stop();
        profiler.Print();

        // TODO: Pretty absurd memory allocation
        // 
        // Inital implementation
        //   Elapsed: 15705ms
        //   Allocated memory: 8,032,079.74 kb
        // 
        // Using HashSet.Clear() instead of creating a new HashSet
        //   Elapsed: 12631ms
        //   Allocated memory: 1,774,714.77 kb
        //
        // Using Pos3 instead of (pos, direction)
        //   Elapsed: 13848ms
        //   Allocated memory: 4,138,966.30 kb
        //
        // Using tuple (pos.x, pos.y directionIndex) instead of (Pos, Pos) for visited
        //   Elapsed: 11553ms (on battery)
        //   Allocated memory: 1,774,233.66 kb
        //
        // Using bytes for state instead of ints and HashSet
        //   Elapsed: 12938ms
        //   Allocated memory: 1,774,030.96 kb

        return result.ToString();

    }

    [TestMethod]
    public void Day06_Part1_Example01()
    {
        var input = """
            ....#.....
            .........#
            ..........
            ..#.......
            .......#..
            ..........
            .#..^.....
            ........#.
            #.........
            ......#...
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("41", result);
    }
    
    [TestMethod]
    public void Day06_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day06), "2024"));
        Assert.AreEqual("4656", result);
    }
    
    [TestMethod]
    public void Day06_Part2_Example01()
    {
        var input = """
            ....#.....
            .........#
            ..........
            ..#.......
            .......#..
            ..........
            .#..^.....
            ........#.
            #.........
            ......#...
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("6", result);
    }
    
    [TestMethod]
    public void Day06_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day06), "2024"));
        Assert.AreEqual("1575", result);
    }
    
}
