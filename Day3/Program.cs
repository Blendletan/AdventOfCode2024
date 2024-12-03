using System.Text;
using System.Text.RegularExpressions;

namespace Day3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextReader reader = new StreamReader("inputs.txt");
            string inputs = GetInputs(reader);
            List<string> matches = MatchPattern(inputs);
            Int32 sum = 0;
            foreach (var m in matches)
            {
                var numbers = StripText(m);
                sum += numbers.Item2 * numbers.Item1;
            }
            Console.WriteLine(sum);
            PartTwo pt = new PartTwo(inputs);
            List<string> PartTwoMatches = new List<string>();
            while (true)
            {
                string? nextInput = pt.Next();
                if (nextInput != null)
                {
                    PartTwoMatches.Add(nextInput);
                    Console.WriteLine(nextInput);
                }
                if (pt.IsEmpty())
                {
                    break;
                }
            }
            sum = 0;
            foreach (var m in PartTwoMatches)
            {
                var numbers = StripText(m);
                sum += numbers.Item2 * numbers.Item1;
            }
            Console.WriteLine(sum);
        }
        static (Int32, Int32) StripText(string input)
        {
            string[] splitInputs = input.Split(",");
            Int32 x = 0;
            Int32 y = 0;
            StringBuilder xBuilder = new StringBuilder();
            foreach (var c in splitInputs[0])
            {
                if (Char.IsDigit(c))
                {
                    xBuilder.Append(c);
                }
            }
            StringBuilder yBuilder = new StringBuilder();
            foreach (var c in splitInputs[1])
            {
                if (Char.IsDigit(c))
                {
                    yBuilder.Append(c);
                }
            }
            x = Int32.Parse(xBuilder.ToString());
            y = Int32.Parse(yBuilder.ToString());
            return (x, y);
        }
        static List<string> MatchPattern(string input)
        {
            List<string> output = new List<string>();
            Regex regEx = new Regex(@"mul\(\d{1,3},\d{1,3}\)", RegexOptions.Compiled);
            foreach (var itemMatch in regEx.Matches(input))
            {
                output.Add(itemMatch.ToString());
            }
            return output;
        }
        static string GetInputs(TextReader reader)
        {
            return reader.ReadToEnd();
        }
    }
    internal class PartTwo
    {
        bool multiplicationEnabled;
        const string multiplicationString = @"mul\(\d{1,3},\d{1,3}\)";
        const string doString = @"do\(\)";
        const string dontString = @"don't\(\)";
        Regex matchDo;
        Regex matchDont;
        Regex matchMultiply;
        StringBuilder remainingString;
        public PartTwo(string inputs)
        {
            remainingString = new StringBuilder(inputs);
            multiplicationEnabled = true;
            matchDo = new Regex(doString, RegexOptions.Compiled);
            matchDont = new Regex(dontString, RegexOptions.Compiled);
            matchMultiply = new Regex(multiplicationString, RegexOptions.Compiled);
        }
        public bool IsEmpty()
        {
            if (remainingString.Length == 0)
            {
                return true;
            }
            return false;
        }
        public string? Next()
        {
            Match nextMultiplication = matchMultiply.Match(remainingString.ToString());
            if (!nextMultiplication.Success)
            {
                remainingString.Remove(0, remainingString.Length);
                return null;
            }
            Match nextDont = matchDont.Match(remainingString.ToString());
            if (multiplicationEnabled)
            {
                if (nextDont.Success)
                {
                    if (nextDont.Index < nextMultiplication.Index)
                    {
                        multiplicationEnabled = false;
                        remainingString.Remove(0, nextDont.Index + nextDont.Length);
                        return null;
                    }
                }
                string output = nextMultiplication.Value;
                remainingString.Remove(0, nextMultiplication.Index + nextMultiplication.Length);
                return output;
            }
            if (!multiplicationEnabled)
            {
                Match nextDo = matchDo.Match(remainingString.ToString());
                if (!nextDo.Success)
                {
                    remainingString.Remove(0, remainingString.Length);
                    return null;
                }
                multiplicationEnabled = true;
                remainingString.Remove(0, nextDo.Index + nextDo.Length);
                return null;
            }
            return null;
        }
    }
}
