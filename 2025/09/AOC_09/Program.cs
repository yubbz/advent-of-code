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
        //fileName = "InputTest.txt";

        long grandTotalP1 = 0;
        long grandTotalP2 = 0;

        Console.WriteLine($"Advent Of Code 09");

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



    static long SolvePart2(List<(long x, long y)> poly)
    {
        // STEP 1 — compute bounding box
        long minX = poly.Min(p => p.x);
        long maxX = poly.Max(p => p.x);
        long minY = poly.Min(p => p.y);
        long maxY = poly.Max(p => p.y);

        // STEP 2 — store fence/edge tiles in a HashSet
        var fence = new HashSet<(long x, long y)>();
        for (int i = 0; i < poly.Count; i++)
        {
            var (x1, y1) = poly[i];
            var (x2, y2) = poly[(i + 1) % poly.Count];

            if (x1 == x2) // vertical edge
            {
                for (long y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
                    fence.Add((x1, y));
            }
            else if (y1 == y2) // horizontal edge
            {
                for (long x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
                    fence.Add((x, y1));
            }
            else
                throw new Exception("Diagonal edges not allowed in AoC input.");
        }

        // STEP 3 — flood-fill outside from bounding box edges
        var outside = new HashSet<(long x, long y)>();
        var queue = new Queue<(long x, long y)>();

        void TryPush(long x, long y)
        {
            if (x < minX || x > maxX || y < minY || y > maxY) return;
            if (outside.Contains((x, y))) return;
            if (fence.Contains((x, y))) return;

            outside.Add((x, y));
            queue.Enqueue((x, y));
        }

        // push edges
        for (long x = minX; x <= maxX; x++)
        {
            TryPush(x, minY);
            TryPush(x, maxY);
        }
        for (long y = minY; y <= maxY; y++)
        {
            TryPush(minX, y);
            TryPush(maxX, y);
        }

        long[] dx = { 1, -1, 0, 0 };
        long[] dy = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            for (int d = 0; d < 4; d++)
                TryPush(x + dx[d], y + dy[d]);
        }

        // STEP 4 — test all rectangles formed by fence posts
        long best = 0;
        for (int a = 0; a < poly.Count; a++)
        {
            for (int b = a + 1; b < poly.Count; b++)
            {
                long x1 = Math.Min(poly[a].x, poly[b].x);
                long x2 = Math.Max(poly[a].x, poly[b].x);
                long y1 = Math.Min(poly[a].y, poly[b].y);
                long y2 = Math.Max(poly[a].y, poly[b].y);

                long area = (x2 - x1 + 1) * (y2 - y1 + 1);
                if (area <= best) continue;

                bool ok = true;
                for (long x = x1; x <= x2 && ok; x++)
                    for (long y = y1; y <= y2 && ok; y++)
                        if (outside.Contains((x, y)))
                            ok = false;

                if (ok)
                    best = area;
            }
        }

        return best;
    }




}