using Advent_of_Code_2024.Commons;
using System;
using System.IO;

namespace Advent_of_Code_2024.NotFinished;

[TestClass]
public class Day20
{
    private static string Part1(IEnumerable<string> input, int threshold)
    {
        var result = new StringBuilder();
        var walls = new HashSet<Pos<int>>();
        var start = Pos<int>.Zero;
        var end = Pos<int>.Zero;
        var y = 0;
        foreach (var line in input)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == 'S')
                {
                    start = new Pos<int>(x, y);
                }
                else if (c == 'E')
                {
                    end = new Pos<int>(x, y);
                }
                else if (c == '#')
                {
                    walls.Add(new Pos<int>(x, y));
                }
                x++;
            }
            y++;
        }
        var box = new Box<int>(walls);

        NoCheatFastest(walls, start, end, box, out int oldFastest, out DefaultValueDictionary<Pos<int>, int>  oldMinMap);

        var savings = new DefaultValueDictionary<int, int>(() => 0);
        {
            var minMap = new DefaultValueDictionary<(Pos<int>, Pos<int> cheatPos), int>(() => int.MaxValue);
            var queue = new PriorityQueue<(Pos<int>, int cheat, Pos<int> cheatPos, int steps), int>();
            queue.Enqueue((start, 1, start, 0), 0);
            while (queue.Count > 0)
            {
                var (pos, cheat, cheatPos, steps) = queue.Dequeue();
                if (pos == end)
                {
                    int saving = oldFastest - steps;
                    if (saving > 0)
                    {
                        savings[saving]++;
                    }
                    continue;
                }

                if (oldMinMap[pos] < steps || minMap[(pos, cheatPos)] <= steps)
                {
                    continue;
                }
                minMap[(pos, cheatPos)] = steps;

                foreach (var dir in Pos<int>.CardinalDirections)
                {
                    var newPos = pos + dir;
                    if (!box.Contains(newPos))
                    {
                        continue;
                    }

                    bool inWall = walls.Contains(newPos);
                    if (cheat > 0 && inWall)
                    {
                        queue.Enqueue((newPos, cheat - 1, newPos, steps + 1), steps + 1);
                        continue;
                    }
                    if (inWall)
                    {
                        continue;
                    }

                    queue.Enqueue((newPos, cheat, cheatPos, steps + 1), steps + 1);
                }
            }
        }

        Console.WriteLine(string.Join("\n", savings.Select(savings => $"{savings.Key}: {savings.Value}")));

        return savings.Where(s => s.Key >= threshold).Sum(s => s.Value).ToString();
    }

    private static void NoCheatFastest(HashSet<Pos<int>> walls, Pos<int> start, Pos<int> end, Box<int> box, out int oldFastest, out DefaultValueDictionary<Pos<int>, int> oldMinMap)
    {
        oldFastest = int.MaxValue;
        oldMinMap = new DefaultValueDictionary<Pos<int>, int>(() => int.MaxValue);

        var queue = new Queue<(Pos<int>, int)>();
        queue.Enqueue((start, 0));
        while (queue.Count > 0)
        {
            var (pos, steps) = queue.Dequeue();
            if (pos == end)
            {
                oldFastest = steps;
                queue.Clear();
                break;
            }
            if (oldMinMap[pos] <= steps)
            {
                continue;
            }
            oldMinMap[pos] = steps;
            foreach (var dir in Pos<int>.CardinalDirections)
            {
                var newPos = pos + dir;
                if (walls.Contains(newPos) || !box.Contains(newPos))
                {
                    continue;
                }
                queue.Enqueue((newPos, steps + 1));
            }
        }
    }

    enum Cheat { Init, CollitionDisabled, Done }

    private static string Part2(IEnumerable<string> input, int threshold)
    {
        var result = new StringBuilder();
        var walls = new HashSet<Pos<int>>();
        var start = Pos<int>.Zero;
        var end = Pos<int>.Zero;
        var y = 0;
        foreach (var line in input)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == 'S')
                {
                    start = new Pos<int>(x, y);
                }
                else if (c == 'E')
                {
                    end = new Pos<int>(x, y);
                }
                else if (c == '#')
                {
                    walls.Add(new Pos<int>(x, y));
                }
                x++;
            }
            y++;
        }
        var box = new Box<int>(walls);

        NoCheatFastest(walls, start, end, box, out int noCheatFastest, out DefaultValueDictionary<Pos<int>, int> noCheatMinMap);

        var savings = new DefaultValueDictionary<int, HashSet<(Pos<int> start, Pos<int> end)>>(() => []);
        {
            var minMap = new DefaultValueDictionary<(Pos<int> pos, Pos<int> cheatStart, Pos<int> cheatEnd), int>(() => int.MaxValue);
            var queue = new PriorityQueue<(Pos<int> pos, Cheat cheatState, int cheatCount, Pos<int> cheatStart, List<Pos<int>> cheatPath, int steps), int>();
            queue.Enqueue((start, Cheat.Init, 0, -Pos<int>.One, [], 0), 0);
            while (queue.Count > 0)
            {
                var (pos, cheat, cheatCount, cheatStart, cheatPath, steps) = queue.Dequeue();
                
                Pos<int> FirstNonWallAfterLastWall()
                {
                    int i = cheatPath.Count - 1;
                    while (i >= 0 && !walls.Contains(cheatPath[i]))
                    {
                        i--;
                    }
                    if (i < 0 || i >= cheatPath.Count - 1)
                    {
                        return -Pos<int>.One;
                    }
                    return cheatPath[i + 1];
                }
                var cheatEnd = FirstNonWallAfterLastWall();

                if (minMap[(pos, cheatStart, cheatEnd)] <= steps)
                {
                    continue;
                }
                minMap[(pos, cheatStart, cheatEnd)] = steps;
                
                if (pos == end)
                {
                    int saving = noCheatFastest - steps;
                    if (saving >= threshold)
                    {
                        savings[saving].Add((cheatStart, cheatEnd));
                    }
                    continue;
                }

                foreach (var dir in Pos<int>.CardinalDirections)
                {
                    var newPos = pos + dir;
                    if (!box.Contains(newPos))
                    {
                        continue;
                    }

                    if (walls.Contains(newPos)) 
                    {
                        if (cheat == Cheat.Init)
                        {
                            queue.Enqueue((newPos, Cheat.CollitionDisabled, cheatCount + 1, pos, [newPos], steps + 1), steps + 1);
                        }
                        else if (cheat == Cheat.CollitionDisabled && cheatCount < 20)
                        {
                            queue.Enqueue((newPos, Cheat.CollitionDisabled, cheatCount + 1, cheatStart, [..cheatPath, newPos], steps + 1), steps + 1);
                        }

                        continue;
                    }

                    if (cheat == Cheat.CollitionDisabled)
                    {
                        if (cheatCount < 20)
                        {
                            queue.Enqueue((newPos, Cheat.Done,              cheatCount + 1, cheatStart, [..cheatPath, newPos], steps + 1), steps + 1);
                            queue.Enqueue((newPos, Cheat.CollitionDisabled, cheatCount + 1, cheatStart, [..cheatPath, newPos], steps + 1), steps + 1);
                        }
                        else
                        {
                            queue.Enqueue((newPos, Cheat.Done, cheatCount, cheatStart, cheatPath, steps + 1), steps + 1);
                        }
                    }
                    else
                    {
                        queue.Enqueue((newPos, cheat, cheatCount, cheatStart, cheatPath, steps + 1), steps + 1);
                    }
                }
            }
        }

        foreach (var kv in savings)
        {
            Console.WriteLine($"{kv.Key}: {kv.Value.Count}");
            foreach (var p in box.GetPositions())
            {
                if (kv.Value.Where(s => s.start == p).Any())
                {
                    Console.Write('S');
                }
                else if (kv.Value.Where(s => s.end == p).Any())
                {
                    Console.Write('D');
                }
                else if (walls.Contains(p))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
                if (p.x == box.Max.x)
                {
                    Console.WriteLine();
                }
            }
        }

            Console.WriteLine(string.Join("\n", savings.Select(savings => $"{savings.Key}: {savings.Value.Count}: {string.Join(",", savings.Value)}")));

        return savings.Where(s => s.Key >= threshold).Sum(s => s.Value.Count).ToString();
    }

    [TestMethod]
    public void Day20_Part1_Example01()
    {
        var input = """
            ###############
            #...#...#.....#
            #.#.#.#.#.###.#
            #S#...#.#.#...#
            #######.#.#.###
            #######.#.#...#
            #######.#.###.#
            ###..E#...#...#
            ###.#######.###
            #...###...#...#
            #.#####.#.###.#
            #.#...#.#.#...#
            #.#.#.#.#.#.###
            #...#...#...###
            ###############
            """;
        var result = Part1(Common.GetLines(input), 0);
        Assert.AreEqual("44", result);
    }
    
    [TestMethod]
    public void Day20_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day20), "2024"), 100);
        Assert.AreEqual("1365", result);
    }
    
    [TestMethod]
    public void Day20_Part2_Example01()
    {
        var input = """
            ###############
            #...#...#.....#
            #.#.#.#.#.###.#
            #S#...#.#.#...#
            #######.#.#.###
            #######.#.#...#
            #######.#.###.#
            ###..E#...#...#
            ###.#######.###
            #...###...#...#
            #.#####.#.###.#
            #.#...#.#.#...#
            #.#.#.#.#.#.###
            #...#...#...###
            ###############
            """;
        var result = Part2(Common.GetLines(input), 50);
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day20_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day20), "2024"), 100);
        Assert.AreEqual("", result);
    }
    
}
