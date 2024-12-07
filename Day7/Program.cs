namespace Day7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "inputs.txt";
            StreamReader reader = new StreamReader(filePath);
            List<Equation> inputs = ParseInputs(reader);
            reader.Close();
            Int128 sum = 0;
            foreach(var v in inputs)
            {
                if (Equation.TryToSolve(v,false))
                {
                    sum += v.Result;
                }
            }
            Console.WriteLine(sum);
            sum = 0;
            foreach (var v in inputs)
            {
                if (Equation.TryToSolve(v,true))
                {
                    sum += v.Result;
                }
            }
            Console.WriteLine(sum);
            Console.WriteLine("Hello, World!");
        }
        static List<Equation> ParseInputs(StreamReader reader)
        {
            List<Equation> output = new List<Equation>();
            string? nextLine = reader.ReadLine();
            while (true)
            {
                if (nextLine == null)
                {
                    break;
                }
                output.Add(new Equation(nextLine));
                nextLine = reader.ReadLine();
            }
            return output;
        }
    }
    internal class Equation
    {
        public readonly Int128 Result;
        public readonly Int128[] Numbers;
        public readonly Int32 Length;
        public Equation(string input)
        {
            string[] sides = input.Split(":");
            Result = Int128.Parse(sides[0]);
            string[] rightHandSide = sides[1].Split(" ",StringSplitOptions.RemoveEmptyEntries);
            Length = rightHandSide.Length;
            Numbers = new Int128[Length];
            for(Int32 i = 0; i < Length; i++)
            {
                Numbers[i] = Int128.Parse(rightHandSide[i]);
            }
        }
        public Equation(Int128 result, List<Int128> numbers)
        {
            Result = result;
            Numbers = numbers.ToArray();
            Length = Numbers.Length;
        }
        public static bool TryToSolve(Equation inputEquation,bool partTwo)
        {
            if (inputEquation.Length == 1)
            {
                if (inputEquation.Numbers[0] == inputEquation.Result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            Int128 multFirstDigit = inputEquation.Numbers[0] * inputEquation.Numbers[1];
            List<Int128> multList = inputEquation.Numbers.ToList();
            multList.RemoveAt(0);
            multList.RemoveAt(0);
            multList.Insert(0, multFirstDigit);
            Equation tryMult = new Equation(inputEquation.Result, multList);
            Int128 addFirstDigit = inputEquation.Numbers[0] + inputEquation.Numbers[1];
            List<Int128> addList = inputEquation.Numbers.ToList();
            addList.RemoveAt(0);
            addList.RemoveAt(0);
            addList.Insert(0, addFirstDigit);
            Equation tryAdd = new Equation(inputEquation.Result, addList);
            if (TryToSolve(tryAdd,partTwo) || TryToSolve(tryMult,partTwo))
            {
                return true;
            }
            if (partTwo)
            {
                Int128 concatFirstDigit = Int128.Parse(inputEquation.Numbers[0].ToString() + inputEquation.Numbers[1].ToString());
                List<Int128> concatList = inputEquation.Numbers.ToList();
                concatList.RemoveAt(0);
                concatList.RemoveAt(0);
                concatList.Insert(0, concatFirstDigit);
                Equation tryConcat = new Equation(inputEquation.Result, concatList);
                if (TryToSolve(tryConcat, partTwo))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
