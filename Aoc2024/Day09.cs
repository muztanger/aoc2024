using System;
using Aoc2024.Commons;

namespace Advent_of_Code_2024;

[TestClass]
public class Day09
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var diskmap = input.First().Select(c => int.Parse("" + c)).ToList();
        var disk = new List<int>();
        var size = 0;
        for (var i = 0; i < diskmap.Count; i++)
        {
            if (i % 2 == 0)
            {
                var index = i / 2;
                disk.AddRange(Enumerable.Repeat(index, diskmap[i]));
                size += diskmap[i];
            }
            else
            {
                disk.AddRange(Enumerable.Repeat(-1, diskmap[i]));
            }
        }

        var profiler = new Profiler();
        profiler.Start();

        var lastNonZero = disk.Count - 1;
        bool IsCompact()
        {
            for (int i = lastNonZero; i >= 0; i--)
            {
                if (disk[i] >= 0)
                {
                    lastNonZero = i;
                    break;
                }
            }
            return lastNonZero == (size - 1);
        }

        for (var i = disk.Count - 1; i >= 0; i--)
        {
            if (disk[i] < 0) continue;
            if (IsCompact()) break;
            for (var j = 0; j < disk.Count; j++)
            {
                if (disk[j] < 0)
                {
                    disk[j] = disk[i];
                    disk[i] = -2;
                    break;
                }
            }
        }
        profiler.Stop();
        profiler.Print();

        long checksum = 0;
        for (var i = 0; i < disk.Count; i++)
        {
            if (disk[i] < 0) break;
            checksum += disk[i] * i;
        }
        return checksum.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var diskmap = input.First().Select(c => int.Parse("" + c)).ToList();
        var disk = new List<int>();
        var fileSizes = new List<(int index, int length)>();
        var fileIndex = 0;
        for (var i = 0; i < diskmap.Count; i++)
        {
            if (i % 2 == 0)
            {
                var index = i / 2;
                
                disk.AddRange(Enumerable.Repeat(index, diskmap[i]));
                fileSizes.Add((fileIndex, diskmap[i]));
                fileIndex += diskmap[i];
            }
            else
            {
                disk.AddRange(Enumerable.Repeat(-1, diskmap[i]));
                fileIndex += diskmap[i];
            }
        }
        PrintDisk();
        void PrintDisk() { 
            //Console.WriteLine(string.Join("", disk.Select(x => x < 0 ? x == -1 ? " " : " " : x.ToString()))); 
        }

        foreach (var fs in fileSizes.OrderByDescending(xs => xs.index))
        {
            foreach (var (start, length) in FindSpans())
            {
                if (start > fs.index) break;
                if (length < fs.length) continue;

                for (var i = 0; i < fs.length; i++)
                {
                    disk[start + i] = disk[fs.index + i];
                    disk[fs.index + i] = -2;
                }

                PrintDisk();
                break;
            }

            IEnumerable<(int, int)> FindSpans() {
                var length = 0;
                var spanStart = -1;
                for (var i = 1; i < disk.Count; i++)
                {
                    if (disk[i] >= 0)
                    {
                        if (length > 0)
                        {
                            yield return (spanStart, length);
                            length = 0;
                        }
                    }
                    else
                    {
                        if (length == 0)
                        {
                            spanStart = i;
                        }
                        length++;
                    }
                }
            }
        }

        
        PrintDisk();

        long checksum = 0;
        for (var i = 0; i < disk.Count; i++)
        {
            if (disk[i] < 0) continue;
            checksum += disk[i] * i;
        }
        return checksum.ToString();
    }

    [TestMethod]
    public void Day09_Part1_Example01()
    {
        var input = """
            2333133121414131402
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("1928", result);
    }
    
    [TestMethod]
    public void Day09_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day09), "2024"));
        Assert.AreEqual("6367087064415", result);
    }
    
    [TestMethod]
    public void Day09_Part2_Example01()
    {
        var input = """
            2333133121414131402
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("2858", result);
    }
    
    [TestMethod]
    public void Day09_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day09), "2024"));
        Assert.AreEqual("6390781891880", result);
    }
    
}
