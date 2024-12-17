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
        return Part(program.ToArray(), registerA);
    }
    private static string Part(int[] program, long registerA, bool checkStartWith = false)
    {
        var result = new StringBuilder();
        var registers = new long[]
        {
            registerA,
            0,
            0
        };

        var instructionPointer = 0;
        var hasJumped = false;
        var outputIndex = 0;
        var operations = new Action[]
        {
            // adv
            () => {
                
                long x = Math.Min(ComboOperandValue(program[instructionPointer + 1]), 32);
                registers[0] = registers[0] >> (int) x;
            },
            
            // bxl
            () => {
                registers[1] ^= program[instructionPointer + 1];
            },

            // bst
            () => { 
                registers[1] = ComboOperandValue(program[instructionPointer + 1]) & 0b111;
            },

            // jnz
            () => { 
                if (registers[0] != 0)
                {
                    instructionPointer = program[instructionPointer + 1];
                    hasJumped = true;
                }
            },

            // bxc
            () => {
                registers[1] ^= registers[2];
            },

            // out
            () => { 
                var value = ComboOperandValue(program[instructionPointer + 1]) & 0b111;
                if (checkStartWith && (outputIndex >= program.Length || program[outputIndex] != value))
                {
                    instructionPointer = program.Length;
                }
                if (result.Length > 0)
                {
                    result.Append(',');
                }
                result.Append(value);
                outputIndex++;
            },

            // bdv
            () => {
                registers[1] = registers[0] >> (int) Math.Min(ComboOperandValue(program[instructionPointer + 1]), 32);
            },

            // cdv
            () => { 
                registers[2] = registers[0] >> (int) Math.Min(ComboOperandValue(program[instructionPointer + 1]), 32);
            },
        };

        long ComboOperandValue(int operand) => operand switch
        {
            >= 0 and <= 3 => operand,
            4 => registers[0],
            5 => registers[1],
            6 => registers[2],
            _ => throw new InvalidOperationException($"Unknown operand {operand}")
        };

        for (;;)
        {
            if (instructionPointer > program.Length - 1)
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
        string programString = string.Join(",", program);
        //Console.WriteLine($"programString={programString}");
        //100_000_000
        int[] programArray = program.ToArray();
        long minResult = long.MaxValue;
        var pResult = Parallel.For(0L, long.MaxValue, new ParallelOptions() { MaxDegreeOfParallelism = 6 }, (i, state) =>
        {
            var result = Part(programArray, i, checkStartWith: true);
            if (result == programString)
            {
                minResult = Math.Min(i, minResult);
                state.Break();
            }
        });
        if (minResult < long.MaxValue)
        {
            return minResult.ToString();
        }
        return "Not found";
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
    public void Day17_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day17), "2024"));
        Assert.AreEqual("1,7,2,1,4,1,5,4,0", result);
    }
    
    [TestMethod]
    public void Day17_Part2_Example01()
    {
        var input = """
            Register A: 2024
            Register B: 0
            Register C: 0

            Program: 0,3,5,4,3,0
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("117440", result);
    }
    
    [TestMethod]
    public void Day17_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day17), "2024"));
        Assert.AreEqual("", result);
    }
    
}
