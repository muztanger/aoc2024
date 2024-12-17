using System.Reflection.Emit;

namespace Advent_of_Code_2024;

[TestClass]
public class Day17
{
    private static string Part1(IEnumerable<string> input)
    {
        var registers = new Dictionary<char, long>();
        var program = new List<int>();
        var result = new StringBuilder();
        foreach (var line in input)
        {
            if (line.Contains("Register"))
            {
                var match = Regex.Match(line, @"Register (\w): (\d+)");
                var register = match.Groups[1].Value[0];
                var value = int.Parse(match.Groups[2].Value);
                registers[register] = value;
            }
            else if (line.Contains("Program"))
            {
                program = line.Substring("Program: ".Length).Split(',').Select(int.Parse).ToList();
            }
        }
        Console.WriteLine(string.Join(", ", registers.Select(kv => $"{kv.Key}: {kv.Value}")));
        Console.WriteLine(string.Join(", ", program));

        var instructionPointer = 0;
        var hasJumped = false;

        var operations = new Dictionary<int, Action>
        {
            // adv
            { 0, () => {
                // numenator is value of register A
                // The denominator is found by raising 2 to the power of the instruction's combo operand.
                // The result of the division operation is truncated to an integer and then written to the A register.
                registers['A'] = registers['A'] / (long) Math.Pow(2, ComboOperandValue(program[instructionPointer + 1]));
            } },
            
            // bxl
            { 1, () => {
                // The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand, then stores the result in register B.
                registers['B'] ^= program[instructionPointer + 1];
            } },

            // bst
            { 2, () => { 
                // The bst instruction (opcode 2) calculates the value of its combo operand modulo 8 (thereby keeping only its lowest 3 bits),
                // then writes that value to the B register.
                registers['B'] = ComboOperandValue(program[instructionPointer + 1]) & 0b111;
            } },

            // jnz
            { 3, () => { 
                // The jnz instruction (opcode 3) does nothing if the A register is 0. However, if the A register is not zero, it jumps by setting the
                // instruction pointer to the
                // value of its literal operand; if this instruction jumps, the instruction pointer is not increased by 2 after this instruction.
                if (registers['A'] != 0)
                {
                    instructionPointer = program[instructionPointer + 1];
                    hasJumped = true;
                }
            } },

            // bxc
            { 4, () => {
                // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C,
                // then stores the result in register B. (For legacy reasons, this instruction reads an operand but ignores it.)
                registers['B'] ^= registers['C'];
            } },

            // out
            { 5, () => { 
                // The out instruction (opcode 5) calculates the value of its combo operand modulo 8, then outputs that value.
                // (If a program outputs multiple values, they are separated by commas.)
                if (result.Length > 0)
                {
                    result.Append(',');
                }
                result.Append(ComboOperandValue(program[instructionPointer + 1]) & 0b111);
            } },

            // bdv
            { 6, () => {
                // The bdv instruction (opcode 6) works exactly like the adv instruction except that the result is stored in the B register.
                // (The numerator is still read from the A register.)
                registers['B'] = registers['A'] / (long) Math.Pow(2, ComboOperandValue(program[instructionPointer + 1]));
            } },

            // cdv
            { 7, () => { 
                // The cdv instruction (opcode 7) works exactly like the adv instruction except that the result is stored in the C register.
                // (The numerator is still read from the A register.)
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

        for (; ; )
        {
            if (instructionPointer > program.Count - 1)
            {
                // program halts
                break;
            }
            Console.WriteLine($"IP: {instructionPointer}, A: {registers['A']}, B: {registers['B']}, C: {registers['C']}");
            Console.WriteLine($"   Resut: {result}");

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
        Assert.AreEqual("", result);
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
