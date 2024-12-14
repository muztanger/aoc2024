namespace Advent_of_Code_2024;

[TestClass]
public class Day14
{
    class Robot
    {
        public required int Id;
        public Pos<int> Position;
        public Pos<int> Velocity;

        public string PosString()
        {
            return $"({Id}:{Position})";
        }
    }

    private static string Part1(IEnumerable<string> input, Box<int> space)
    {
        var robots = new List<Robot>();
        var id = 0;
        foreach (var line in input)
        {
            var match = Regex.Match(line, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
            if (match.Success)
            {
                robots.Add(new Robot()
                {
                    Id = id++,
                    Position = new Pos<int>(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                    Velocity = new Pos<int>(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
                });
            }
        }
        void Move()
        {
            foreach (var robot in robots)
            {
                robot.Position += robot.Velocity;
                if (!space.Contains(robot.Position))
                {
                    if (space.Min.x > robot.Position.x)
                    {
                        robot.Position.x += space.Width;
                    }
                    else if (space.Max.x < robot.Position.x)
                    {
                        robot.Position.x -= space.Width;
                    }

                    if (space.Min.y > robot.Position.y)
                    {
                        robot.Position.y += space.Height;
                    }
                    else if (space.Max.y < robot.Position.y)
                    {
                        robot.Position.y -= space.Height;
                    }
                }
            }
        }

        void Print()
        {
            var result = new StringBuilder();

            for (int y = space.Min.y; y <= space.Max.y; y++)
            {
                for (int x = space.Min.x; x <= space.Max.x; x++)
                {
                    Pos<int> pos = new Pos<int>(x, y);
                    int value = robots.Count(r => r.Position == pos);
                    if (value > 0)
                    {
                        result.Append(value > 9 ? '+' : value);
                    }
                    else
                    {
                        result.Append(".");
                    }
                }
                result.AppendLine();
            }
            Console.WriteLine(result.ToString());
            Console.WriteLine();
        }

        Print();
        for (int i = 0; i < 100; i++)
        {
            Move();
        }
        Print();

        var safetyFactor = 1;
        foreach (var dq in new Box<int>(2, 2).GetPositions())
        {
            var width = space.Width / 2;
            var height = space.Height / 2;
            int x = dq.x * width + dq.x;
            int y = dq.y * height + dq.y;
            var quadrant = new Box<int>(new Pos<int>(x, y), new Pos<int>(x + width - 1, y + height - 1));
            var count = robots.Count(r => quadrant.Contains(r.Position));
            safetyFactor *= count;
        }

        return safetyFactor.ToString();
    }

    private static string Part2(IEnumerable<string> input, Box<int> space)
    {
        var robots = new List<Robot>();
        int id = 0;
        foreach (var line in input)
        {
            var match = Regex.Match(line, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
            if (match.Success)
            {
                robots.Add(new Robot()
                {
                    Id = id++,
                    Position = new Pos<int>(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                    Velocity = new Pos<int>(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
                });
            }
        }
        var initialRobots = new List<Robot>(robots.Select(r => new Robot() { Position = r.Position, Id = r.Id, Velocity = r.Velocity}));

        void Move()
        {
            foreach (var robot in robots)
            {
                robot.Position += robot.Velocity;
                if (!space.Contains(robot.Position))
                {
                    if (space.Min.x > robot.Position.x)
                    {
                        robot.Position.x += space.Width;
                    }
                    else if (space.Max.x < robot.Position.x)
                    {
                        robot.Position.x -= space.Width;
                    }

                    if (space.Min.y > robot.Position.y)
                    {
                        robot.Position.y += space.Height;
                    }
                    else if (space.Max.y < robot.Position.y)
                    {
                        robot.Position.y -= space.Height;
                    }
                }
            }
        }
        string ToString()
        {
            var result = new StringBuilder();

            for (int y = space.Min.y; y <= space.Max.y; y++)
            {
                for (int x = space.Min.x; x <= space.Max.x; x++)
                {
                    Pos<int> pos = new Pos<int>(x, y);
                    int value = robots.Count(r => r.Position == pos);
                    if (value > 0)
                    {
                        result.Append(value > 9 ? '+' : value);
                    }
                    else
                    {
                        result.Append(".");
                    }
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        void Print()
        {
            Console.WriteLine(ToString());
            Console.WriteLine();
        }

        int width = space.Width / 2;
        int height = space.Height / 2;
        var quadrants = new List<Box<int>>();
        foreach (var dq in new Box<int>(2, 2).GetPositions())
        {
            int x = dq.x * width + dq.x;
            int y = dq.y * height + dq.y;
            var quadrant = new Box<int>(new Pos<int>(x, y), new Pos<int>(x + width - 1, y + height - 1));
            quadrants.Add(quadrant);
        }
        string RobotsString()
        {
            var result = new StringBuilder();
            foreach (var robot in robots)
            {
                result.Append(robot.PosString());
            }
            return result.ToString();
        }
        var visited = new HashSet<string>
        {
            RobotsString()
        };

        // create a template tree of positions
        var template = new HashSet<Pos<int>>();
        var center = space.Width / 2;
        template.Add(new Pos<int>(center, 0));
        for (int y = 1; y <= Math.Min(space.Max.y, space.Max.x / 2); y++)
        {
            template.Add(new Pos<int>(center, y));
            for (int dx = 1; dx <= y; dx++)
            {
                template.Add(new Pos<int>(center + dx, y));
                template.Add(new Pos<int>(center - dx, y));
            }
        }
        var templateBox = new Box<int>(template);

        var seconds = 0;
        for (int dy = 0; dy <= space.Height - templateBox.Height; dy++)
        {
            var dyPos = new Pos<int>(0, dy);
            robots = new List<Robot>(initialRobots.Select(r => new Robot() { Position = r.Position, Id = r.Id, Velocity = r.Velocity }));
            seconds = 0;
            visited.Clear();

            while(true)
            {
                Move();
                seconds++;

                if (!visited.Add(RobotsString()))
                {
                    // Loop detected
                    Console.WriteLine($"Loop detected after {seconds} seconds. dy={dy}");
                    break;
                }

                var templateCount = 0;
                Parallel.ForEach(robots, robot =>
                {
                    if (template.Contains(robot.Position + dyPos))
                    {
                        Interlocked.Increment(ref templateCount);
                    }
                });
                if (templateCount > robots.Count * 2 / 3)
                {
                    var robotsString = ToString();
                    Console.WriteLine(seconds);
                    Console.WriteLine(robotsString);
                    return seconds.ToString();
                }
            }

        }

        return (-1).ToString();
    }


    [TestMethod]
    public void Day14_Part1_Example01()
    {
        var input = """
            p=0,4 v=3,-3
            p=6,3 v=-1,-3
            p=10,3 v=-1,2
            p=2,0 v=2,-1
            p=0,0 v=1,3
            p=3,0 v=-2,-2
            p=7,6 v=-1,-3
            p=3,0 v=-1,-2
            p=9,3 v=2,3
            p=7,3 v=-1,2
            p=2,4 v=2,-3
            p=9,5 v=-3,-3
            """;
        var result = Part1(Common.GetLines(input), new Box<int>(11, 7));
        Assert.AreEqual("12", result);
    }
    
    [TestMethod]
    public void Day14_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day14), "2024"), new Box<int>(101, 103));
        Assert.AreEqual("221142636", result);
    }
    
   
    [TestMethod]
    public void Day14_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day14), "2024"), new Box<int>(101, 103));
        Assert.AreEqual("7916", result);
    }
    
}
