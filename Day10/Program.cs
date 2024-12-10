using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "inputs.txt";
            Int32[,] puzzleInput;
            using (StreamReader sr = new StreamReader(filePath))
            {
                puzzleInput = ParseInput(sr);
            }
            Int32 partOne = Solve(puzzleInput,true);
            Console.WriteLine(partOne);
            Int32 partTwo = Solve(puzzleInput, false);
            Console.WriteLine(partTwo);
        }
        static Int32 Solve(Int32[,] puzzleInput,bool partOne)
        {
            Int32 length = puzzleInput.GetLength(0);
            Int32 sum = 0;
            for (Int32 i = 0; i < length; i++)
            {
                for (Int32 j = 0; j < length; j++)
                {
                    if (puzzleInput[i, j] == 0)
                    {
                        List<(Int32,Int32)> finalLocations = new List<(Int32,Int32)>();
                        GetTrailHeadScore(i, j, finalLocations, puzzleInput,partOne);
                        sum += finalLocations.Count;
                    }
                }
            }
            return sum;
        }
        static void GetTrailHeadScore(Int32 y,Int32 x,List<(Int32,Int32)> finalLocations, Int32[,] puzzle,bool partOne)
        {
            Int32 length = puzzle.GetLength(0);
            Int32 currentValue = puzzle[y,x];
            if (currentValue==9)
            {
                if (partOne)
                {
                    if (finalLocations.Contains((y, x)) == false)
                    {
                        finalLocations.Add((y, x));
                    }
                    return;
                }
                finalLocations.Add((y, x));
                return;
            }
            if (x > 0)
            {
                Int32 nextValue = puzzle[y,x-1];
                if (nextValue == currentValue + 1)
                {
                    GetTrailHeadScore(y,x - 1, finalLocations, puzzle,partOne);
                }
            }
            if (y > 0)
            {
                Int32 nextValue = puzzle[y-1,x];
                if (nextValue == currentValue + 1)
                {
                    GetTrailHeadScore(y - 1, x, finalLocations, puzzle,partOne);
                }
            }
            if (x < length - 1)
            {
                Int32 nextValue = puzzle[y, x + 1];
                if (nextValue == currentValue + 1)
                {
                    GetTrailHeadScore(y, x + 1, finalLocations, puzzle, partOne);
                }
            }
            if (y < length - 1)
            {
                Int32 nextValue = puzzle[y+1,x];
                if (nextValue == currentValue + 1)
                {
                    GetTrailHeadScore(y+1, x, finalLocations, puzzle, partOne);
                }
            }

        }
        static Int32[,] ParseInput(StreamReader reader)
        {
            string nextLine = reader.ReadLine();
            Int32 length = nextLine.Length;
            Int32[,] output = new Int32[length, length];
            for(Int32 i = 0; i < length; i++)
            {
                output[0, i] = Int32.Parse(Char.ToString(nextLine[i]));
            }
            for (Int32 i = 1; i < length; i++)
            {
                nextLine = reader.ReadLine();
                for (Int32 j = 0; j < length; j++)
                {
                    output[i,j]= Int32.Parse(Char.ToString(nextLine[j]));
                }
            }
            return output;
        }
    }
}
