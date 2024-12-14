namespace Advent_of_Code_2024;

[TestClass]
public class Day14
{
    class Robot
    {
        public Pos<int> Position;
        public Pos<int> Velocity;
    }

    private static string Part1(IEnumerable<string> input, Box<int> space)
    {
        var robots = new List<Robot>();
        foreach (var line in input)
        {
            var match = Regex.Match(line, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
            if (match.Success)
            {
                robots.Add(new Robot()
                {
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
        foreach (var line in input)
        {
            var match = Regex.Match(line, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");
            if (match.Success)
            {
                robots.Add(new Robot()
                {
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

        var seconds = 0;
        for(;;)
        {
            Move();
            seconds++;
            // check quadrant counts
            var counts = new List<int>();
            foreach (var quadrant in quadrants)
            {
                var count = robots.Count(r => quadrant.Contains(r.Position));
                counts.Add(count);
            }
            if (counts[0] != counts[1] || counts[2] != counts[3])
            {
                continue;
            }

            // check for mirror symmetry
            var isSymmetric = true;
            Parallel.For(space.Min.y, space.Max.y + 1, (y, state) =>
            {
                for (var x = space.Min.x; x <= space.Max.x / 2; x++)
                {
                    var pos = new Pos<int>(x, y);
                    var value = robots.Any(r => r.Position == pos);
                    if (value == true)
                    {
                        var mirrorPos = new Pos<int>(space.Max.x - x, y);
                        var mirrorValue = robots.Any(r => r.Position == mirrorPos);
                        if (value != mirrorValue)
                        {
                            isSymmetric = false;
                            state.Break();
                        }
                    }
                }
            });
            if (isSymmetric) {
                Console.WriteLine(seconds);
                var robotsString = ToString();
                Console.WriteLine(robotsString);
                break;
            }
        }

        return seconds.ToString();
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
        Assert.AreEqual("", result);
    }
    
   
    [TestMethod]
    public void Day14_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day14), "2024"), new Box<int>(101, 103));
        Assert.AreEqual("", result);
    }
    
}
