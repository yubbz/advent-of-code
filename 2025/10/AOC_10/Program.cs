using System.Data;
using System.Net.Sockets;
using System.Xml.Schema;

namespace AOC10;

// Working Answers
// P1: 
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

        Console.WriteLine($"Advent Of Code 10");

        List<(long x, long y)> input = [];

        foreach (String line in File.ReadLines(fileName))
        {
            var linesplit = line.Split(',');
            input.Add((long.Parse(linesplit[0]), long.Parse(linesplit[1])));
        }



        grandTotalP1 = 0;
        Console.WriteLine("Part 1 done.");


        grandTotalP2 = 0;
        Console.WriteLine("Part 2 done.");



        // OUTPUT
        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The part1 total end was: {grandTotalP1,22} ||");
        Console.WriteLine($"  || The part2 total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }
}