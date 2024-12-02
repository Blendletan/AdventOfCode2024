using System.Diagnostics.CodeAnalysis;

namespace Day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TextReader reader = new StreamReader("inputTest.txt");
            TextReader reader = new StreamReader("inputs.txt");
            List<List<Int32>> reportList = GetInputs(reader);
            Int32 sumPartOne = SolvePartOne(reportList);
            Console.WriteLine(sumPartOne);
            Int32 sumPartTwo = SolvePartTwo(reportList);
            Console.WriteLine(sumPartTwo);
        }
        static Int32 SolvePartTwo(List<List<Int32>> reportList)
        {
            Int32 sum = 0;
            foreach (var report in reportList)
            {
                if (IsSafe(report))
                {
                    sum++;
                    continue;
                }
                Int32 length = report.Count;
                for(Int32 i = 0; i < length; i++)
                {
                    List<Int32> alternateReport = new List<Int32>(report);
                    alternateReport.RemoveAt(i);
                    if (IsSafe(alternateReport))
                    {
                        sum++;
                        break;
                    }
                }
            }
            return sum;
        }
        static Int32 SolvePartOne(List<List<Int32>> reportList)
        {
            Int32 sum = 0;
            foreach (var report in reportList)
            {
                if (IsSafe(report))
                {
                    sum++;
                }
            }
            return sum;
        }
        static List<List<Int32>> GetInputs(TextReader reader)
        {
            string? nextLine = reader.ReadLine();
            List<List<Int32>> reportList = new List<List<Int32>>();
            while (true)
            {
                if (nextLine == null)
                {
                    break;
                }
                string[] reportStrings = nextLine.Split(" ",StringSplitOptions.RemoveEmptyEntries);
                List<Int32> nextReport =new List<Int32>();
                foreach(var inputValue in reportStrings)
                {
                    nextReport.Add(Int32.Parse(inputValue));
                }
                reportList.Add(nextReport);
                nextLine = reader.ReadLine();
            }
            return reportList;
        }
        static bool IsSafe(List<Int32> report)
        {
            Int32 length = report.Count;
            if (length == 1)
            {
                return true;
            }
            if (report[0] > report[1])
            {
                report.Reverse();
            }
            for(Int32 i = 0; i < length-1; i++)
            {
                Int32 currentValue = report[i];
                Int32 nextValue = report[i + 1];
                if (currentValue == nextValue)
                {
                    return false;
                }
                if (currentValue > nextValue)
                {
                    return false;
                }
                if (nextValue-currentValue > 3)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
