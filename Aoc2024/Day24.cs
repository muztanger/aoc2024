using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace Advent_of_Code_2024.NotFinished;

[TestClass]
public class Day24
{
    enum Operation
    {
        AND,
        OR,
        XOR,
    }

    class Wire
    {
        required public string Name { get; set; }
        public bool? Value { get; set; } = null;

        override public bool Equals(object? obj)
        {
            if (obj is Wire other)
            {
                return Name == other.Name;
            }
            return false;
        }

        override public int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }

    class Gate
    {
        required public string Name { get; set; }
        public Operation Operation { get; set; }
        required public Wire Input1 { get; set; }
        required public Wire Input2 { get; set; }
        required public Wire Output { get; set; }

        public bool WaitingForInput => Input1.Value == null || Input2.Value == null;
        public bool TryCalculate()
        {
            if (WaitingForInput)
            {
                return false;
            }
            Output.Value = Operation switch
            {
                Operation.AND => Input1.Value & Input2.Value,
                Operation.OR => Input1.Value | Input2.Value,
                Operation.XOR => Input1.Value ^ Input2.Value,
                _ => throw new Exception("Unknown operation")
            };
            return true;
        }
    }

    private static void ParseInputToWiresAndGates(IEnumerable<string> input, out Dictionary<string, Wire> wires, out List<Gate> gates)
    {
        wires = new Dictionary<string, Wire>();
        gates = new List<Gate>();
        foreach (var line in input)
        {
            if (line.Contains(':'))
            {
                var parts = line.Split(':');
                var wire = new Wire { Name = parts[0].Trim(), Value = parts[1].Trim() == "1" };
                wires[wire.Name] = wire;
            }
            else if (line.Contains("->"))
            {
                //x00 AND y00 -> z00
                var parts = line.Split(" ");
                var operation = parts[1] switch
                {
                    "AND" => Operation.AND,
                    "OR" => Operation.OR,
                    "XOR" => Operation.XOR,
                    _ => throw new Exception("Unknown operation")
                };
                var input1 = parts[0].Trim();
                if (!wires.TryGetValue(input1, out Wire? wire1))
                {
                    wire1 = new Wire { Name = input1, Value = null };
                    wires[input1] = wire1;
                }
                var input2 = parts[2].Trim();
                if (!wires.TryGetValue(input2, out Wire? wire2))
                {
                    wire2 = new Wire { Name = input2, Value = null };
                    wires[input2] = wire2;
                }
                var outputName = parts[parts.Length - 1].Trim();
                if (!wires.TryGetValue(outputName, out Wire? output))
                {
                    output = new Wire { Name = outputName, Value = null };
                    wires[outputName] = output;
                }
                gates.Add(new Gate { Name = outputName, Operation = operation, Input1 = wire1, Input2 = wire2, Output = output });
            }
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        ParseInputToWiresAndGates(input, out var wires, out var gates);
        int count = 0;
        while (gates.Where(g => g.Output.Name.StartsWith("z")).Any(g => !g.TryCalculate()))
        {
            count++;
            gates.ForEach(g => g.TryCalculate());
        }
        long value = BitsToLong(wires, 'z');
        return value.ToString();
    }

    private static long BitsToLong(Dictionary<string, Wire> wires, char letter)
    {
        var stack = new Stack<char>();
        int i = 0;
        while (wires.TryGetValue($"{letter}{i:00}", out Wire? wire))
        {
            stack.Push(wire.Value == true ? '1' : '0');
            i++;
        }

        string value = string.Concat(stack);
        return Convert.ToInt64(value, 2);
    }

    private static string Part2(IEnumerable<string> input)
    {
        ParseInputToWiresAndGates(input, out var originalWires, out var originalGates);
        var outputs = new List<string>(originalGates.Select(g => g.Output.Name));

        // Testing everything is too slow
        // 222 choose 8 = 128,795,283,347,445
        long xValue = BitsToLong(originalWires, 'x');
        long yValue = BitsToLong(originalWires, 'y');
        var zExpected = xValue + yValue;
        var zExpectedString = Convert.ToString(zExpected, 2);

        // find the four pairs of swapped outputs
        var stack = new Stack<List<string>>();
        var visited = new HashSet<string>();
        var result = long.MinValue;
        do
        {
            ParseInputToWiresAndGates(input, out var wires, out var gates);

            if (stack.Count > 0)
            {
                var swap = stack.Pop();
                var visitedString = string.Join(',', swap.OrderBy(s => s));
                if (visited.Contains(visitedString))
                {
                    continue;
                }
                for (int i = 0; i < swap.Count; i += 2)
                {
                    var output1 = swap[i];
                    var output2 = swap[i + 1];
                    var gate1 = gates.First(g => g.Output.Name == output1);
                    var gate2 = gates.First(g => g.Output.Name == output2);
                    (gate2.Output, gate1.Output) = (gate1.Output, gate2.Output);
                }
            }

            while (gates.Where(g => g.Output.Name.StartsWith("z")).Any(g => !g.TryCalculate()))
            {
                gates.ForEach(g => g.TryCalculate());
            }

            for (int i = 0; wires.TryGetValue($"z{i:00}", out Wire? wire); i++)
            {
                Assert.IsTrue(wire.Value.HasValue); // fail or only disregard?
                if (wire.Value != (zExpectedString[i] == '1'))
                {
                    // find all the outputs leading to this wire
                    var outputsToSwap = new List<string>();
                    //var stack = new Stack<string>();
                    foreach (var gate in gates)
                    {
                        if (gate.Output.Name == wire.Name)
                        {
                            outputsToSwap.Add(gate.Input1.Name);
                            outputsToSwap.Add(gate.Input2.Name);
                        }
                    }
                }

            }

            result = BitsToLong(wires, 'z');
        }
        while (stack.Count > 0 && zExpected != result);

        return (-1).ToString();
    }
    
    [TestMethod]
    public void Day24_Part1_Example01()
    {
        var input = """
            x00: 1
            x01: 1
            x02: 1
            y00: 0
            y01: 1
            y02: 0

            x00 AND y00 -> z00
            x01 XOR y01 -> z01
            x02 OR y02 -> z02
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("4", result);
    }
    
    [TestMethod]
    public void Day24_Part1_Example02()
    {
        var input = """
            x00: 1
            x01: 0
            x02: 1
            x03: 1
            x04: 0
            y00: 1
            y01: 1
            y02: 1
            y03: 1
            y04: 1

            ntg XOR fgs -> mjb
            y02 OR x01 -> tnw
            kwq OR kpj -> z05
            x00 OR x03 -> fst
            tgd XOR rvg -> z01
            vdt OR tnw -> bfw
            bfw AND frj -> z10
            ffh OR nrd -> bqk
            y00 AND y03 -> djm
            y03 OR y00 -> psh
            bqk OR frj -> z08
            tnw OR fst -> frj
            gnj AND tgd -> z11
            bfw XOR mjb -> z00
            x03 OR x00 -> vdt
            gnj AND wpb -> z02
            x04 AND y00 -> kjc
            djm OR pbm -> qhw
            nrd AND vdt -> hwm
            kjc AND fst -> rvg
            y04 OR y02 -> fgs
            y01 AND x02 -> pbm
            ntg OR kjc -> kwq
            psh XOR fgs -> tgd
            qhw XOR tgd -> z09
            pbm OR djm -> kpj
            x03 XOR y03 -> ffh
            x00 XOR y04 -> ntg
            bfw OR bqk -> z06
            nrd XOR fgs -> wpb
            frj XOR qhw -> z04
            bqk OR frj -> z07
            y03 OR x01 -> nrd
            hwm AND bqk -> z03
            tgd XOR rvg -> z12
            tnw OR pbm -> gnj
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("2024", result);
    }
    
    [TestMethod]
    public void Day24_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day24), "2024"));
        Assert.AreEqual("49520947122770", result);
    }
    
    [TestMethod]
    public void Day24_Part2_Example01()
    {
        var input = """
            x00: 0
            x01: 1
            x02: 0
            x03: 1
            x04: 0
            x05: 1
            y00: 0
            y01: 0
            y02: 1
            y03: 1
            y04: 0
            y05: 1

            x00 AND y00 -> z05
            x01 AND y01 -> z02
            x02 AND y02 -> z01
            x03 AND y03 -> z03
            x04 AND y04 -> z04
            x05 AND y05 -> z00
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day24_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day24_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day24), "2024"));
        Assert.AreEqual("", result);
    }
    
}
