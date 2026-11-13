using System.Data;
using System.Net.Sockets;
using System.Xml.Schema;

namespace AOC9;

// Working Answers
// P1: 4771532800
// P2: 1544362560
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
        // STEP 1 — extract all unique X and Y coordinates
        var xs = poly.Select(p => p.x).Distinct().OrderBy(x => x).ToList();
        var ys = poly.Select(p => p.y).Distinct().OrderBy(y => y).ToList();

        // STEP 2 — map original coordinates to compressed grid indices
        var xMap = xs.Select((v, i) => new { v, i }).ToDictionary(a => a.v, a => a.i);
        var yMap = ys.Select((v, i) => new { v, i }).ToDictionary(a => a.v, a => a.i);

        int W = xs.Count;
        int H = ys.Count;

        // STEP 3 — create compressed grid
        int[,] grid = new int[W, H];

        // Draw fence
        for (int i = 0; i < poly.Count; i++)
        {
            var (x1, y1) = poly[i];
            var (x2, y2) = poly[(i + 1) % poly.Count];

            int cx1 = xMap[x1], cx2 = xMap[x2];
            int cy1 = yMap[y1], cy2 = yMap[y2];

            if (cx1 == cx2) // vertical
                for (int y = Math.Min(cy1, cy2); y <= Math.Max(cy1, cy2); y++)
                    grid[cx1, y] = 1;
            else if (cy1 == cy2) // horizontal
                for (int x = Math.Min(cx1, cx2); x <= Math.Max(cx1, cx2); x++)
                    grid[x, cy1] = 1;
            else
                throw new Exception("Diagonal edges not allowed.");
        }

        // STEP 4 — flood-fill outside
        Queue<(int x, int y)> q = new();
        void TryPush(int x, int y)
        {
            if (x < 0 || x >= W || y < 0 || y >= H) return;
            if (grid[x, y] != 0) return;
            grid[x, y] = 2;
            q.Enqueue((x, y));
        }

        for (int x = 0; x < W; x++)
        {
            TryPush(x, 0);
            TryPush(x, H - 1);
        }
        for (int y = 0; y < H; y++)
        {
            TryPush(0, y);
            TryPush(W - 1, y);
        }

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        while (q.Count > 0)
        {
            var (x, y) = q.Dequeue();
            for (int d = 0; d < 4; d++)
                TryPush(x + dx[d], y + dy[d]);
        }

        // STEP 5 — test all rectangles between red tiles
        long best = 0;
        int n = poly.Count;
        int totalPairs = n * (n - 1) / 2;
        int donePairs = 0;
        var startTime = DateTime.Now;

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                donePairs++;
                if (donePairs % 1000 == 0) // update every 1000 pairs
                {
                    double pct = (donePairs / (double)totalPairs) * 100;
                    var elapsed = DateTime.Now - startTime;
                    var estTotal = TimeSpan.FromTicks((long)(elapsed.Ticks / (donePairs / (double)totalPairs)));
                    var remaining = estTotal - elapsed;
                    Console.WriteLine($"Progress: {pct:F2}% | elapsed: {elapsed:mm\\:ss} | est remaining: {remaining:mm\\:ss}");
                }

                int x1 = Math.Min(xMap[poly[i].x], xMap[poly[j].x]);
                int x2 = Math.Max(xMap[poly[i].x], xMap[poly[j].x]);
                int y1 = Math.Min(yMap[poly[i].y], yMap[poly[j].y]);
                int y2 = Math.Max(yMap[poly[i].y], yMap[poly[j].y]);

                long area = (xs[x2] - xs[x1] + 1) * (ys[y2] - ys[y1] + 1);
                if (area <= best) continue;

                bool ok = true;
                for (int x = x1; x <= x2 && ok; x++)
                    for (int y = y1; y <= y2 && ok; y++)
                        if (grid[x, y] == 2)
                            ok = false;

                if (ok) best = area;
            }
        }

        return best;
    }





}