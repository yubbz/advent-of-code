using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Xml.Schema;
using Microsoft.Z3;


namespace AOC10;

// Working Answers
// P1: 411
// P2: 
class Program
{
   static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";

        long grandTotalP1 = 0;
        long grandTotalP2 = 0;

        Console.WriteLine($"Advent Of Code 10");

        int count = 0;
        foreach (String line in File.ReadLines(fileName))
        {

            long answer = calcPart1(line);
            Console.WriteLine($"{count,3}: Anwer is {answer} for {line}");

            grandTotalP1 += answer;
            count++;
        }

        Console.WriteLine("Part 1 done.");

        count = 0;
        foreach (String line in File.ReadLines(fileName))
        {

            long answer = calcPart2(line);
            Console.WriteLine($"{count,3}: Anwer is {answer} for {line}");

            grandTotalP2 += answer;
            count++;
        }

        Console.WriteLine("Part 2 done.");

        // OUTPUT
        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The part1 total end was: {grandTotalP1,22} ||");
        Console.WriteLine($"  || The part2 total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }


    private static long calcPart1(string line)
    {
        // ------------------------
        // PARSE TARGET MASK
        // ------------------------
        var diagMatch = System.Text.RegularExpressions.Regex.Match(line, @"\[(.*?)\]");
        string diag = diagMatch.Groups[1].Value;
        int n = diag.Length;

        ulong target = 0;
        for (int i = 0; i < n; i++)
            if (diag[i] == '#')
                target |= 1UL << i; // leftmost = bit 0, matches button indices

        // ------------------------
        // PARSE BUTTON MASKS
        // ------------------------
        string beforeCurlies = line.Split('{')[0];
        var buttonMatches = System.Text.RegularExpressions.Regex.Matches(beforeCurlies, @"\((.*?)\)");
        List<ulong> buttons = new();
        foreach (System.Text.RegularExpressions.Match m in buttonMatches)
        {
            string inside = m.Groups[1].Value.Trim();
            if (inside.Length == 0) continue;
            ulong mask = 0;
            foreach (var num in inside.Split(',', StringSplitOptions.RemoveEmptyEntries))
                mask |= 1UL << int.Parse(num); // same indexing as target
            buttons.Add(mask);
        }

        // ------------------------
        // BFS TO FIND MIN PRESSES
        // ------------------------
        int maxState = 1 << n; // n <= 32
        bool[] visited = new bool[maxState];
        int[] dist = new int[maxState];
        var q = new Queue<int>();
        q.Enqueue(0);
        visited[0] = true;
        dist[0] = 0;

        while (q.Count > 0)
        {
            int state = q.Dequeue();
            if ((ulong)state == target)
                return dist[state];

            foreach (var b in buttons)
            {
                int next = (int)((ulong)state ^ b);
                if (!visited[next])
                {
                    visited[next] = true;
                    dist[next] = dist[state] + 1;
                    q.Enqueue(next);
                }
            }
        }

        return -1; // should never happen
    }



private static long calcPart2(string line)
{
    // PARSE BUTTONS
    string beforeCurlies = line.Split('{')[0];
    var buttonMatches = System.Text.RegularExpressions.Regex.Matches(beforeCurlies, @"\((.*?)\)");

    var buttons = new List<int[]>();
    foreach (System.Text.RegularExpressions.Match m in buttonMatches)
    {
        string inside = m.Groups[1].Value.Trim();
        if (inside.Length == 0) continue;
        int[] btn = inside.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => int.Parse(s.Trim()))
                          .ToArray();
        buttons.Add(btn);
    }

    // PARSE target counters
    var after = line.Split('{')[1].TrimEnd('}');
    int[] target = after.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => int.Parse(s.Trim()))
                        .ToArray();

    int numButtons = buttons.Count;
    int numCounters = target.Length;

    using var ctx = new Context();
    // create integer variables x0, x1, ..., xN-1
    IntExpr[] xs = new IntExpr[numButtons];
    for (int i = 0; i < numButtons; i++)
    {
        xs[i] = (IntExpr)ctx.MkIntConst($"x{i}");
    }

    var solver = ctx.MkOptimize();

    // add constraints: x_i >= 0
    foreach (var xi in xs)
    {
        solver.Add(ctx.MkGe(xi, ctx.MkInt(0)));
    }

    // for each counter, sum of button‑presses covering it == target[counter]
    for (int c = 0; c < numCounters; c++)
    {
        var sumTerms = new List<ArithExpr>();
        for (int b = 0; b < numButtons; b++)
        {
            if (buttons[b].Contains(c))
                sumTerms.Add(xs[b]);
        }
        ArithExpr sum = sumTerms.Count > 0 ? ctx.MkAdd(sumTerms.ToArray()) : ctx.MkInt(0);
        solver.Add(ctx.MkEq(sum, ctx.MkInt(target[c])));
    }

    // objective: minimize total presses = sum(x_i)
    ArithExpr total = ctx.MkAdd(xs.Select(x => (ArithExpr)x).ToArray());
    solver.MkMinimize(total);

    if (solver.Check() == Status.SATISFIABLE)
    {
        var model = solver.Model;
        long tot = (model.Evaluate(total) as IntNum)!.Int64;
        return tot;
    }
    else
    {
        // no solution
        return -1;
    }
}



}