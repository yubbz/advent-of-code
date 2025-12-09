namespace AOC4;

// Working Answers
// P1: 1564
// P2: 9401

class Program
{
    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";

        List<List<char>> grid = new List<List<char>>();

        Console.WriteLine($"Advent Of Code 04");

        foreach (String line in File.ReadLines(fileName))
        {
            grid.Add(line.ToList());
        }

        PrintGrid(grid, "ingestion");

        var resultPt1 = Part1(grid);


        int lastRemoval = 0;
        int grandTotalP2 = 0;

        List<List<char>> newGrid = grid;

        do
        {

            var resultPt2 = Part1(newGrid);
            lastRemoval = resultPt2.numberRemoved;
            grandTotalP2 += resultPt2.numberRemoved;
            newGrid = resultPt2.newGrid;
            //PrintGrid(newGrid, "newGrid");
        } while (lastRemoval > 0);

        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The part1 total end was: {resultPt1.numberRemoved,22} ||");
        Console.WriteLine($"  || The part2 total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }

    private static (int numberRemoved, List<List<char>> newGrid) Part1(List<List<char>> grid)
    {
        int Xs = 0;
        List<List<char>> gridResult = new List<List<char>>();


        for (int i = 0; i < grid.Count; i++)
        {
            gridResult.Add(new List<char>());
            for (int j = 0; j < grid[i].Count; j++)
            {
                if (grid[i][j] == '.')
                {
                    gridResult[i].Add('.');
                }
                else
                {

                    int paperBlocks = 0;
                    //above
                    if (i > 0)
                    {
                        if (j > 0)
                            if (grid[i - 1][j - 1] == '@')
                                paperBlocks++;

                        if (grid[i - 1][j] == '@')
                            paperBlocks++;

                        if (j < grid[i - 1].Count - 1)
                            if (grid[i - 1][j + 1] == '@')
                                paperBlocks++;
                    }

                    //same
                    if (j > 0)
                        if (grid[i][j - 1] == '@')
                            paperBlocks++;

                    if (j < grid[i].Count - 1)
                        if (grid[i][j + 1] == '@')
                            paperBlocks++;

                    //below
                    if (i < grid.Count - 1)
                    {
                        if (grid[i + 1][j] == '@')
                            paperBlocks++;

                        if (j > 0)
                            if (grid[i + 1][j - 1] == '@')
                                paperBlocks++;

                        if (j < grid[i + 1].Count - 1)
                            if (grid[i + 1][j + 1] == '@')
                                paperBlocks++;
                    }

                    //total
                    if (paperBlocks < 4)
                    {
                        gridResult[i].Add('.');
                        Xs++;
                    }
                    else
                    {
                        gridResult[i].Add(grid[i][j]);
                    }
                }
            }
        }

        return (Xs, gridResult);
    }

    static void PrintGrid(List<List<char>> grid, string title)
    {
        Console.WriteLine($"\n{title}: Printgrid:");
        foreach (var row in grid)
            Console.WriteLine(string.Join("", row));
    }


    public static int IngestLine1(string line)
    {
        List<int> parts = line.Select(c => int.Parse(c.ToString())).ToList();
        int max = 0;

        for (int i = 0; i < line.Length - 1; i++)
        {
            for (int j = (i + 1); j < line.Length; j++)
            {
                int sum = int.Parse($"{parts[i]}{parts[j]}");
                if (sum > max)
                {
                    max = sum;
                }
            }
        }
        return max;
    }


    public static string IngestLine2(string line, int size)
    {
        return "";
    }
}