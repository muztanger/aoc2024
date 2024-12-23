namespace Advent_of_Code_2024;

[TestClass]
public class Day23
{
    class Computer
    {
        required public string Name { get; set; }
        public List<Computer> Connections { get; set; } = [];

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (Computer)obj;
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = 0;
        var networkMap = new HashSet<Computer>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var (left, right) = line.Split('-');
            var leftComputer = new Computer() { Name = left };
            var rightComputer = new Computer() { Name = right };
            networkMap.Add(leftComputer);
            networkMap.Add(rightComputer);
        }
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var (left, right) = line.Split('-');
            var leftComputer = networkMap.First(c => c.Name == left);
            var rightComputer = networkMap.First(c => c.Name == right);
            if (!leftComputer.Connections.Contains(rightComputer))
            {
                leftComputer.Connections.Add(rightComputer);
            }
            if (!rightComputer.Connections.Contains(leftComputer))
            {
                rightComputer.Connections.Add(leftComputer);
            }
        }

        Console.WriteLine(string.Join(",", networkMap.Select(c => $"{c.Name}: {string.Join(",", c.Connections)}")));

        var found = new List<string>();
        var visited = new HashSet<string>();
        var queue = new Queue<List<Computer>>();
        foreach (var computer in networkMap)
        {
            queue.Enqueue([computer]);
        }
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var visitedString = string.Join(",", current.OrderBy(c => c.Name));
            if (visited.Contains(visitedString))
            {
                continue;
            }
            visited.Add(visitedString);
            
            if (current.Count == 3)
            {
                found.Add(visitedString);
                continue;
            }

            foreach (var computer in networkMap)
            {
                if (current.Contains(computer))
                {
                    continue;
                }
                if (current.All(c => c.Connections.Contains(computer)))
                {
                    var next = new List<Computer>(current) { computer };
                    queue.Enqueue(next);
                }
            }
        }
        foreach (var item in found.OrderBy(s => s))
        {
            //at least one of the computer names should start with 't'
            if (item.Split(',').Any(c => c.StartsWith("t")))
            {
                result++;
            }
        }

        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var networkMap = new HashSet<Computer>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var (left, right) = line.Split('-');
            var leftComputer = new Computer() { Name = left };
            var rightComputer = new Computer() { Name = right };
            networkMap.Add(leftComputer);
            networkMap.Add(rightComputer);
        }
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            var (left, right) = line.Split('-');
            var leftComputer = networkMap.First(c => c.Name == left);
            var rightComputer = networkMap.First(c => c.Name == right);
            if (!leftComputer.Connections.Contains(rightComputer))
            {
                leftComputer.Connections.Add(rightComputer);
            }
            if (!rightComputer.Connections.Contains(leftComputer))
            {
                rightComputer.Connections.Add(leftComputer);
            }
        }

        var found = new List<string>();
        var visited = new HashSet<string>();
        var queue = new Queue<List<Computer>>();
        foreach (var computer in networkMap)
        {
            queue.Enqueue([computer]);
        }
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var visitedString = string.Join(",", current.OrderBy(c => c.Name));
            if (visited.Contains(visitedString))
            {
                continue;
            }
            visited.Add(visitedString);

            foreach (var computer in networkMap)
            {
                if (current.Contains(computer))
                {
                    continue;
                }
                if (current.All(c => c.Connections.Contains(computer)))
                {
                    var next = new List<Computer>(current) { computer };
                    queue.Enqueue(next);
                }
            }
        }

        return visited.OrderByDescending(s => s.Length).First();

    }

    [TestMethod]
    public void Day23_Part1_Example01()
    {
        var input = """
            kh-tc
            qp-kh
            de-cg
            ka-co
            yn-aq
            qp-ub
            cg-tb
            vc-aq
            tb-ka
            wh-tc
            yn-cg
            kh-ub
            ta-co
            de-co
            tc-td
            tb-wq
            wh-td
            ta-ka
            td-qp
            aq-cg
            wq-ub
            ub-vc
            de-ta
            wq-aq
            wq-vc
            wh-yn
            ka-de
            kh-ta
            co-tc
            wh-qp
            tb-vc
            td-yn
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("7", result);
    }
    
    [TestMethod]
    public void Day23_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day23), "2024"));
        Assert.AreEqual("1075", result);
    }
    
    [TestMethod]
    public void Day23_Part2_Example01()
    {
        var input = """
            kh-tc
            qp-kh
            de-cg
            ka-co
            yn-aq
            qp-ub
            cg-tb
            vc-aq
            tb-ka
            wh-tc
            yn-cg
            kh-ub
            ta-co
            de-co
            tc-td
            tb-wq
            wh-td
            ta-ka
            td-qp
            aq-cg
            wq-ub
            ub-vc
            de-ta
            wq-aq
            wq-vc
            wh-yn
            ka-de
            kh-ta
            co-tc
            wh-qp
            tb-vc
            td-yn
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("co,de,ka,ta", result);
    }
    
    [TestMethod]
    public void Day23_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day23), "2024"));
        Assert.AreEqual("az,cg,ei,hz,jc,km,kt,mv,sv,sx,wc,wq,xy", result);
    }
    
}
