namespace AOC7;

// Working Answers
// P1: 1615
// P2: 43560947406326

class Program
{
    static Dictionary<(int row, int col), long> memo = new();
    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";

        long grandTotalP1 = 0;
        long grandTotalP2 = 0;

        Console.WriteLine($"Advent Of Code 07");

        var beams = new List<string>();
        int lineCount = 0;

        foreach (String line in File.ReadLines(fileName))
        {
            string lineChars = "";
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case 'S':
                        lineChars += "S";
                        break;

                    case '.':
                        if (lineCount != 0)
                        {
                            if (beams[lineCount - 1][i] == 'S')
                                lineChars += "|";
                            else if (beams[lineCount - 1][i] == '|')
                                lineChars += '|';
                            else
                                lineChars += '.';
                        }
                        else
                        {
                            lineChars += ".";
                        }
                        break;

                    case '^':
                        if (beams[lineCount - 1][i] == '|')
                        {
                            lineChars += "X";
                        }
                        else
                        {
                            lineChars += "^";
                        }
                        break;

                    default:
                        throw new Exception("err");
                }

            }
            int count = lineChars.Count(c => c == 'X');

            Console.Write($"cnt: {count}");

            grandTotalP1 += count;

            //lineChars += $"cnt: {count}";

            lineChars = lineChars.Replace(".X.", "|^|");
            lineChars = lineChars.Replace("|X.", "|^|");
            lineChars = lineChars.Replace(".X|", "|^|");
            lineChars = lineChars.Replace("|X|", "|^|");
            lineChars = lineChars.Replace("X", "^");


            beams.Add(lineChars);
            lineCount++;
        }

        Console.WriteLine(beams.Count);



        for (int i = 0; i < beams.Count; i++)
        {
            Console.WriteLine($"{beams[i]}");

        }


        // PART 2
        string result = CalculateTimelines(beams);
        Console.WriteLine($"Total timelines: {result}");

        // OUTPUT
        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The part1 total end was: {grandTotalP1,22} ||");
        Console.WriteLine($"  || The part2 total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }

    static long CountPaths(List<string> grid, int row, int col)
    {
        if (row >= grid.Count)
            return 1L; // reached bottom → one timeline

        if (memo.TryGetValue((row, col), out long cached))
            return cached;

        char cell = grid[row][col];
        long result = 0;

        if (cell == '|')
        {
            result = CountPaths(grid, row + 1, col);
        }
        else if (cell == '^')
        {
            long left = (col > 0) ? CountPaths(grid, row + 1, col - 1) : 0;
            long right = (col < grid[row].Length - 1) ? CountPaths(grid, row + 1, col + 1) : 0;
            result = left + right;
        }
        else
        {
            result = 0;
        }

        memo[(row, col)] = result;
        return result;
    }

    public static string CalculateTimelines(List<string> grid)
    {
        memo.Clear();

        int startRow = -1, startCol = -1;
        for (int r = 0; r < grid.Count; r++)
        {
            int c = grid[r].IndexOf('S');
            if (c != -1)
            {
                startRow = r;
                startCol = c;
                break;
            }
        }

        if (startRow == -1)
            return "No start found";

        long total = CountPaths(grid, startRow + 1, startCol);
        return total.ToString();
    }
}