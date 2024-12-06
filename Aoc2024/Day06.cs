namespace Advent_of_Code_2024;

[TestClass]
public class Day06
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var y = 0;
        var start = new Pos<int>(0, 0);
        var box = new Box<int>(start);
        var obstructions = new List<Pos<int>>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;
            var x = 0;
            foreach (var c in line)
            {
                if (c == '#')
                {
                    box.IncreaseToPoint(new Pos<int>(x, y));
                    obstructions.Add(new Pos<int>(x, y));
                }
                else if (c == '^')
                {
                    start = new Pos<int>(x, y);
                }
                x++;
            }
            y++;
        }
        var directionIndex = 3;
        var pos = start;
        var traversed = new List<Pos<int>>() { pos };
        while (box.Contains(pos))
        {
            var nextPos = pos + Pos<int>.CardinalDirections[directionIndex];
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
        var y = 0;
        var start = new Pos<int>(0, 0);
        var box = new Box<int>(start);
        var obstructions = new HashSet<Pos<int>>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line)) continue;
            var x = 0;
            foreach (var c in line)
            {
                if (c == '#')
                {
                    box.IncreaseToPoint(new Pos<int>(x, y));
                    obstructions.Add(new Pos<int>(x, y));
                }
                else if (c == '^')
                {
                    start = new Pos<int>(x, y);
                }
                x++;
            }
            y++;
        }
        foreach (var newObstruction in box.GetPositions())
        {
            if (obstructions.Contains(newObstruction) || start == newObstruction)
            {
                continue;
            }

            var directionIndex = 3;
            var pos = start;
            var traversed = new HashSet<(Pos<int>, Pos<int>)>() {(pos, Pos<int>.CardinalDirections[directionIndex]) };
            while (box.Contains(pos))
            {
                var nextPos = pos + Pos<int>.CardinalDirections[directionIndex];
                if (!box.Contains(nextPos))
                {
                    break;
                }

                if (traversed.Contains((nextPos, Pos<int>.CardinalDirections[directionIndex])))
                {
                    result++;
                    break;
                }

                if (obstructions.Contains(nextPos) || newObstruction == nextPos)
                {
                    directionIndex = (directionIndex + 1) % Pos<int>.CardinalDirections.Count;
                }
                else
                {
                    pos = nextPos;
                    traversed.Add((pos, Pos<int>.CardinalDirections[directionIndex]));
                }
            }

        }
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
