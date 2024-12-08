using System.Linq;

namespace Advent_of_Code_2024;

[TestClass]
public class Day08
{

    private static string Part1(IEnumerable<string> input)
    {
        var antennas = new DefaultValueDictionary<char, List<Pos<int>>>(() => new List<Pos<int>>());
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
                    antennas[frequency].Add(pos);
                    occupied.Add(pos);
                }
                box.IncreaseToPoint(pos);
            }
            row++;
        }

        var antiNodes = new HashSet<Pos<int>>();
        foreach (var frequency in antennas.Keys)
        {
            foreach (var item in antennas[frequency]
                .SelectMany((a1) => antennas[frequency].Select((a2) => (a1, a2)))
                .Where((item) => item.a1 != item.a2))
            {
                var dp = item.a2 - item.a1;
                List<Pos<int>> positions = [item.a1 - dp, item.a2 + dp];
                foreach (var pos in positions.Where(box.Contains))
                {
                    antiNodes.Add(pos);
                }
            }
        }

        return antiNodes.Count.ToString();
    }

    private static string Part2(IEnumerable<string> input)
    {
        var antennas = new DefaultValueDictionary<char, List<Pos<int>>>(() => new List<Pos<int>>());
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
                    antennas[frequency].Add(pos);
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
                    if (a1 == a2)
                    {
                        continue;
                    }

                    var dp = a2 - a1;
                    Harmonics(a1, -dp);
                    Harmonics(a2, dp);

                    void Harmonics(Pos<int> p, Pos<int> dp) {
                        while (box.Contains(p))
                        {
                            antiNodes.Add(p);
                            p += dp;
                        }
                    }
                }
            }
        }

        return antiNodes.Count.ToString();
    }

    string example = """
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

    [TestMethod]
    public void Day08_Part1_Example01()
    {
        var result = Part1(Common.GetLines(example));
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
        var result = Part2(Common.GetLines(example));
        Assert.AreEqual("34", result);
    }
    
    [TestMethod]
    public void Day08_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day08), "2024"));
        Assert.AreEqual("949", result);
    }
    
}
