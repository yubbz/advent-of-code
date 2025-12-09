using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AOC5;

// Working Answers
// P1: 529
// P2: 344260049617193

class Program
{
    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        fileName = "InputTest.txt";

        var input1ingredients = new List<long>();
        var input1StartEnd = new List<(long start, long end)>();
        var input1StartEndString = new List<string>();

        Console.WriteLine($"Advent Of Code 05");

        var ids = new List<long>();

        bool hasBlankPassed = false;
        foreach (String line in File.ReadLines(fileName))
        {
            if (line == "")
            {
                hasBlankPassed = true;
            }
            else
            {
                if (hasBlankPassed)
                {

                    input1ingredients.Add(long.Parse(line));

                }
                else
                {
                    {

                        input1StartEndString.Add(line);

                        string l = line.Substring(0, line.IndexOf('-'));
                        string r = line.Substring(line.IndexOf('-') + 1);

                        input1StartEnd.Add((long.Parse(l), long.Parse(r)));

                    }
                }

            }
        }


        int grandTotalP1 = ids.Count;
        long grandTotalP2 = 0;

        // P1
        foreach (var ingr in input1ingredients)
        {
            bool isFound = false;

            foreach (var list in input1StartEnd)
            {
                if (!isFound && list.start <= ingr && ingr <= list.end)
                {
                    isFound = true;
                    grandTotalP1++;
                    Console.WriteLine($"ingr {ingr} found in {list.start} -- {list.end}");
                }


                if (isFound)
                    continue;
            }

            Console.WriteLine($"ingr {ingr} not found.");
        }

                // pt2
        var normalizedList = NormalizeRanges(input1StartEnd);

        foreach (var item in normalizedList)
        {
            grandTotalP2 += item.end - item.start + 1;// incl.
        }

        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The part1 total end was: {grandTotalP1,22} ||");
        Console.WriteLine($"  || The part2 total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }


   public static List<(long start, long end)> NormalizeRanges(List<(long start, long end)> listToSort)
    {
        listToSort.Sort((a, b) => a.start.CompareTo(b.start));

        var result = new List<(long start, long end)>();
        var current = listToSort[0];

        foreach (var range in listToSort.Skip(1))
        {
            if (range.start <= current.end)
            {
                // Overlapping → extend the current range
                current.end = Math.Max(current.end, range.end);
            }
            else
            {
                // Non-overlapping → push current and reset
                result.Add(current);
                current = range;
            }
        }

        result.Add(current);

        return result;
    }

}