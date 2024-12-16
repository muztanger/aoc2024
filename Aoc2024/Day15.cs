using static System.Net.Mime.MediaTypeNames;

namespace Advent_of_Code_2024;

[TestClass]
public class Day15
{
    private static string Part1(IEnumerable<string> input)
    {
        var moveToDir = new Dictionary<char, Pos<int>>
        {
            ['^'] = Pos<int>.North,
            ['v'] = Pos<int>.South,
            ['<'] = Pos<int>.West,
            ['>'] = Pos<int>.East,
        };
        var moves = new List<char>();
        var robot = Pos<int>.Zero;
        var grid = new List<List<char>>();
        var walls = new HashSet<Pos<int>>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.Contains('<') || line.Contains('>') || line.Contains('^') || line.Contains('v'))
            {
                moves.AddRange(line);
            }
            else
            {
                if (line.Contains('@'))
                {
                    robot = new Pos<int>(line.IndexOf('@'), grid.Count);
                }
                var col = 0;
                foreach (var c in line)
                {
                    if (c == '#')
                    {
                        walls.Add(new Pos<int>(col, grid.Count));
                    }
                    col++;
                }
                grid.Add(line.ToCharArray().ToList());
            }
        }
        var box = new Box<int>(grid[0].Count, grid.Count);

        void PrintGrid()
        {
            var lines = new StringBuilder();
            for (int y = 0; y < grid.Count; y++)
            {
                for (int x = 0; x < grid[y].Count; x++)
                {
                    lines.Append(grid[y][x]);
                }
                lines.AppendLine();
            }
            var lineString = lines.ToString();
            Console.WriteLine(lineString);
        }

        PrintGrid();
        foreach (var move in moves)
        {
            Console.WriteLine($"Move: {move}, robot: {robot}");

            // search for '.' in the direction of the robot, stop search at walls
            var next = robot + moveToDir[move];
            var stack = new Stack<Pos<int>>();
            //stack.Push(robot);
            var foundSpace = false;
            while (box.Contains(next))
            {
                if (walls.Contains(next))
                {
                    break;
                }
                if (grid[next.y][next.x] == '.')
                {
                    stack.Push(new Pos<int>(next));
                    foundSpace = true;
                    break;
                }
                else if (grid[next.y][next.x] == 'O')
                {
                    stack.Push(new Pos<int>(next));
                }
                else
                {
                    Assert.Fail($"Unexpected character: {grid[next.y][next.x]}");
                }
                next += moveToDir[move];
            }
            if (foundSpace)
            {
                robot += moveToDir[move];
                while (stack.Any())
                {
                    var pos = stack.Pop();
                    var last = pos - moveToDir[move];
                    (grid[last.y][last.x], grid[pos.y][pos.x]) = (grid[pos.y][pos.x], grid[last.y][last.x]);
                }
            }

            PrintGrid();
        }
        var result = 0;
        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[y].Count; x++)
            {
                if (grid[y][x] == 'O')
                {
                    result += y * 100 + x;
                }
            }
        }
        return result.ToString();
    }

    private static string Part2(IEnumerable<string> input)
    {
        var moveToDir = new Dictionary<char, Pos<int>>
        {
            ['^'] = Pos<int>.North,
            ['v'] = Pos<int>.South,
            ['<'] = Pos<int>.West,
            ['>'] = Pos<int>.East,
        };
        var replace = new Dictionary<char, string>
        {
            ['#'] = "##",
            ['O'] = "[]",
            ['.'] = "..",
            ['@'] = "@.",
        };
        var moves = new List<char>();
        var robot = Pos<int>.Zero;
        var grid = new List<List<char>>();
        var walls = new HashSet<Pos<int>>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.Contains('<') || line.Contains('>') || line.Contains('^') || line.Contains('v'))
            {
                moves.AddRange(line);
            }
            else
            {
                if (line.Contains('@'))
                {
                    robot = new Pos<int>(line.IndexOf('@') * 2, grid.Count);
                }
                var col = 0;
                foreach (var c in line)
                {
                    if (c == '#')
                    {
                        walls.Add(new Pos<int>(col * 2, grid.Count));
                        walls.Add(new Pos<int>(col * 2 + 1, grid.Count));
                    }
                    col++;
                }
                grid.Add(string.Concat(line.Select(c => replace[c])).ToCharArray().ToList());
            }
        }
        var box = new Box<int>(grid[0].Count, grid.Count);

        void PrintGrid()
        {
            var lines = new StringBuilder();
            for (int y = 0; y < grid.Count; y++)
            {
                for (int x = 0; x < grid[y].Count; x++)
                {
                    lines.Append(grid[y][x]);
                }
                lines.AppendLine();
            }
            var lineString = lines.ToString();
            Console.WriteLine(lineString);
        }

        PrintGrid();
        foreach (var move in moves)
        {
            Console.WriteLine($"Robot: {robot}");
            Console.WriteLine($"Move {move}");
            var dir = moveToDir[move];
            if (TryMove(robot, dir, grid, box, walls, out var movedPoss))
            {
                robot += dir;
                foreach (var pos in movedPoss.OrderBy(p => dir.y > 0 ? -p.y : p.y).ThenBy(p => dir.x > 0 ? -p.x : p.x))
                {
                    var next = pos + dir;
                    (grid[next.y][next.x], grid[pos.y][pos.x]) = (grid[pos.y][pos.x], grid[next.y][next.x]);
                }
            }

            PrintGrid();
            Assert.IsTrue(grid[robot.y][robot.x] == '@');
        }
        var result = 0;
        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[y].Count; x++)
            {
                if (grid[y][x] == '[')
                {
                    result += y * 100 + x;
                }
            }
        }
        return result.ToString();
    }

    private static bool TryMove(Pos<int> start, Pos<int> dir, List<List<char>> grid, Box<int> gridBox, HashSet<Pos<int>> walls, out HashSet<Pos<int>> movedPoss)
    {
        var next = start + dir;
        movedPoss = new HashSet<Pos<int>>() { start };
        while (gridBox.Contains(next))
        {
            char c = grid[next.y][next.x];
            if (walls.Contains(next))
            {
                return false;
            }
            if (c == '.')
            {
                return true;
            }
            else if (c == 'O')
            {
                movedPoss.Add(new Pos<int>(next));
            }
            else if (c == '[')
            {
                movedPoss.Add(new Pos<int>(next));
                if (dir == Pos<int>.North || dir == Pos<int>.South)
                {
                    if (!TryMove(next + Pos<int>.East, dir, grid, gridBox, walls, out var newMovedPoss))
                    {
                        return false;
                    }
                    movedPoss.UnionWith(newMovedPoss);
                }
            }
            else if (c == ']')
            {
                movedPoss.Add(new Pos<int>(next));
                if (dir == Pos<int>.North || dir == Pos<int>.South)
                {
                    if (!TryMove(next + Pos<int>.West, dir, grid, gridBox, walls, out var newMovedPoss))
                    {
                        return false;
                    }
                    movedPoss.UnionWith(newMovedPoss);
                }
            }
            else if (c == '@')
            {
                movedPoss.Add(new Pos<int>(next));
            }
            else
            {
                Assert.Fail($"Unexpected character: {grid[next.y][next.x]}");
            }
            next += dir;
        }
        return false;
    }

    [TestMethod]
    public void Day15_Part1_Example01()
    {
        var input = """
            ##########
            #..O..O.O#
            #......O.#
            #.OO..O.O#
            #..O@..O.#
            #O#..O...#
            #O..O..O.#
            #.OO.O.OO#
            #....O...#
            ##########

            <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
            vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
            ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
            <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
            ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
            ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
            >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
            <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
            ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
            v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("10092", result);
    }
    
    [TestMethod]
    public void Day15_Part1_Example02()
    {
        var input = """
            ########
            #..O.O.#
            ##@.O..#
            #...O..#
            #.#.O..#
            #...O..#
            #......#
            ########

            <^^>>>vv<v>>v<<
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("2028", result);
    }
    
    [TestMethod]
    public void Day15_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day15), "2024"));
        Assert.AreEqual("1509863", result);
    }
    
    [TestMethod]
    public void Day15_Part2_Example01()
    {
        var input = """
            ##########
            #..O..O.O#
            #......O.#
            #.OO..O.O#
            #..O@..O.#
            #O#..O...#
            #O..O..O.#
            #.OO.O.OO#
            #....O...#
            ##########

            <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
            vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
            ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
            <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
            ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
            ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
            >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
            <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
            ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
            v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("9021", result);
    }
    
    [TestMethod]
    public void Day15_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day15), "2024"));
        Assert.AreEqual("1548815", result);
    }
    
}
