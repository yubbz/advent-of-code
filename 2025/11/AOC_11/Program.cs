using System.Data;
using System.Net;

namespace AOC11;

// Working Answers
// P1:
// P2: 
class Program
{
    static long dfsSteps = 0;
    static long dfsReportEvery = 200000;  // reduce noise
    static Dictionary<(string node, bool a, bool b), long> memoP2 = new();

    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";
        //fileName = "InputTest2.txt";

        long grandTotalP1 = 0;
        long grandTotalP2 = 0;

        Console.WriteLine($"Advent Of Code 11");

        List<Route> routeList = [];

        int count = 0;
        foreach (String line in File.ReadLines(fileName))
        {
            routeList.Add(new Route(line));

            count++;
        }

        var graph = routeList.ToDictionary(r => r.address, r => r.destinations);
       // grandTotalP1 = CountPaths("you", "out", graph, new HashSet<string>());

        Console.WriteLine($"Part 1 done.");

        foreach (var r in routeList)
        {
            Console.WriteLine($"p1: '{r.address}'; --> {string.Join(",", r.destinations)}");
        }
        dfsSteps = 0;
        memoP2.Clear();

        grandTotalP2 = CountPathsWithRequiredNodesMemo(
            "svr",
            "out",
            "dac",
            "fft",
            graph,
            new HashSet<string>(),
            false,
            false
        );

        Console.WriteLine($"Part 2 complete. Total DFS steps: {dfsSteps:N0}");
        Console.WriteLine($"Memo entries: {memoP2.Count:N0}");

        Console.WriteLine("Part 2 done.");

        // OUTPUT
        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The part1 total end was: {grandTotalP1,22} ||");
        Console.WriteLine($"  || The part2 total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }

    static long CountPaths(string current, string target, Dictionary<string, List<string>> graph, HashSet<string> visited)
    {
        if (current == target)
            return 1;

        long total = 0;

        // Prevent cycles
        if (visited.Contains(current))
            return 0;

        visited.Add(current);

        if (graph.TryGetValue(current, out var nextList))
        {
            foreach (var next in nextList)
            {
                total += CountPaths(next, target, graph, visited);
            }
        }

        visited.Remove(current);
        return total;
    }

    static long CountPathsWithRequiredNodesMemo(
       string current,
       string target,
       string requiredA,
       string requiredB,
       Dictionary<string, List<string>> graph,
       HashSet<string> visited,
       bool visitedA,
       bool visitedB)
    {
        // Memo key
        var key = (current, visitedA, visitedB);

        // Check memo first
        if (memoP2.TryGetValue(key, out long memoValue))
            return memoValue;

        // Progress output
        dfsSteps++;
        if (dfsSteps % dfsReportEvery == 0)
            Console.WriteLine($"DFS progress: {dfsSteps:N0} steps... (memo size={memoP2.Count:N0})");

        // Reached target?
        if (current == target)
        {
            long result = (visitedA && visitedB) ? 1 : 0;
            memoP2[key] = result;
            return result;
        }

        // Avoid cycles
        if (visited.Contains(current))
        {
            memoP2[key] = 0;
            return 0;
        }

        visited.Add(current);

        long total = 0;

        if (graph.TryGetValue(current, out var nextList))
        {
            foreach (var next in nextList)
            {
                bool nextA = visitedA || (next == requiredA);
                bool nextB = visitedB || (next == requiredB);

                total += CountPathsWithRequiredNodesMemo(
                    next,
                    target,
                    requiredA,
                    requiredB,
                    graph,
                    visited,
                    nextA,
                    nextB
                );
            }
        }

        visited.Remove(current);

        // Save in memo
        memoP2[key] = total;
        return total;
    }




    public class Route
    {
        public string address { get; set; }
        public List<string> destinations { get; set; }

        public Route(string line)
        {
            string p1 = line.Substring(0, 3);
            string p2 = line.Substring(5);

            var parts = p2.Split(' ').ToList();

            address = p1;
            destinations = parts;


        }

        public override string ToString()
        {
            return $"p1: '{address}'; --> {string.Join(",", destinations)}";
        }
    }
}