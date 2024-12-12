namespace Advent_of_Code_2024;

[TestClass]
public class Day12
{
    class Region
    {
        public char Plant { get; set; }
        public HashSet<Pos<int>> Positions { get; set; } = new();

        public int Perimeter()
        {
            var perimeter = 0;
            foreach (var pos in Positions)
            {
                foreach (var dir in Pos<int>.CardinalDirections)
                {
                    var newPos = pos + dir;
                    if (!Positions.Contains(newPos))
                    {
                        perimeter++;
                    }
                }
            }
            return perimeter;
        }

        public int Area()
        {
            return Positions.Count;
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var grid = new List<List<char>>();
        foreach (var line in input)
        {
            grid.Add(line.ToList());
        }
        var box = new Box<int>(grid[0].Count, grid.Count);

        var visited = new HashSet<Pos<int>>();
        var regions = new List<Region>();

        var stack = new Stack<Pos<int>>();
        while (visited.Count < box.Area)
        {
            var pos = box.GetPositions().First(p => !visited.Contains(p));
            var region = new Region { Plant = grid[pos.y][pos.x] };

            stack.Push(pos);
            while (stack.Count > 0)
            {
                pos = stack.Pop();
                if (visited.Contains(pos))
                {
                    continue;
                }

                if (region.Plant != grid[pos.y][pos.x])
                {
                    continue;
                }
                visited.Add(pos);

                region.Positions.Add(pos);

                foreach (var dir in Pos<int>.CardinalDirections)
                {
                    var newPos = pos + dir;
                    if (box.Contains(newPos) && !visited.Contains(newPos))
                    {
                        stack.Push(newPos);
                    }
                }
            }
            regions.Add(region);
        }

        return regions.Sum(r => r.Perimeter() * r.Area()).ToString();
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
    public void Day12_Part1_Example01()
    {
        var input = """
            AAAA
            BBCD
            BBCC
            EEEC
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("140", result);
    }
    
    [TestMethod]
    public void Day12_Part1_Example02()
    {
        var input = """
            OOOOO
            OXOXO
            OOOOO
            OXOXO
            OOOOO
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void Day12_Part1_Example03()
    {
        var input = """
            RRRRIICCFF
            RRRRIICCCF
            VVRRRCCFFF
            VVRCCCJFFF
            VVVVCJJCFE
            VVIVCCJJEE
            VVIIICJJEE
            MIIIIIJJEE
            MIIISIJEEE
            MMMISSJEEE
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void Day12_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day12), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day12_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day12_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day12_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day12), "2024"));
        Assert.AreEqual("", result);
    }
    
}
