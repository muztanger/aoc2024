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

                    var dp = a2.Pos - a1.Pos;
                    List<Pos<int>> positions = [a1.Pos - dp, a2.Pos + dp];

                    {
                        var p = a1.Pos;
                        while (box.Contains(p))
                        {
                            antiNodes.Add(p);
                            p -= dp;
                        }
                    }
                    {
                        var p = a2.Pos;
                        while (box.Contains(p))
                        {
                            antiNodes.Add(p);
                            p += dp;
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
    public void Day08_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day08), "2024"));
        Assert.AreNotEqual("540", result, "540 is too high");
        Assert.AreEqual("269", result);
    }
    
    [TestMethod]
    public void Day08_Part2_Example01()
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
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("34", result);
    }
    
    [TestMethod]
    public void Day08_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day08), "2024"));
        Assert.AreEqual("949", result);
    }
    
}
