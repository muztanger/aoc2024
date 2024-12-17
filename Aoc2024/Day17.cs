using System.Reflection.Emit;

namespace Advent_of_Code_2024;

[TestClass]
public class Day17
{
    private static string Part1(IEnumerable<string> input)
    {
        var program = new List<int>();
        var registerA = 0;
        foreach (var line in input)
        {
            if (line.Contains("Register A"))
            {
                var match = Regex.Match(line, @"Register (\w): (\d+)");
                var value = int.Parse(match.Groups[2].Value);
                registerA = value;
            }
            else if (line.Contains("Program"))
            {
                program = line.Substring("Program: ".Length).Split(',').Select(int.Parse).ToList();
            }
        }
        Console.WriteLine(string.Join(", ", program));
        return Part(program, registerA);
    }
    private static string Part(List<int> program, int registerA)
    {
        var result = new StringBuilder();
        var registers = new Dictionary<char, long>
        {
            ['A'] = registerA,
            ['B'] = 0,
            ['C'] = 0
        };

        var instructionPointer = 0;
        var hasJumped = false;

        var operations = new Dictionary<int, Action>
        {
            // adv
            { 0, () => {
                registers['A'] = registers['A'] / (long) Math.Pow(2, ComboOperandValue(program[instructionPointer + 1]));
            } },
            
            // bxl
            { 1, () => {
                registers['B'] ^= program[instructionPointer + 1];
            } },

            // bst
            { 2, () => { 
                registers['B'] = ComboOperandValue(program[instructionPointer + 1]) & 0b111;
            } },

            // jnz
            { 3, () => { 
                if (registers['A'] != 0)
                {
                    instructionPointer = program[instructionPointer + 1];
                    hasJumped = true;
                }
            } },

            // bxc
            { 4, () => {
                registers['B'] ^= registers['C'];
            } },

            // out
            { 5, () => { 
                if (result.Length > 0)
                {
                    result.Append(',');
                }
                result.Append(ComboOperandValue(program[instructionPointer + 1]) & 0b111);
            } },

            // bdv
            { 6, () => {
                registers['B'] = registers['A'] / (long) Math.Pow(2, ComboOperandValue(program[instructionPointer + 1]));
            } },

            // cdv
            { 7, () => { 
                registers['C'] = registers['A'] / (long) Math.Pow(2, ComboOperandValue(program[instructionPointer + 1]));
            } },
        };

        long ComboOperandValue(int operand) => operand switch
        {
            >= 0 and <= 3 => operand,
            4 => registers['A'],
            5 => registers['B'],
            6 => registers['C'],
            _ => throw new InvalidOperationException($"Unknown operand {operand}")
        };

        for (;;)
        {
            if (instructionPointer > program.Count - 1)
            {
                // program halts
                break;
            }
            //Console.WriteLine($"IP: {instructionPointer}, A: {registers['A']}, B: {registers['B']}, C: {registers['C']}");
            //Console.WriteLine($"   Resut: {result}");

            operations[program[instructionPointer]]();

            if (!hasJumped)
            {
                instructionPointer += 2;
            }
            hasJumped = false;
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
    public void Day17_Part1_Example01()
    {
        var input = """
            Register A: 729
            Register B: 0
            Register C: 0

            Program: 0,1,5,4,3,0
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("4,6,3,5,6,3,5,2,1,0", result);
    }
    
    [TestMethod]
    public void Day17_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day17_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day17), "2024"));
        Assert.AreEqual("1,7,2,1,4,1,5,4,0", result);
    }
    
    [TestMethod]
    public void Day17_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day17_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day17_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day17), "2024"));
        Assert.AreEqual("", result);
    }
    
}
