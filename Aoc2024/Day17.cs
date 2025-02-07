using Newtonsoft.Json.Linq;
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
    private static string Part(int[] program, int registerA, bool checkStartWith = false)
    {
        var result = new StringBuilder();
        var registers = new int[]
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
                
                registers[0] = registers[0] >> ComboOperandValue(program[instructionPointer + 1]);
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
                registers[1] = registers[0] >> ComboOperandValue(program[instructionPointer + 1]);
            },

            // cdv
            () => {
                registers[2] = registers[0] >> ComboOperandValue(program[instructionPointer + 1]);
            },
        };

        int ComboOperandValue(int operand) => operand switch
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
        var programList = new List<int>();
        foreach (var line in input)
        {
            if (line.Contains("Program"))
            {
                programList = line.Substring("Program: ".Length).Split(',').Select(int.Parse).ToList();
            }
        }
        string programString = string.Join(",", programList);
        //Console.WriteLine($"programString={programString}");
        //100_000_000
        int[] program = programList.ToArray();
        long minResult = long.MaxValue;
        //117440
        //var i = 117440;
        //var pResult = Parallel.For(0L, long.MaxValue, (i, state) =>
        //111011100111110100
        // 01011110111110100 (11)
        //     ...0111110100 <-- all numbers with 8 correct outputs ends with this

        //TODO solve by iteratively searching for the correct input number by checking the common suffixes of input binary numbers for the cases where the output is most correct

        //i = 101000100110000100000000101001011110111110100, outputIndex = 13, value = 3
        //i = 101000100110000100000010101001011110111110100, outputIndex = 13, value = 3
        //i = 101000100110000100000010101101011110111110100, outputIndex = 13, value = 3
        //i = 101000100110000100000100101001011110111110100, outputIndex = 13, value = 3


        long printThreshold = 13;
        for (long j = 0; j < 400_000_000; j++)
        {
            //long i = (0b1010001001100001 << 5) | j;
            long i = (j << 17) | 0b01011110111110100;
            //Console.WriteLine($"i={Convert.ToString(i, 2)}");
            var isMatch = true;
            var registers = new long[]
            {
                i,
                0,
                0
            };

            var instructionPointer = 0;
            var hasJumped = false;
            var outputIndex = 0;

            for (;;)
            {
                if (instructionPointer > program.Length - 1)
                {
                    // program halts
                    break;
                }

                switch (program[instructionPointer])
                {
                    case 0: // adv
                    {
                        registers[0] = registers[0] >> (program[instructionPointer + 1] switch
                        {
                            >= 0 and <= 3 => program[instructionPointer + 1],
                            4 => (int)registers[0],
                            5 => (int)registers[1],
                            6 => (int)registers[2],
                            _ => throw new InvalidOperationException($"Unknown operand {program[instructionPointer + 1]}")
                        });
                        break;

                    }
                    case 1: // bxl
                    {
                        registers[1] ^= program[instructionPointer + 1];
                        break;
                    }
                    case 2: // bst
                    {
                        registers[1] = (program[instructionPointer + 1] switch
                        {
                            >= 0 and <= 3 => program[instructionPointer + 1],
                            4 => registers[0],
                            5 => registers[1],
                            6 => registers[2],
                            _ => throw new InvalidOperationException($"Unknown operand {program[instructionPointer + 1]}")
                        }) & 0b111;
                        break;
                    }
                    case 3: // jnz
                    {
                        if (registers[0] != 0)
                        {
                            instructionPointer = program[instructionPointer + 1];
                            hasJumped = true;
                        }
                        break;
                    }
                    case 4: // bxc
                    {
                        registers[1] ^= registers[2];
                        break;
                    }
                    case 5: // out
                    {
                        var value = program[instructionPointer + 1] switch
                        {
                            >= 0 and <= 3 => program[instructionPointer + 1],
                            4 => registers[0],
                            5 => registers[1],
                            6 => registers[2],
                            _ => throw new InvalidOperationException($"Unknown operand {program[instructionPointer + 1]}")
                        } & 0b111;
                        if (outputIndex >= program.Length || program[outputIndex] != value)
                        {
                            isMatch = false;
                            instructionPointer = program.Length;
                        }
                        if (outputIndex >= printThreshold && isMatch)
                        {
                            Console.WriteLine($"i={Convert.ToString(i, 2)}, outputIndex={outputIndex}, value={value}");
                        }
                        outputIndex++;
                        break;
                    }
                    case 6: // bdv
                    {
                        registers[1] = registers[0] >> (program[instructionPointer + 1]  switch
                        {
                            >= 0 and <= 3 => program[instructionPointer + 1],
                            4 => (int)registers[0],
                            5 => (int)registers[1],
                            6 => (int)registers[2],
                            _ => throw new InvalidOperationException($"Unknown operand {program[instructionPointer + 1]}")
                        });
                        break;
                    }
                    case 7: // cdv
                    {
                        registers[2] = registers[0] >> (program[instructionPointer + 1] switch
                        {
                            >= 0 and <= 3 => program[instructionPointer + 1],
                            4 => (int)registers[0],
                            5 => (int)registers[1],
                            6 => (int)registers[2],
                            _ => throw new InvalidOperationException($"Unknown operand {program[instructionPointer + 1]}")
                        });
                        break;
                    }
                    default:
                    {
                        throw new InvalidOperationException($"Unknown operation {program[instructionPointer]}");
                    }
                };

                if (!hasJumped)
                {
                    instructionPointer += 2;
                }
                hasJumped = false;
            }
            if (isMatch && program.Length == outputIndex)
            {
                minResult = Math.Min(i, minResult);
                break;
            }
        }//);
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
