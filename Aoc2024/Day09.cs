using System;

namespace Advent_of_Code_2024;

[TestClass]
public class Day09
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var diskmap = input.First().Select(c => int.Parse("" + c)).ToList();
        var disk = new List<int>();
        for (var i = 0; i < diskmap.Count; i++)
        {
            if (i % 2 == 0)
            {
                var index = i / 2;
                disk.AddRange(Enumerable.Range(0, diskmap[i]).Select(_ => index));
            }
            else
            {
                disk.AddRange(Enumerable.Range(0, diskmap[i]).Select(_ => -1));
            }
        }
        //PrintDisk();
        void PrintDisk() => Console.WriteLine(string.Join("", disk.Select(x => x < 0 ? x == -1 ? "." : "-" : x.ToString())));
        bool IsCompact()
        {
            var flipCount = 0;
            var sign = disk[0] >= 0;
            for (var i = 1; i < disk.Count; i++)
            {
                var sign2 = disk[i] >= 0;
                if (sign != sign2)
                {
                    flipCount++;
                    sign = sign2;
                }
                if (flipCount > 1) return false;
            }
            return true;
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
        //PrintDisk();
        //0099811188827773336446555566.............
        //0099811188827773336446555566
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
        foreach (var line in input)
        {
        }
        return result.ToString();
    }
    
    [TestMethod]
    public void Day09_Part1_Example01()
    {
        var input = """
            2333133121414131402
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day09_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day09_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day09), "2024"));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day09_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day09_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day09_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day09), "2024"));
        Assert.AreEqual("", result);
    }
    
}