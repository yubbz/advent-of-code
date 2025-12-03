namespace AOC3;

// Working Answers
// P1: 17766
// P2: 176582889354075

class Program
{
    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";
        int grandTotalP1 = 0;
        long grandTotalP2 = 0;

        Console.WriteLine($"Advent Of Code 03");

        foreach (String line in File.ReadLines(fileName))
        {
            Console.Write($"{line}");

            var linetotalP1 = IngestLine1(line);
            grandTotalP1 += linetotalP1;

            // size= size - 1
            var linestring = IngestLine2(line, 11);
            long lineTotalP2 = long.Parse(linestring);
            grandTotalP2 += lineTotalP2;
            Console.WriteLine($" :- Pt1: '{linetotalP1}' / Pt2: '{lineTotalP2}'");
        }

        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The total end was: {grandTotalP1,22} ||");
        Console.WriteLine($"  || The total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }


    public static int IngestLine1(string line)
    {
        List<int> parts = line.Select(c => int.Parse(c.ToString())).ToList();
        int max = 0;

        for (int i = 0; i < line.Length - 1; i++)
        {
            for (int j = (i + 1); j < line.Length; j++)
            {
                int sum = int.Parse($"{parts[i]}{parts[j]}");
                if (sum > max)
                {
                    max = sum;
                }
            }
        }
        return max;
    }


    public static string IngestLine2(string line, int size)
    {

        var highestDigit = 0;
        var highestDigitIndex = 0;
        for (int i = 0; i < line.Length - (size); i++)
        {
            var intAtPos = int.Parse($"{line[i]}");

            if (intAtPos > highestDigit)
            {
                highestDigit = intAtPos;
                highestDigitIndex = i;
            }
        }

        if (size-- > 0)
        {
            return $"{highestDigit}{IngestLine2(line.Substring(highestDigitIndex + 1), size--)} ";
        }
        return $"{highestDigit}";
    }
}