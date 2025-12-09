using System.Data;
using System.Net.Sockets;
using System.Xml.Schema;

namespace AOC9;

// Working Answers
// P1: 4771532800
// P2: 
class Program
{
    static Dictionary<(int row, int col), long> memo = new();
    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        fileName = "InputTest.txt";

        long grandTotalP1 = 0;
        long grandTotalP2 = 0;

        Console.WriteLine($"Advent Of Code 09");

        var beams = new List<string>();

        List<(long x, long y)> input = [];

        foreach (String line in File.ReadLines(fileName))
        {
            var linesplit = line.Split(',');
            input.Add((long.Parse(linesplit[0]), long.Parse(linesplit[1])));
        }

        long max = 0;

        for (int i = 0; i < input.Count; i++)
        {
            for (int j = i; j < input.Count; j++)
            {
                long xy = Math.Abs(input[i].x - input[j].x) + 1;
                long yy = Math.Abs(input[i].y - input[j].y) + 1;


                long multiply = xy * yy;

                Console.WriteLine(multiply);

                if (multiply > max)
                    max = multiply;
            }

        }

        grandTotalP1 = max;

        Console.WriteLine("p1dun");


        grandTotalP2 = SolvePart2(input);



        // OUTPUT
        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The part1 total end was: {grandTotalP1,22} ||");
        Console.WriteLine($"  || The part2 total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }

   
    static long SolvePart2(List<(long x, long y)> red)
    {
      
        return 9000;
    }


}