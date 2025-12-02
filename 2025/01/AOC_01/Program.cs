namespace AOC1;

class Program
{
    const int STARTPOS = 50;
    const int SIZE = 100;

    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";

        int zeros = 0;
        int passingZeros = 0;
        int position = STARTPOS;
        Console.WriteLine($"The dial starts by pointing at {position}");

        foreach (Instruction instruction in File.ReadLines(fileName).Select(IngestLine))
        {
            var oldPos = position;
            var (newPosition, zerosPassed) = ProcessLine(instruction, position);
            position = newPosition;
            passingZeros += zerosPassed;
            var newZeroEnds = newPosition == 0 ? 1 : 0;
            zeros += newZeroEnds;

            Console.WriteLine($"Start {oldPos,3} + {(instruction.Symbol)}{instruction.Moves,6}: {position,6}." +
                " Zeros (Pass/End) ({passingZeros,3}/{newZeroEnds,1})");
        }

        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The total end-at-zero count was         : {zeros,4} ||");
        Console.WriteLine($"  || The total passingZeros (incl. above) was: {passingZeros,4} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }

    public record Instruction(Direction Direction, int Moves)
    {
        public string Symbol => Direction == Direction.Right ? "R" : "L";
        public int Apply(int start, int size) => Direction switch
        {
            Direction.Right => (start + Moves) % size,
            Direction.Left => ((start - Moves) % size + size) % size,
            _ => throw new InvalidOperationException()
        };
    }

    public enum Direction
    {
        Left,
        Right
    }

    public static Instruction IngestLine(string line)
    {
        var dir = line[0] switch
        {
            'R' or 'r' => Direction.Right,
            'L' or 'l' => Direction.Left,
            _ => throw new Exception($"Invalid direction: {line[0]}")
        };
        int moves = int.Parse(line[1..]);
        return new Instruction(dir, moves);
    }

    public static int CountCrossings(int start, int moves, Direction dir)
    {
        if (dir == Direction.Right)
        {
            return (start + moves) / SIZE;
        }
        else
        {
            int first = (start == 0 ? SIZE : start);
            return moves >= first ? 1 + (moves - first) / SIZE : 0;
        }
    }

    public static (int newPos, int passingZeros) ProcessLine(Instruction instruction, int start)
    {
        int newPos = instruction.Apply(start, SIZE);
        int passingZeros = CountCrossings(start, instruction.Moves, instruction.Direction);
        return (newPos, passingZeros);
    }
}