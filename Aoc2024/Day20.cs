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
        {
            var queue = new Queue<(Pos<int>, int)>();
            queue.Enqueue((start, 0));
            while (queue.Any())
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
    }

    enum Cheat { NotStarted, Started, Done }

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

        var savings = new DefaultValueDictionary<int, HashSet<string>>(() => []);
        {
            var minMap = new DefaultValueDictionary<Pos<int>, int>(() => int.MaxValue);
            var queue = new PriorityQueue<(Pos<int> pos, Pos<int> last, Cheat cheatState, HashSet<Pos<int>> cheatPath, Pos<int> cheatStart, Pos<int> cheatEnd, int steps), int>();
            queue.Enqueue((start, -Pos<int>.One, Cheat.NotStarted, new HashSet<Pos<int>>(), -Pos<int>.One, -Pos<int>.One, 0), 0);
            var visited = new HashSet<(Pos<int>, string cheatPathString, int steps)>();
            while (queue.Count > 0)
            {
                var (pos, last, cheat, cheatPath, cheatStart, cheatEnd, steps) = queue.Dequeue();

                if (noCheatMinMap[pos] < steps || minMap[pos] <= steps)
                {
                    continue;
                }
                minMap[pos] = steps;
                
                if (pos == end)
                {
                    int saving = noCheatFastest - steps;
                    if (saving > 0)
                    {
                        savings[saving].Add(string.Concat(cheatStart, cheatEnd));
                    }
                    continue;
                }

                foreach (var dir in Pos<int>.CardinalDirections)
                {
                    var newPos = pos + dir;
                    if (!box.Contains(newPos) || newPos == last || cheatPath.Contains(newPos))
                    {
                        continue;
                    }

                    bool inWall = walls.Contains(newPos);
                    if (inWall && (cheat == Cheat.NotStarted || (cheat == Cheat.Started && cheatPath.Count < 20)))
                    {
                        var newCheatPath = new HashSet<Pos<int>>(cheatPath)
                        {
                            newPos
                        };
                        queue.Enqueue((newPos, pos, Cheat.Started, newCheatPath, pos, -Pos<int>.One, steps + 1), steps + 1);
                        continue;
                    }
                    
                    if (inWall)
                    {
                        continue;
                    }

                    if (cheat == Cheat.Started)
                    {
                        queue.Enqueue((newPos, pos, Cheat.Done, cheatPath, cheatStart, newPos, steps + 1), steps + 1);
                    }
                    else
                    {
                        queue.Enqueue((newPos, pos, cheat, cheatPath, cheatStart, cheatEnd, steps + 1), steps + 1);
                    }
                }
            }
        }

        Console.WriteLine(string.Join("\n", savings.Select(savings => $"{savings.Key}: {savings.Value}")));

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
