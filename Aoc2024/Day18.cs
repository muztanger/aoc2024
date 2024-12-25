namespace Advent_of_Code_2024.NotFinished;

[TestClass]
public class Day18
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var incoming = new DefaultValueDictionary<Pos<int>, List<int>>(() => new List<int>());
        var lineIndex = 0;
        foreach (var line in input)
        {
            var ints = line.Split(',').Select(int.Parse).ToArray();
            incoming[new Pos<int>(ints[0], ints[1])].Add(lineIndex);
            lineIndex++;
        }
        var box = new Box<int>(incoming.Select(kv => kv.Key));
        
        void Print(int time, List<Pos<int>>? path = null)
        {
            if (path is null)
            {
                path = new List<Pos<int>>();
            }
            for (var y = box.Min.y; y <= box.Max.y; y++)
            {
                for (var x = box.Min.x; x <= box.Max.x; x++)
                {
                    var pos = new Pos<int>(x, y);
                    if (path.Contains(pos))
                    {
                        Console.Write('O');
                    }
                    else if (incoming[pos].DefaultIfEmpty(int.MaxValue).Max() <= time)
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }

        var time = 12;
        var queue = new PriorityQueue<(Pos<int> pos, int time, List<Pos<int>> path), int>();
        queue.Enqueue((Pos<int>.Zero, 0, new List<Pos<int>> { Pos<int>.Zero }), 0);
        while (queue.Count > 0)
        {
            var (pos, _, path) = queue.Dequeue();
            if (incoming[pos].DefaultIfEmpty(int.MaxValue).Max() <= time)
            {
                continue;
            }
            if (pos == box.Max)
            {
                Console.WriteLine(string.Join(" -> ", path));
                result.Append(time);
                Print(0, path);
                break;
            }
            foreach (var dir in Pos<int>.CardinalDirections)
            {
                var newPos = pos + dir;
                if (!box.Contains(newPos))
                {
                    continue;
                }
                queue.Enqueue((newPos, time + 1, new List<Pos<int>>(path) { newPos }), time + 1);
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
    public void Day18_Part1_Example01()
    {
        var input = """
            5,4
            4,2
            4,5
            3,0
            2,1
            6,3
            2,4
            1,5
            0,6
            3,3
            2,6
            5,1
            1,2
            5,5
            2,5
            6,5
            1,4
            0,4
            6,4
            1,1
            6,1
            1,0
            0,5
            1,6
            2,0
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
   
    [TestMethod]
    public void Day18_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day18), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day18_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day18_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day18_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day18), "2024"));
        Assert.AreEqual("", result);
    }
    
}
