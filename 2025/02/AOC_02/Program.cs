namespace AOC2;

// Working Answers
//-----------------------------------------------------
//|| The total end Part1 was:            55916882972 ||
//-----------------------------------------------------
//|| The total end Part2 was:            76169125915 ||
//-----------------------------------------------------

class Program
{
    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";

        foreach (String line in File.ReadLines(fileName))
        {
            IngestLine(line);
        }
    }


    public static void IngestLine(string line)
    {

        long totalP1 = 0, totalP2 = 0;
        List<string> parts = line.Split(",").ToList();
        foreach (string part in parts)
        {
            Console.WriteLine(part);
            var (p1, p2) = processPart(part);
            totalP1 += p1;
            totalP2 += p2;
        }

        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The total end Part1 was: {totalP1,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The total end Part2 was: {totalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }


    public static (long p1, long p2) processPart(string part)
    {
        List<string> section = part.Split("-").ToList();
        string strL = section[0];
        long lngL = long.Parse(strL);
        string strR = section[1];
        long lngR = long.Parse(strR);

        long offenderTotalPart1 = 0;
        long offenderTotalPart2 = 0;


        for (long i = lngL; i <= lngR; i++)
        {
            offenderTotalPart1 += checkNumberP1(i);
            offenderTotalPart2 += checkNumberP2(i);
        }

        return (offenderTotalPart1, offenderTotalPart2);
    }

    public static long checkNumberP1(long part)
    {
        string numberAsString = $"{part}";
        long throwback = 0;

        if (numberAsString.Length % 2 == 0)
        {
            string left = numberAsString.Substring(0, numberAsString.Length / 2);
            string right = numberAsString.Substring(numberAsString.Length / 2);
            if (left.Equals(right))
            {
                //Console.WriteLine($"Silly: {numberAsString}");
                throwback += part;
            }
        }
        return throwback;

    }


    public static long checkNumberP2(long part)
    {
        string numberAsString = $"{part}";
        long throwback = 0;

        for (int i = 2; i <= numberAsString.Length; i++)
        {
            if (numberAsString.Length % i == 0)
            {
                List<string> subStrs = [];

                int len = numberAsString.Length;
                int partLen = numberAsString.Length / i;


                for (int j = 0; j < i; j++)
                {
                    subStrs.Add(numberAsString.Substring(j * partLen, partLen));
                }

                if (AreEqual(subStrs))
                {
                    //Console.WriteLine($"Silly: {numberAsString}");
                    throwback += part;
                    break;
                }
            }
        }
        return throwback;
    }


    public static bool AreEqual(List<string> input)
    {
        bool areEqual = true;

        string first = input[0];
        foreach (string str in input)
        {
            if (!str.Equals(first))
            {
                areEqual = false;
                break;
            }
        }
        return areEqual;
    }

}