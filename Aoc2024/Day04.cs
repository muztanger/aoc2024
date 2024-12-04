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
        var box = new Box<int>(board[0].Length, board.Count);

        const string xmas = "XMAS";
        foreach (var start in box.GetPositions())
        {
            foreach (var dir in Pos<int>.CompassDirections)
            {
                var pos = start;
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
        var box = new Box<int>(board[0].Length, board.Count);
        const string mas = "MAS";

        foreach (var center in box.GetPositions())
        {
            if (board[center.y][center.x] != 'A')
            {
                continue;
            }
            var count = 0;
            foreach (var dir in Pos<int>.Diagonals)
            {
                var pos = center + dir;
                for (int i = 0; i < mas.Length; i++)
                {
                    if (!box.Contains(pos) || board[pos.y][pos.x] != mas[i])
                    {
                        break;
                    }
                    if (i == mas.Length - 1)
                    {
                        count++;
                    }
                    pos -= dir;
                }
            }
            if (count >= 2)
            {
                result++;
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
        Assert.AreEqual("18", result);
    }
    
    [TestMethod]
    public void Day04_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day04), "2024"));
        Assert.AreEqual("2536", result);
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
        Assert.AreEqual("9", result);
    }
    
    [TestMethod]
    public void Day04_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day04), "2024"));
        Assert.AreEqual("1875", result);
    }
    
}
