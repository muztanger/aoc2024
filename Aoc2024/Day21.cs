namespace Advent_of_Code_2024.NotFinished;

[TestClass]
public class Day21
{
    abstract class KeyPad
    {
        private Dictionary<char, Pos<int>> _keyPad;
        private Dictionary<Pos<int>, char> _keyPadReverse;
        private Box<int> _box;

        public Pos<int> AButton => _keyPad['A'];
        public Pos<int> Gap => _keyPad[' '];
        public Pos<int> KeyPos(char c) => new Pos<int>(_keyPad[c]);
        public char PosKey(Pos<int> pos) => _keyPadReverse[pos];

        public bool Contains(Pos<int> pos) => _box.Contains(pos) && pos != Gap;

        public KeyPad(string pattern)
        {
            _box = new Box<int>(Pos<int>.Zero);
            _keyPad = new Dictionary<char, Pos<int>>();
            _keyPadReverse = new Dictionary<Pos<int>, char>();
            var lines = pattern.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    Pos<int> pos = new Pos<int>(x, y);
                    _box.IncreaseToPoint(pos);
                    _keyPad[c] = pos;
                    _keyPadReverse[pos] = c;
                }
            }
        }
    }

    class NumericKeyPad : KeyPad
    {
        public NumericKeyPad() : base("""
                789
                456
                123
                 0A
                """)
        {
        }
    }

    class DirectionalKeyPad : KeyPad
    {
        public DirectionalKeyPad() : base("""
                 ^A
                <v>
                """)
        {
        }
    }

    class Robot
    {
        public Pos<int> Pos { get; set; }
        private readonly KeyPad _keyPad;
        private readonly Robot? _child = null;
        private readonly Dictionary<(char, char), List<string>> _pathMemo = [];

        public Robot(KeyPad keyPad)
        {
            _keyPad = keyPad;
            Pos = keyPad.AButton;
        }

        public Robot(KeyPad keyPad, Robot child)
        {
            _keyPad = keyPad;
            Pos = keyPad.AButton;
            _child = child;
        }

        public IEnumerable<string> FindShortestPaths(string code)
        {
            List<string> FindPaths(char startChar, char endChar)
            {
                if (_pathMemo.TryGetValue((startChar, endChar), out var paths))
                {
                    return paths;
                }
                var start = _keyPad.KeyPos(startChar);
                var end = _keyPad.KeyPos(endChar);
                var queue = new PriorityQueue<(Pos<int> pos, string path), int>();
                var minPathPos = new DefaultValueDictionary<Pos<int>, int>(() => int.MaxValue);
                var visited = new HashSet<string>();
                var subPaths = new List<string>();
                queue.Enqueue((start, ""), 0);
                while (queue.Count > 0)
                {
                    var (pos, path) = queue.Dequeue();

                    if (!_keyPad.Contains(pos))
                    {
                        continue;
                    }

                    if (visited.Contains(path))
                    {
                        continue;
                    }
                    visited.Add(path);

                    if (minPathPos[pos] < path.Length)
                    {
                        continue;
                    }
                    minPathPos[pos] = path.Length;

                    if (pos == end)
                    {
                        subPaths.Add(path + "A");
                    }

                    foreach (var dir in Pos<int>.CardinalDirections)
                    {
                        var next = pos + dir;
                        var dirChar = dir switch
                        {
                            Pos<int> p when p == Pos<int>.East => '>',
                            Pos<int> p when p == Pos<int>.South => 'v',
                            Pos<int> p when p == Pos<int>.West => '<',
                            Pos<int> p when p == Pos<int>.North => '^',
                            _ => throw new InvalidOperationException()
                        };
                        queue.Enqueue((next, path + dirChar), path.Length + 1);
                    }
                }
                _pathMemo.Add((startChar, endChar), subPaths);
                return subPaths;
            }
            foreach (var subPath in _child is not null ? _child.FindShortestPaths(code) : [code])
            {
                var pathParts = new List<List<string>>();
                foreach (var c in subPath)
                {
                    // find all shortests paths between Pos and c
                    var start = Pos;
                    //var end = _keyPad.KeyPos(endChar);
                    var subPaths = FindPaths(_keyPad.PosKey(start), c);
                    Pos = new Pos<int>(_keyPad.KeyPos(c));
                    int minLength = subPaths.Min(p => p.Length);
                    pathParts.Add(subPaths.Where(p => p.Length == minLength).ToList());
                }

                var stack = new Stack<(int partIndex, List<int> indexes)>();
                for (int i = 0; i < pathParts[0].Count; i++)
                {
                    stack.Push((0, [i]));
                }
                while (stack.Count > 0)
                {
                    var (partIndex, indexes) = stack.Pop();
                    if (partIndex == pathParts.Count - 1)
                    {
                        var result = new StringBuilder();
                        for (int i = 0; i < pathParts.Count; i++)
                        {
                            result.Append(pathParts[i][indexes[i]]);
                        }
                        yield return result.ToString();
                    }
                    else
                    {
                        for (int i = 0; i < pathParts[partIndex + 1].Count; i++)
                        {
                            stack.Push((partIndex + 1, new List<int>(indexes) {i}));
                        }
                    }
                }
            }
        }

        internal int LengthOfShortestSequence(string code)
        {
            int result = int.MaxValue;

            foreach (var path in FindShortestPaths(code))
            {
                result = Math.Min(result, path.Length);
            }
            
            return result;
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var numericRobot = new Robot(new NumericKeyPad());
        var depressurizedRobot = new Robot(new DirectionalKeyPad(), numericRobot);
        var coldRobot = new Robot(new DirectionalKeyPad(), depressurizedRobot);

        var result = 0;
        foreach (var code in input)
        {
            var numeric = int.Parse(code[..^1]);
            var length = coldRobot.LengthOfShortestSequence(code);
            result += numeric * length;
            Console.WriteLine($"{code} => {numeric} * {length} = {numeric * length}");
        }

        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var numericRobot = new Robot(new NumericKeyPad());
        var robot = new Robot(new DirectionalKeyPad(), numericRobot);
        for (int i = 0; i < 24; i++)
        {
            robot = new Robot(new DirectionalKeyPad(), robot);
        }

        var result = 0;
        foreach (var code in input)
        {
            var numeric = int.Parse(code[..^1]);
            var length = robot.LengthOfShortestSequence(code);
            result += numeric * length;
            Console.WriteLine($"{code} => {numeric} * {length} = {numeric * length}");
        }

        return result.ToString();
    }

    [TestMethod]
    public void TestNumericRobot()
    {
        var robot = new Robot(new NumericKeyPad());
        var paths = robot.FindShortestPaths("029A");
        Assert.IsTrue(paths.Contains("<A^A>^^AvvvA"));
        Assert.IsTrue(paths.Contains("<A^A^>^AvvvA"));
        Assert.IsTrue(paths.Contains("<A^A^^>AvvvA"));
        Assert.AreEqual(3, paths.Count());
    }

    [TestMethod]
    public void TestDirectionalRobot()
    {
        var numericRobot = new Robot(new NumericKeyPad());
        var robot = new Robot(new DirectionalKeyPad(), numericRobot);
        var paths = robot.FindShortestPaths("029A");
        Assert.IsTrue(paths.Contains("v<<A>>^A<A>AvA<^AA>A<vAAA>^A"));
    }

    [TestMethod]
    public void TestSecondDirectionalRobot()
    {
        var numericRobot = new Robot(new NumericKeyPad());
        var subRobot = new Robot(new DirectionalKeyPad(), numericRobot);
        var robot = new Robot(new DirectionalKeyPad(), subRobot);
        var paths = robot.FindShortestPaths("029A");
        Assert.IsTrue(paths.Contains("<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A"));
    }

    [TestMethod]
    public void Day21_Part1_Example01()
    {
        var input = """
            029A
            980A
            179A
            456A
            379A
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("126384", result);
    }
    
    [TestMethod]
    public void Day21_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day21), "2024"));
        Assert.AreEqual("188398", result);
    }
        
    [TestMethod]
    public void Day21_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day21), "2024"));
        Assert.AreEqual("", result);
    }
    
}
