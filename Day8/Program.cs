using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Day8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "inputs.txt";
            string testFilePath = "inputTest.txt";
            StreamReader reader = new StreamReader(filePath);
            char[,] input = GetMap(reader);
            reader.Close();
            AntennaMap map = new AntennaMap(input);
            map.FillInAntiNodes(false);
            char[,]? outputMap = input.Clone() as char[,];
            if (outputMap == null)
            {
                return;
            }
            foreach (var v in map.antiNodeLocations)
            {
                Int32 i = v.y;
                Int32 j = v.x;
                outputMap[i, j] = '#';
            }
            for(Int32 i = 0; i < outputMap.GetLength(0);i++)
            {
                for(Int32 j = 0; j < outputMap.GetLength(1); j++)
                {
                    Console.Write(outputMap[i,j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine(map.GetTotalAntiNodes());
            map = new AntennaMap(input);
            map.FillInAntiNodes(true);
            outputMap = input.Clone() as char[,];
            if (outputMap == null)
            {
                return;
            }
            foreach (var v in map.antiNodeLocations)
            {
                Int32 i = v.y;
                Int32 j = v.x;
                outputMap[i, j] = '#';
            }
            for (Int32 i = 0; i < outputMap.GetLength(0); i++)
            {
                for (Int32 j = 0; j < outputMap.GetLength(1); j++)
                {
                    Console.Write(outputMap[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine(map.GetTotalAntiNodes());
            Console.WriteLine("Hello, World!");
        }
        static char[,] GetMap(StreamReader reader)
        {
            List<string> mapInput = new List<string>();
            string? nextLine = reader.ReadLine();
            Int32 width = nextLine.Length;
            Int32 height = 0;
            while(nextLine != null)
            {
                mapInput.Add(nextLine);
                height++;
                nextLine = reader.ReadLine();
            }
            char[,] output = new char[height,width];
            for(Int32 i = 0; i < height; i++)
            {
                for(Int32 j = 0; j < width; j++)
                {
                    output[i, j] = mapInput[i][j];
                }
            }
            return output;
        }
    }
    public struct Location : IEqualityComparer<Location>
    {
        public readonly Int32 y;
        public readonly Int32 x;
        public Location(Int32 y,Int32 x)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Location x, Location y)
        {
            if (x.x == y.x && y.x == y.y)
            {
                return true;
            }
            return false;
        }

        public Int32 GetHashCode([DisallowNull] Location obj)
        {
            return obj.x * (Int32)Math.Pow(2,16) + obj.y;
        }
    }
    public class AntennaMap
    {
        Dictionary<char, List<Location>> antennaLocations;
        public List<Location> antiNodeLocations;
        readonly Int32 height;
        readonly Int32 width;
        public AntennaMap(char[,] inputMap)
        {
            antennaLocations = new Dictionary<char, List<Location>>();
            antiNodeLocations = new List<Location>();
            height = inputMap.GetLength(0);
            width = inputMap.GetLength(1);
            for(Int32 i = 0; i < height; i++)
            {
                for(Int32 j = 0; j < width; j++)
                {
                    char nextChar = inputMap[i, j];
                    if (char.IsLetterOrDigit(nextChar))
                    {
                        if (!antennaLocations.ContainsKey(nextChar))
                        {
                            antennaLocations.Add(nextChar, new List<Location>());
                        }
                        antennaLocations[nextChar].Add(new Location(i, j));
                    }
                }
            }
        }
        public Int32 GetTotalAntiNodes()
        {
            return antiNodeLocations.Count;
        }
        public void FillInAntiNodes(bool partTwo)
        {
            foreach(var v in antennaLocations)
            {
                FillInAntiNodeLocation(v.Key,partTwo);
            }
        }
        private bool TryToAddLocation(Location nextLocation)
        {
            if (nextLocation.y >= height || nextLocation.y < 0 )
            {
                return false;
            }
            if (nextLocation.x >= width || nextLocation.x < 0)
            {
                return false;
            }
            foreach(var foundLocation in antiNodeLocations)
            {
                if (foundLocation.Equals(nextLocation))
                {
                    return false;
                }
            }
            antiNodeLocations.Add(nextLocation);
            return true;
        }
        private Location GetNextInLine(Location a, Location b)
        { 
            Int32 newX = a.x + 2 * (b.x - a.x);
            Int32 newY = a.y + 2 * (b.y - a.y);
            return new Location(newY, newX);
        }
        private void FillInAntiNodeLocation(char c, bool partTwo)
        {
            if (!antennaLocations.ContainsKey(c))
            {
                return;
            }
            List<Location> antennaList = antennaLocations[c];
            foreach(var firstAntenna in antennaList)
            {
                if (partTwo)
                {
                    TryToAddLocation(firstAntenna);
                }
                foreach (var secondAntenna in antennaList)
                {
                    if (firstAntenna.Equals(secondAntenna))
                    {
                        continue;
                    }
                    Location nextLocation = GetNextInLine(firstAntenna, secondAntenna);
                    TryToAddLocation(nextLocation);
                    if (partTwo)
                    {
                        Location previousLocation = secondAntenna;
                        Location currentLocation = nextLocation;
                        Int32 maxDepth = Math.Max(height, width);
                        for(Int32 i = 0; i < maxDepth; i++)
                        {
                            nextLocation = GetNextInLine(previousLocation, currentLocation);
                            TryToAddLocation(nextLocation);
                            previousLocation = currentLocation;
                            currentLocation = nextLocation;
                        }
                    }
                }
            }
        }
    }
}
