using System.Diagnostics;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            var startingList = ParseInput(input);
            Int32 numberOfSteps = Int32.Parse(Console.ReadLine());
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var nextList = startingList;
            for(Int32 i = 0; i < numberOfSteps; i++)
            {
                nextList = NextStep(nextList);
            }
            Int64 sum = 0;
            foreach(var v in nextList)
            {
                sum += v.Value;
            }
            sw.Stop();
            Console.WriteLine(sum);
            Console.WriteLine($"Elapsed time was  {sw.ElapsedMilliseconds} milliseconds");
        }
        static Dictionary<Int64, Int64> NextStep(Dictionary<Int64, Int64> inputList)
        {
            var output = new Dictionary<Int64, Int64>();
            foreach(var stone in inputList)
            {
                if (stone.Key == 0)
                {
                    if (output.ContainsKey(1) == false)
                    {
                        output.Add(1, 0);
                    }
                    output[1] += stone.Value;
                    continue;
                }
                if (EvenNumberOfDigits(stone.Key))
                {
                    (Int64, Int64) nextStones = SplitStone(stone.Key);
                    if (output.ContainsKey(nextStones.Item1) == false)
                    {
                        output.Add(nextStones.Item1, 0);
                    }
                    if (output.ContainsKey(nextStones.Item2) == false)
                    {
                        output.Add(nextStones.Item2, 0);
                    }
                    output[nextStones.Item1] += stone.Value;
                    output[nextStones.Item2] += stone.Value;
                    continue;
                }
                if (output.ContainsKey(2024 * stone.Key) == false)
                {
                    output.Add(2024 * stone.Key, 0);
                }
                output[2024 * stone.Key] += stone.Value;
            }
            return output;
        }
        static (Int64, Int64) SplitStone(Int64 stone)
        {
            string input = stone.ToString();
            Int32 length = input.Length/2;
            StringBuilder left = new StringBuilder();
            StringBuilder right = new StringBuilder();
            for(Int32 i = 0; i < length; i++)
            {
                left.Append(input[i]);
                right.Append(input[length+i]);
            }
            Int64 leftOutput = Int64.Parse(left.ToString());
            Int64 rightOutput = Int64.Parse(right.ToString());
            return (leftOutput, rightOutput);
        }
        static bool EvenNumberOfDigits(Int64 input)
        {
            Int32 numberOfDigits = (Int32)Math.Log10(input);
            numberOfDigits++;
            if (numberOfDigits % 2 == 0)
            {
                return true;
            }
            return false;
        }
        static Dictionary<Int64,Int64> ParseInput(string input)
        {
            var output = new Dictionary<Int64, Int64>();
            string[] numbers = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach(var n in numbers)
            {
                Int64 nextInput = Int64.Parse(n);
                if (output.ContainsKey(nextInput) == false)
                {
                    output.Add(nextInput, 0);
                }
                output[nextInput]++;
            }
            return output;
        }
    }
}
