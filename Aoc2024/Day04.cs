using System.Runtime.Intrinsics.Arm;

namespace Advent_of_Code_2024;

[TestClass]
public class Day04
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = 0;
        var board = new List<string>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            board.Add(line);
        }
        var box = new Box<int>(new Pos<int>(0, 0), new Pos<int>(board[0].Length - 1, board.Count - 1));
        const string xmas = "XMAS";
        var directions = new List<Pos<int>> {new (1, 0), new (1, 1), new (0, 1), new (-1, 1), new (-1, 0), new (-1, -1), new (0, -1), new (1, -1)};
        for (int y = 0; y < box.Height; y++)
        {
            for (int x = 0; x < box.Width; x++)
            {
                foreach (var dir in directions)
                {
                    var pos = new Pos<int>(x, y);
                    var found = true;
                    for (int i = 0; i < xmas.Length; i++)
                    {
                        if (!box.Contains(pos) || board[pos.y][pos.x] != xmas[i])
                        {
                            found = false;
                            break;
                        }

                        pos += dir;
                    }
                    if (found)
                    {
                        result++;
                    }
                }
            }
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = 0;
        var board = new List<string>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            board.Add(line);
        }
        var box = new Box<int>(new Pos<int>(0, 0), new Pos<int>(board[0].Length - 1, board.Count - 1));
        const string mas = "MAS";
        const string test = "MS";
        var directions = new List<Pos<int>> { new(1, 1), new(-1, 1), new(-1, -1), new(1, -1) };

        for (int y = 0; y < box.Height; y++)
        {
            for (int x = 0; x < box.Width; x++)
            {
                if (board[y][x] != 'A')
                {
                    continue;
                }
                foreach (var dir in directions)
                {
                    var pos = new Pos<int>(x, y);
                    //TODO
                    var found = true;
                    if (found)
                    {
                        result++;
                    }
                }
            }
        }
        return result.ToString();

    }

    [TestMethod]
    public void Day04_Part1_Example01()
    {
        var input = """
            MMMSXXMASM
            MSAMXMSMSA
            AMXSXMAAMM
            MSAMASMSMX
            XMASAMXAMM
            XXAMMXXAMA
            SMSMSASXSS
            SAXAMASAAA
            MAMMMXMMMM
            MXMXAXMASX
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day04_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day04), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day04_Part2_Example01()
    {
        var input = """
            MMMSXXMASM
            MSAMXMSMSA
            AMXSXMAAMM
            MSAMASMSMX
            XMASAMXAMM
            XXAMMXXAMA
            SMSMSASXSS
            SAXAMASAAA
            MAMMMXMMMM
            MXMXAXMASX
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day04_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day04_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day04), "2024"));
        Assert.AreEqual("", result);
    }
    
}
