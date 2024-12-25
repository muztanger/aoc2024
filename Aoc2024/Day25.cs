namespace Advent_of_Code_2024.NotFinished;

[TestClass]
public class Day25
{
    class Grid
    {
        public int Id { get; set; }
        required public List<List<char>> Cells { get; set; }
        required public Box<int> Size { get; set; }

        public bool IsLock()
        {
            return Cells[0][0] == '#';
        }

        public bool IsKey()
        {
            return !IsLock();
        }

        public bool IsFit(Grid other)
        {
            if (!Size.Equals(other.Size))
            {
                return false;
            }

            for (int y = 0; y < Size.Height; y++)
            {
                for (int x = 0; x < Size.Width; x++)
                {
                    if (Cells[y][x] == '#' && other.Cells[y][x] == '#')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
    private static string Part1(IEnumerable<string> input)
    {
        var result = 0;
        var grids = new List<Grid>();
        var rows = new List<List<char>>();
        foreach (var line in input)
        {
            if (line == "")
            {
                grids.Add(new Grid { Id = grids.Count, Cells = rows, Size = new Box<int>(rows[0].Count, rows.Count) });
                rows = [];
            }
            else
            {
                rows.Add(line.ToList());
            }
        }
        if (rows.Any())
        {
            grids.Add(new Grid { Id = grids.Count, Cells = rows, Size = new Box<int>(rows[0].Count, rows.Count) });
        }
        foreach (var grid in grids.Where(g => g.IsLock()))
        {
            foreach (var other in grids.Where(g => g.IsKey()))
            {
                if (grid.IsFit(other))
                {
                    result += 1;
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
    public void Day25_Part1_Example01()
    {
        var input = """
            #####
            .####
            .####
            .####
            .#.#.
            .#...
            .....

            #####
            ##.##
            .#.##
            ...##
            ...#.
            ...#.
            .....

            .....
            #....
            #....
            #...#
            #.#.#
            #.###
            #####

            .....
            .....
            #.#..
            ###..
            ###.#
            ###.#
            #####

            .....
            .....
            .....
            #....
            #.#..
            #.#.#
            #####
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("3", result);
    }
    
    [TestMethod]
    public void Day25_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day25), "2024"));
        Assert.AreEqual("3365", result);
    }
    
    [TestMethod]
    public void Day25_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day25_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day25_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day25), "2024"));
        Assert.AreEqual("", result);
    }
    
}
