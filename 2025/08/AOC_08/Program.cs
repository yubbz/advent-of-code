namespace AOC8;

// Working Answers
// P1: 67488
// P2: 


class Program
{
    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";

        Console.WriteLine($"Advent Of Code 08 - Full Solution");

        List<Xyz> points = File.ReadLines(fileName)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => new Xyz(l)).ToList();

        int n = points.Count;
        Console.WriteLine($"Loaded {n} points.");

        // Build list of all pair distances
        List<Edge> edges = new();

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                long dist = points[i].DistanceSq(points[j]);
                edges.Add(new Edge(i, j, dist));
            }
        }

        Console.WriteLine($"Computed {edges.Count} edges. Sorting...");

        edges.Sort((a, b) => a.Weight.CompareTo(b.Weight));

        // Union-find
        UnionFind uf = new(n);

        // Determine number of connections for Part 1
        int connectionsToMake = fileName.Contains("Test", StringComparison.OrdinalIgnoreCase) ? 10 : 1000;

        int lastA = -1, lastB = -1;

        for (int k = 0; k < Math.Min(connectionsToMake, edges.Count); k++)
        {
            if (uf.Union(edges[k].A, edges[k].B))
            {
                lastA = edges[k].A;
                lastB = edges[k].B;
            }
        }

        // Part 1 sizes and product
        var sizes = uf.ComponentSizes();
        sizes.Sort(); sizes.Reverse();
        long answer = sizes.Take(3).Aggregate(1L, (a, b) => a * b);
        Console.WriteLine($"Largest circuits: {string.Join(", ", sizes.Take(3))}");
        Console.WriteLine($"Final Answer (product): {answer}");

        // PART 2: continue until all points are connected
        int totalComponents = sizes.Count;
        int index = Math.Min(connectionsToMake, edges.Count);

        while (totalComponents > 1 && index < edges.Count)
        {
            if (uf.Union(edges[index].A, edges[index].B))
            {
                totalComponents--;
                lastA = edges[index].A;
                lastB = edges[index].B;
            }
            index++;
        }

        if (lastA == -1 || lastB == -1)
        {
            Console.WriteLine("ERROR: No last merge found.");
        }
        else
        {
            long part2 = points[lastA].x * points[lastB].x;
            Console.WriteLine($"Part 2: last join X-product = {part2}");
        }
    }

    class Xyz
    {
        public long x, y, z;

        public Xyz(string line)
        {
            var p = line.Split(',');
            x = long.Parse(p[0]);
            y = long.Parse(p[1]);
            z = long.Parse(p[2]);
        }

        public long DistanceSq(Xyz b)
        {
            long dx = x - b.x;
            long dy = y - b.y;
            long dz = z - b.z;
            return dx * dx + dy * dy + dz * dz;
        }
    }

    class Edge
    {
        public int A, B;
        public long Weight;
        public Edge(int a, int b, long w) { A = a; B = b; Weight = w; }
    }

    class UnionFind
    {
        private int[] parent;
        private int[] size;

        public UnionFind(int n)
        {
            parent = Enumerable.Range(0, n).ToArray();
            size = Enumerable.Repeat(1, n).ToArray();
        }

        public int Find(int x)
        {
            if (parent[x] != x)
                parent[x] = Find(parent[x]);
            return parent[x];
        }

        public bool Union(int a, int b)
        {
            int ra = Find(a);
            int rb = Find(b);
            if (ra == rb) return false;

            if (size[ra] < size[rb])
            {
                (ra, rb) = (rb, ra);
            }

            parent[rb] = ra;
            size[ra] += size[rb];
            return true;
        }

        public List<int> ComponentSizes()
        {
            for (int i = 0; i < parent.Length; i++)
                parent[i] = Find(i);

            var groups = new Dictionary<int, int>();
            foreach (var p in parent)
            {
                if (!groups.ContainsKey(p)) groups[p] = 0;
                groups[p]++;
            }

            return groups.Values.ToList();
        }
    }
}
