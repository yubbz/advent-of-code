using System.Diagnostics;

namespace AOC12;

// Working Answers
// P1:
// P2: 
class Program
{
    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";

        long grandTotalP1 = 0;
        long grandTotalP2 = 0;

        Console.WriteLine($"Advent Of Code 12");

        // -------------------------------
        // LOAD SHAPES FIRST
        // -------------------------------
        var shapes = LoadShapes(fileName);
        Console.WriteLine($"Loaded {shapes.Count} shapes.\n");

        foreach (var s in shapes)
            Console.WriteLine(s.ToString());

        // -------------------------------
        // NOW LOAD REGIONS
        // skip the first N lines used by shapes
        // -------------------------------
        int shapeLines = CountShapeLines(fileName);

        Console.WriteLine();
        Console.WriteLine("REGIONS:");

        List<Region> regions = [];

        foreach (string line in File.ReadLines(fileName).Skip(shapeLines))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var region = new Region(line);
            Console.WriteLine(region.ToString());
            regions.Add(region);
        }

        Console.WriteLine($"Loaded {regions.Count} regions.");

        grandTotalP1 = Part1(shapes, regions);
        Console.WriteLine($"Part 1 done.");

        Console.WriteLine("Part 2 done.");

        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The part1 total end was: {grandTotalP1,22} ||");
        Console.WriteLine($"  || The part2 total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }

    public class Region
    {
        public int width { get; set; }
        public int length { get; set; }
        public int[] map { get; set; }

        public Region(string line)
        {
            var matrix = line.Substring(0, line.IndexOf(':')).Trim();
            width = int.Parse(matrix.Substring(0, line.IndexOf('x')));
            length = int.Parse(matrix.Substring(line.IndexOf('x') + 1));

            var items = line[(line.IndexOf(':') + 1)..].Trim();
            map = items.Split(' ').Select(int.Parse).ToArray();
        }

        public override string ToString()
        {
            return $"{width} * {length} [" +
                   string.Join("-", map) +
                   "-]";
        }
    }

    public class Shape
    {
        public int Index { get; set; }
        public bool[,] Grid { get; set; }   // # = true, . = false
        public int Width => Grid.GetLength(1);
        public int Height => Grid.GetLength(0);

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Shape {Index}:");
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                    sb.Append(Grid[r, c] ? '#' : '.');
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public static int CountShapeLines(string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        int i = 0;

        while (i < lines.Length)
        {
            string line = lines[i].Trim();

            if (string.IsNullOrWhiteSpace(line))
            {
                i++;
                continue;
            }

            if (line.EndsWith(":") && char.IsDigit(line[0]))
            {
                i++;
                while (i < lines.Length &&
                       lines[i].Trim() != "" &&
                       !lines[i].Trim().EndsWith(":"))
                    i++;
            }
            else
            {
                break;
            }
        }

        return i;
    }

    public static List<Shape> LoadShapes(string fileName)
    {
        var shapes = new List<Shape>();
        var lines = File.ReadAllLines(fileName);
        int i = 0;

        while (i < lines.Length)
        {
            string line = lines[i].Trim();

            if (line == "")
            {
                i++;
                continue;
            }

            if (line.EndsWith(":") && char.IsDigit(line[0]))
            {
                int idx = int.Parse(line.TrimEnd(':'));
                i++;

                List<string> rows = new();

                while (i < lines.Length &&
                       lines[i].Trim() != "" &&
                       !lines[i].EndsWith(":"))
                {
                    rows.Add(lines[i].Trim());
                    i++;
                }

                shapes.Add(new Shape
                {
                    Index = idx,
                    Grid = ConvertRows(rows)
                });
            }
            else
            {
                break;
            }
        }

        return shapes;
    }

    private static bool[,] ConvertRows(List<string> rows)
    {
        int h = rows.Count;
        int w = rows[0].Length;
        var g = new bool[h, w];

        for (int r = 0; r < h; r++)
            for (int c = 0; c < w; c++)
                g[r, c] = rows[r][c] == '#';

        return g;
    }

    public static long Part1(List<Shape> shapes, List<Region> regions)
    {
        var orientedShapes = shapes
            .Select(s => GenerateOrientations(s.Grid))
            .ToList();

        long validCount = 0;

        for (int regionIdx = 0; regionIdx < regions.Count; regionIdx++)
        {
            var region = regions[regionIdx];
            int H = region.length;
            int W = region.width;
            bool[,] board = new bool[H, W];

            var items = new List<int>();
            for (int i = 0; i < region.map.Length; i++)
                for (int k = 0; k < region.map[i]; k++)
                    items.Add(i);

            items = items
                .OrderByDescending(i => ShapeArea(shapes[i].Grid))
                .ToList();

            int requiredArea = items.Sum(i => ShapeArea(shapes[i].Grid));
            if (requiredArea > W * H)
            {
                Console.WriteLine($"Region {regionIdx} cannot fit (too many blocks).");
                continue;
            }

            if (Backtrack(board, items, 0, orientedShapes))
            {
                validCount++;
                Console.WriteLine($"Region {regionIdx} done: fits!");
            }
            else
            {
                Console.WriteLine($"Region {regionIdx} done: does NOT fit!");
            }
        }

        return validCount;
    }


    static int ShapeArea(bool[,] g)
    {
        int h = g.GetLength(0);
        int w = g.GetLength(1);
        int a = 0;
        for (int r = 0; r < h; r++)
            for (int c = 0; c < w; c++)
                if (g[r, c]) a++;
        return a;
    }

    static List<bool[,]> GenerateOrientations(bool[,] grid)
    {
        var result = new List<bool[,]>();

        void Add(bool[,] g)
        {
            if (!result.Any(e => Same(e, g)))
                result.Add(g);
        }

        bool[,] cur = grid;

        for (int i = 0; i < 4; i++)
        {
            Add(cur);
            Add(FlipH(cur));
            Add(FlipV(cur));
            cur = Rotate(cur);
        }

        return result;
    }

    static bool Same(bool[,] a, bool[,] b)
    {
        if (a.GetLength(0) != b.GetLength(0)) return false;
        if (a.GetLength(1) != b.GetLength(1)) return false;

        for (int r = 0; r < a.GetLength(0); r++)
            for (int c = 0; c < a.GetLength(1); c++)
                if (a[r, c] != b[r, c]) return false;

        return true;
    }

    static bool[,] Rotate(bool[,] g)
    {
        int h = g.GetLength(0);
        int w = g.GetLength(1);
        bool[,] r = new bool[w, h];

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                r[x, h - 1 - y] = g[y, x];

        return r;
    }

    static bool[,] FlipH(bool[,] g)
    {
        int h = g.GetLength(0);
        int w = g.GetLength(1);
        bool[,] r = new bool[h, w];

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                r[y, w - 1 - x] = g[y, x];

        return r;
    }

    static bool[,] FlipV(bool[,] g)
    {
        int h = g.GetLength(0);
        int w = g.GetLength(1);
        bool[,] r = new bool[h, w];

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                r[h - 1 - y, x] = g[y, x];

        return r;
    }

    static bool Backtrack(bool[,] board, List<int> items, int idx, List<List<bool[,]>> oriented)
    {
        if (idx == items.Count)
            return true;

        int H = board.GetLength(0);
        int W = board.GetLength(1);

        int shapeIndex = items[idx];

        foreach (var shape in oriented[shapeIndex])
        {
            int sh = shape.GetLength(0);
            int sw = shape.GetLength(1);

            // ************* FIXED LOOP *************
            for (int top = 0; top <= H - sh; top++)
            {
                for (int left = 0; left <= W - sw; left++)
                {
                    if (!CanPlace(board, shape, top, left))
                        continue;

                    Place(board, shape, top, left, true);

                    if (Backtrack(board, items, idx + 1, oriented))
                        return true;

                    Place(board, shape, top, left, false);
                }
            }
        }

        return false;
    }

    static bool CanPlace(bool[,] board, bool[,] shape, int r0, int c0)
    {
        int sh = shape.GetLength(0);
        int sw = shape.GetLength(1);

        for (int r = 0; r < sh; r++)
            for (int c = 0; c < sw; c++)
                if (shape[r, c] && board[r0 + r, c0 + c])
                    return false;

        return true;
    }

    static void Place(bool[,] board, bool[,] shape, int r0, int c0, bool val)
    {
        int sh = shape.GetLength(0);
        int sw = shape.GetLength(1);

        for (int r = 0; r < sh; r++)
            for (int c = 0; c < sw; c++)
                if (shape[r, c])
                    board[r0 + r, c0 + c] = val;
    }
}
