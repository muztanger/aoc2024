
using System.Linq;

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

        internal int Sides()
        {
            // make a 3x3 square of each pos in Positions to avoid "corner" cases
            var zoomed = new HashSet<Pos<int>>();
            const int zoom = 3;
            foreach (var pos in Positions) {

                var pNew = pos * zoom;
                var box = new Box<int>(zoom, zoom);
                foreach (var dp in box.GetPositions())
                {
                    zoomed.Add(pNew + dp);
                }
            }

            // find all the perimeters
            var perimeters = new HashSet<Pos<int>>();
            foreach (var pos in zoomed)
            {
                foreach (var dp in Pos<int>.CompassDirections)
                {
                    var newPos = pos + dp;
                    if (!zoomed.Contains(newPos))
                    {
                        perimeters.Add(newPos);
                    }
                }
            }

            {
                // print perimeter
                Console.WriteLine($"Perimeter for plant '{Plant}'");
                var box = new Box<int>(perimeters.ToArray());
                for (var y = box.Min.y; y <= box.Max.y; y++)
                {
                    for (var x = box.Min.x; x <= box.Max.x; x++)
                    {
                        var pos = new Pos<int>(x, y);
                        if (perimeters.Contains(pos))
                        {
                            Console.Write('p');
                        }
                        else if (zoomed.Contains(pos))
                        {
                            Console.Write('o');
                        }
                        else
                        {
                            Console.Write('.');
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            // inner corners: 5 compass neighbors
            //   pppppp
            //   pppppp
            //   ppnnnp
            //   ppnooo
            //   ppnooo
            //   pppooo
            //
            // outer corners: 1 compass neighbor
            //   oooooo
            //   oooooo
            //   oooooo
            //   ooonpp
            //   oooppp
            //   oooppp


            // find all corners
            var corners = new HashSet<Pos<int>>();
            foreach (var pos in perimeters)
            {
                var directions = Pos<int>.CardinalDirections.ToArray();
                for (int i = 0; i < directions.Length; i++)
                {
                    var j = (i + 1) % directions.Length;
                    if (perimeters.Contains(pos + directions[i]) && perimeters.Contains(pos + directions[j]))
                    {
                        corners.Add(pos);
                        break;
                    }
                }
            }

            return corners.Count;
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
        Console.WriteLine(regions.Count);
        Console.WriteLine(string.Join("\n", regions.Select(r => $"{r.Plant}: Area={r.Area()} Sides={r.Sides()}")));
        return regions.Sum(r => (long) r.Sides() * r.Area()).ToString();
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
        Assert.AreEqual("1483212", result);
    }
    
    [TestMethod]
    public void Day12_Part2_Example01()
    {
        var input = """
            AAAA
            BBCD
            BBCC
            EEEC
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("80", result);
    }
    
    [TestMethod]
    public void Day12_Part2_Example02()
    {
        var input = """
            EEEEE
            EXXXX
            EEEEE
            EXXXX
            EEEEE
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("236", result);
    }

    [TestMethod]
    public void Day12_Part2_Example03()
    {
        var input = """
            AAAAAA
            AAABBA
            AAABBA
            ABBAAA
            ABBAAA
            AAAAAA
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("368", result);
    }

    [TestMethod]
    public void Day12_Part2_Example04()
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
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("1206", result);
    }

    [TestMethod]
    public void Day12_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day12), "2024"));
        Assert.AreNotEqual("892054", result); // too low
        Assert.AreEqual("", result);
    }
    
}
