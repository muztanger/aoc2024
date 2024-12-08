namespace Advent_of_Code_2024;

[TestClass]
public class Day08
{
    class Antenna
    {
        public char Frequency { get; set; }
        required public Pos<int> Pos { get; set; }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var antennas = new DefaultValueDictionary<char, List<Antenna>>(() => new List<Antenna>());
        var box = new Box<int>(1, 1);
        var row = 0;
        var occupied = new HashSet<Pos<int>>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            for (var col = 0; col < line.Length; col++)
            {
                Pos<int> pos = new Pos<int>(col, row);

                char frequency = line[col];
                if (frequency != '.')
                {
                    antennas[frequency].Add(new Antenna()
                    {
                        Frequency = frequency,
                        Pos = pos
                    });
                    occupied.Add(pos);
                }
                box.IncreaseToPoint(pos);
            }
            row++;
        }

        var antiNodes = new HashSet<Pos<int>>();
        foreach (var frequency in antennas.Keys)
        {
            foreach (var a1 in antennas[frequency])
            {
                foreach (var a2 in antennas[frequency])
                {
                    if (a1.Pos == a2.Pos)
                    {
                        continue;
                    }
                    // Example:
                    // ....
                    // ....
                    // .12.
                    //
                    // a1.Pos = (1, 3)
                    // a2.Pos = (2, 3)
                    // dp = (2, 3) - (1, 3) = (1, 0)
                    // a1 - dp, a2 + dp
                    // positions = [(1, 3) + (1, 0), (1, 3) - (1, 0), (2, 3) + (1, 0), (2, 3) - (1, 0)]
                    // positions = [(2, 3), (0, 3), (3, 3), (1, 3)]

                    var dp = a2.Pos - a1.Pos;
                    List<Pos<int>> positions = [a1.Pos - dp, a2.Pos + dp];
                    foreach (var pos in positions)
                    {
                        if (box.Contains(pos))
                        {
                            antiNodes.Add(pos);
                        }
                    }
                }
            }
        }

        var result = new StringBuilder();
        for (var y = 0; y <= box.Max.y; y++)
        {
            for (var x = 0; x <= box.Max.x; x++)
            {
                if (occupied.Contains(new Pos<int>(x, y)))
                {
                    result.Append('a');
                }
                else if (antiNodes.Contains(new Pos<int>(x, y)))
                {
                    result.Append('#');
                }
                else
                {
                    result.Append('.');
                }
            }
            result.AppendLine();
        }
        Console.WriteLine(result.ToString());


        return antiNodes.Count.ToString();
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
    public void Day08_Part1_Example01()
    {
        var input = """
            ............
            ........0...
            .....0......
            .......0....
            ....0.......
            ......A.....
            ............
            ............
            ........A...
            .........A..
            ............
            ............
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("14", result);
    }
    
    [TestMethod]
    public void Day08_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day08_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day08), "2024"));
        Assert.AreNotEqual("540", result); // 540 is too high
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day08_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day08_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day08_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day08), "2024"));
        Assert.AreEqual("", result);
    }
    
}