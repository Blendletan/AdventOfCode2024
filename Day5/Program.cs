using System.Collections.Generic;
namespace Day5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filepath = "inputsB.txt";
            StreamReader reader = new StreamReader(filepath);
            Dictionary<Int32, List<Int32>> edges = ProcessOrderingRuleInputs(reader);
            List<List<Int32>> inputUpdates = ProcessInputUpdates(reader);
            Int32 sum = 0;
            List<List<Int32>> invalidUpdates = new List<List<Int32>>();
            foreach(var update in inputUpdates)
            {
                if (UpdateIsValid(edges, update))
                {
                    sum += MiddleValue(update);
                }
                else
                {
                    invalidUpdates.Add(update);
                }
            }
            Console.WriteLine(sum);
            sum = 0;
            foreach (var update in invalidUpdates)
            {
                ValidSort(update, edges);
                sum += MiddleValue(update);
            }
            Console.WriteLine(sum);
        }
        static void ValidSort(List<Int32> update,Dictionary<Int32,List<Int32>> edges)
        {
            Int32 length = update.Count;
            while (true)
            {
                if (UpdateIsValid(edges, update))
                {
                    break;
                }
                for (Int32 i = 0; i < length; i++)
                {
                    Int32 nextInt = update[i];
                    if (!edges.ContainsKey(nextInt))
                    {
                        continue;
                    }
                    foreach (var laterInt in edges[nextInt])
                    {
                        if (!update.Contains(laterInt))
                        {
                            continue;
                        }
                        Int32 indexOfNext = update.IndexOf(nextInt);
                        Int32 indexOfLater = update.IndexOf(laterInt);
                        if (indexOfNext > indexOfLater)
                        {
                            Int32 temp = nextInt;
                            update[indexOfNext] = laterInt;
                            update[indexOfLater] = temp;
                        }
                    }
                }
            }
        }
        static Int32 MiddleValue(List<Int32> list)
        {
            Int32 middleIndex = list.Count / 2;
            return list[middleIndex];
        }
        static bool UpdateIsValid (Dictionary<Int32,List<Int32>> edges, List<Int32> update)
        {
            foreach(var nextInt in update)
            {
                if (!edges.ContainsKey(nextInt))
                {
                    continue;
                }
                foreach(var laterInt in edges[nextInt])
                {
                    if (!update.Contains(laterInt))
                    {
                        continue;
                    }
                    if (update.IndexOf(nextInt)> update.IndexOf(laterInt))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static List<List<Int32>> ProcessInputUpdates(StreamReader reader)
        {
            List<List<Int32>> outputs = new List<List<Int32>>();
            string ? nextLine = reader.ReadLine();
            while (true)
            {
                if (nextLine == null)
                {
                    break;
                }
                List<Int32> nextOutput = new List<Int32>();
                string[] inputs = nextLine.Split(",");
                foreach(var nextNumber in inputs)
                {
                    nextOutput.Add(Int32.Parse(nextNumber));
                }
                outputs.Add(nextOutput);
                nextLine = reader.ReadLine();
            }
            return outputs;
        }
        static Dictionary<Int32,List<Int32>> ProcessOrderingRuleInputs(StreamReader reader)
        {
            Dictionary<Int32, List<Int32>> output = new Dictionary<Int32, List<Int32>>();
            string? nextLine = reader.ReadLine();
            while (true)
            {
                if (nextLine == null || nextLine=="")
                {
                    break;
                }
                string[] inputs = nextLine.Split("|");
                Int32 key = Int32.Parse(inputs[0]);
                Int32 value = Int32.Parse(inputs[1]);
                if (!output.ContainsKey(key))
                {
                    output.Add(key, new List<Int32>());
                }
                output[key].Add(value);
                nextLine = reader.ReadLine();
            }
            return output;
        }
    }
}