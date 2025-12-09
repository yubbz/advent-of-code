using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace AOC6;

// Working Answers
// P1: 5784380717354
// P2: 7996218225744

class Program
{
    static void Main(string[] args)
    {
        string fileName = "Input1.txt";
        //fileName = "InputTest.txt";

        long grandTotalP1 = 0;
        long grandTotalP2 = 0;

        Console.WriteLine($"Advent Of Code 06");

        var list = new List<List<string>>();
        var ops = new List<string>();


        var lines = new List<string>();

        foreach (String line in File.ReadLines(fileName))
        {

            //p1
            var parts = GetListItems(line);
            if (int.TryParse(parts[0], out int partNum))
            {
                list.AddRange(parts);
            }
            else
            {
                ops.AddRange(parts);
            }


            // p2:
            lines.Add(line);
        }



        for (int i = 0; i < ops.Count; i++)
        {
            List<long> indv = [];

            for (int j = 0; j < list.Count; j++)
            {

                Console.Write($"{list[j][i]} ({j}/{i}) - ");
                indv.Add(long.Parse($"{list[j][i]}"));
            }

            var res = doMath(indv, ops[i]);
            grandTotalP1 += res;

        }




        //p2
        var processlistp2 = new List<List<string>>();
        var opsp2 = new List<string>();


        int lineCnt = lines.Count;
        int lineLen = lines[0].Length;

        var process = new List<List<string>>();

        Console.WriteLine(lineCnt);

        foreach (var l in lines)
        {
            Console.WriteLine($"linecnt: {l.Length}");

        }


        for (int i = 0; i < lineLen; i++)
        {
            var lastitem = lines[lineCnt - 1][i];
            if (lastitem != ' ')
            {
                opsp2.Add(lastitem.ToString());
                processlistp2.Add(new List<string>());
                Console.WriteLine("OPS = " + lastitem);
            }

            var stringL = "";
            for (int j = 0; j < (lineCnt - 1); j++)
            {
                //Console.Write(lines[j][i]);
                stringL += lines[j][i];
            }


            if (!stringL.IsWhiteSpace())
            {
                processlistp2[processlistp2.Count - 1].Add(stringL.Trim());

                Console.WriteLine($"{stringL.Trim()}\r\n-------");
            }

        }



        Console.WriteLine("op2lwn = " + opsp2.Count);
        Console.WriteLine("linelwn = " + processlistp2.Count);


        for (int i = 0; i < ops.Count; i++)
        {
            Console.WriteLine("op2 = " + opsp2[i]);

            List<long> indv = [];


            var p2line = processlistp2[i];

            for (int j = p2line.Count - 1; j >= 0; j--)
            {

                Console.Write($"{p2line[j]} ({j}/{i}) - ");
                indv.Add(long.Parse($"{p2line[j]}"));
            }

            var res = doMath(indv, opsp2[i]);
            grandTotalP2 += res;

        }

        Console.WriteLine();
        Console.WriteLine($"  -----------------------------------------------------");
        Console.WriteLine($"  || The part1 total end was: {grandTotalP1,22} ||");
        Console.WriteLine($"  || The part2 total end was: {grandTotalP2,22} ||");
        Console.WriteLine($"  -----------------------------------------------------");
    }

    private static long doMath(List<long> indv, string v)
    {
        long linetotal = indv[0];

        switch (v)
        {
            case "*":
                for (int i = 1; i < indv.Count; i++)
                {
                    linetotal = linetotal * indv[i];
                }
                ;
                break;
            case "+":
                for (int i = 1; i < indv.Count; i++)
                {
                    linetotal += indv[i];
                }
                break;
            default:
                Console.WriteLine("no:");
                break;
        }




        Console.WriteLine($"tot: {linetotal}");
        return linetotal;

    }




    public static List<string> GetListItems(string input)
    {


        List<string> parts = input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        return parts;
    }





}