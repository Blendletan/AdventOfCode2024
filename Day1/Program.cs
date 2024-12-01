using System.Numerics;

namespace Day1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "inputs.txt";
            TextReader reader = new StreamReader(filename);
            (List<Int64>, List<Int64>) inputLists = ParseInputs(reader);
            reader.Close();
            Console.WriteLine(SolvePartOne(inputLists));
            Console.WriteLine(SolvePartTwo(inputLists));
        }
        static Int64 SolvePartTwo((List<Int64>, List<Int64>) inputLists)
        {
            List<Int64> leftList = inputLists.Item1;
            List<Int64> rightList = inputLists.Item2;
            Int64 sum = 0;
            foreach (var leftNumber in leftList)
            {
                Int32 numberOfAppearancesOnRight = 0;
                foreach(var rightNumber in rightList)
                {
                    if (rightNumber == leftNumber)
                    {
                        numberOfAppearancesOnRight++;
                    }
                }
                sum += leftNumber * numberOfAppearancesOnRight;
            }
            return sum;
        }
        static Int64 SolvePartOne((List<Int64>, List<Int64>) inputLists)
        {
            Int64 sum = 0;
            Int32 length = inputLists.Item1.Count;
            for (Int32 i = 0; i < length; i++)
            {
                Int64 nextSummand = Math.Abs(inputLists.Item1[i] - inputLists.Item2[i]);
                sum += nextSummand;
            }
            return sum;
        }
        static (List<Int64>,List<Int64>) ParseInputs(TextReader inputReader)
        {
            List<Int64> firstOutput = new List<Int64>();
            List<Int64> secondOutput = new List<Int64>();
            string? nextLine = inputReader.ReadLine();
            while (true)
            {
                if (nextLine == null)
                {
                    break;
                }
                string[] inputs = nextLine.Split(" ",StringSplitOptions.RemoveEmptyEntries);
                firstOutput.Add(Int64.Parse(inputs[0]));
                secondOutput.Add(Int64.Parse(inputs[1]));
                nextLine = inputReader.ReadLine();
            }
            firstOutput.Sort();
            secondOutput.Sort();
            return (firstOutput, secondOutput);
        }
    }
}
