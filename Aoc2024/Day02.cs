using Advent_of_Code_2024.Commons;
using System.Security.Cryptography;

namespace Advent_of_Code_2024;

[TestClass]
public class Day02
{
    class Report
    {
        private List<int> levels = new List<int>();
        public static Report Parse(string line)
        {
            Report report = new Report();
            report.levels = line.SplitTrim(' ').Select(int.Parse).ToList();
            return report;
        }
        public bool IsSafe()
        {
            return IsSafe(levels);
        }

        private bool IsSafe(List<int> ll)
        {
            var angle = Math.Sign(ll[1] - ll[0]);
            for (int i = 0; i < ll.Count - 1; i++)
            {
                var da = ll[i + 1] - ll[i];
                if (Math.Sign(da) != angle)
                {
                    return false;
                }
                if (Math.Abs(da) < 1 || Math.Abs(da) > 3)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsPrettySafe()
        {
            if (IsSafe())
            {
                return true;
            }
            for (int i = 0; i < levels.Count; i++)
            {
                var ll = new List<int>(levels);
                ll.RemoveAt(i);
                if (IsSafe(ll))
                {
                    return true;
                }
            }
            return false;
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = 0;
            
        foreach (var line in input)
        {
            var report = Report.Parse(line);
            if (report.IsSafe())
            {
                result++;
            }
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = 0;
        foreach (var line in input)
        {
            var report = Report.Parse(line);
            if (report.IsPrettySafe())
            {
                result++;
            }
        }
        return result.ToString();
    }

    [DataTestMethod]
    [DataRow("7 6 4 2 1", "1")]
    [DataRow("1 2 7 8 9", "0")]
    [DataRow("9 7 6 2 1", "0")]
    [DataRow("1 3 2 4 5", "0")]
    [DataRow("8 6 4 4 1", "0")]
    [DataRow("1 3 6 7 9", "1")]
    public void Day02_Part1_Examples(string input, string expected)
    {
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    public void Day02_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day02), "2024"));
        Assert.AreEqual("", result);
    }

    [DataTestMethod]
    [DataRow("7 6 4 2 1", "1")]
    [DataRow("1 2 7 8 9", "0")]
    [DataRow("9 7 6 2 1", "0")]
    [DataRow("1 3 2 4 5", "1")]
    [DataRow("8 6 4 4 1", "1")]
    [DataRow("1 3 6 7 9", "1")]
    public void Day02_Part2_Examples(string input, string expected)
    {
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    public void Day02_Part2()
    {
        IEnumerable<string> input = Common.DayInput(nameof(Day02), "2024");
        var timer = new System.Diagnostics.Stopwatch();

        long mem = GC.GetTotalAllocatedBytes();
        timer.Start();
        var result = Part2(input);
        timer.Stop();
        mem = GC.GetTotalAllocatedBytes() - mem;
        Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds} ms");
        Console.WriteLine($"Allocated bytes: {mem / 1024.0:N2} kb");

        // Enumerators/Linq
        //    There were 1000 lines.
        //    Elapsed time: 7 ms
        //    Allocated bytes: 2,297.09 kb

        // This implementation
        //    There were 1000 lines.
        //    Elapsed time: 6 ms
        //    Allocated bytes: 821.55 kb

        Assert.AreEqual("488", result);
    }
    
}
