using System.Diagnostics.CodeAnalysis;
using static Day6.AdventOfCode;
namespace Day6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filepath = "inputs.txt";
            StreamReader reader = new StreamReader(filepath);
            char[,] inputMap = GetInput(reader);
            reader.Close();
            AdventOfCode partOne = new AdventOfCode(inputMap);
            Console.WriteLine(SolvePartOne(partOne));
            List<AdventOfCode> possibleInputs = GeneratePossibleInputs(inputMap);
            Int32 sum = 0;
            Console.WriteLine($"Number of possibilities is {possibleInputs.Count}");
            foreach(var possibleInput in possibleInputs)
            {
                if (possibleInputs.IndexOf(possibleInput) % 2000 == 0)
                {
                    Console.WriteLine($"Currently on possibility {possibleInputs.IndexOf(possibleInput)}");
                }
                if (TestPartTwo(possibleInput))
                {
                    sum++;
                }
            }
            Console.WriteLine(sum);
        }
        static List<AdventOfCode> GeneratePossibleInputs(char[,] inputPuzzle)
        {
            List<AdventOfCode> output = new List<AdventOfCode>();
            Int32 length = inputPuzzle.GetLength(0);
            Int32 width = inputPuzzle.GetLength(1);
            for (Int32 i = 0; i < length; i++)
            {
                for (Int32 j = 0; j < width; j++)
                {
                    AdventOfCode nextOutput = new AdventOfCode(inputPuzzle);
                    if (nextOutput.TryToAddObstacle(i, j))
                    {
                        output.Add(nextOutput);
                    }
                }
            }
            return output;
        }
        static bool TestPartTwo(AdventOfCode input)
        {
            Dictionary<(Int32, Int32), List<Direction>> visitedPoints = new Dictionary<(Int32, Int32), List<Direction>>();
            while (!input.GuardEscaped)
            {
                (Int32, Int32) guardPosition = (input.GuardPositionY, input.GuardPositionX);
                if (!visitedPoints.ContainsKey(guardPosition))
                {
                    visitedPoints.Add(guardPosition, new List<Direction>());
                }
                if (visitedPoints[guardPosition].Contains(input.CurrentGuardDirection))
                {
                    return true;
                }
                visitedPoints[guardPosition].Add(input.CurrentGuardDirection);
                input.AdvanceGuard();
            }
            return false;
        }
        static Int32 SolvePartOne(AdventOfCode puzzle)
        {
            while (!puzzle.GuardEscaped)
            {
                puzzle.AdvanceGuard();
            }
            Int32 total = puzzle.CountVisitedSteps();
            return total;
        }
        static char[,] GetInput(StreamReader reader)
        {
            string? nextInput = reader.ReadLine();
            Int32 width = nextInput.Length;
            Int32 length = 1;
            while (true)
            {
                nextInput = reader.ReadLine();
                if (nextInput == null)
                {
                    break;
                }
                length++;
            }
            char[,] output = new char[length, width];
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            for (Int32 i = 0; i < length; i++)
            {
                string? nextLine = reader.ReadLine();
                if (nextLine == null)
                {
                    throw new Exception("Unexpected Error");
                }
                for (Int32 j = 0; j < width; j++)
                {
                    output[i, j] = nextLine[j];
                }
            }
            return output;
        }
    }
    internal class AdventOfCode
    {
        char[,] puzzleMap;
        public Int32 GuardPositionX { get; private set; }
        public Int32 GuardPositionY { get; private set; }
        readonly Int32 puzzleHeight;
        readonly Int32 puzzleWidth;
        public bool GuardEscaped { get; private set; }
        public Direction CurrentGuardDirection { get; private set; }
        public enum Direction
        {
            North,
            East,
            South,
            West
        }
        public void AdvanceGuard()
        {
            (Int32, Int32) nextGuardPosition;
            if (CurrentGuardDirection == Direction.North)
            {
                nextGuardPosition = (GuardPositionY - 1, GuardPositionX);
            }
            else if (CurrentGuardDirection == Direction.East)
            {
                nextGuardPosition = (GuardPositionY, GuardPositionX + 1);
            }
            else if (CurrentGuardDirection == Direction.South)
            {
                nextGuardPosition = (GuardPositionY + 1, GuardPositionX);
            }
            else if (CurrentGuardDirection == Direction.West)
            {
                nextGuardPosition = (GuardPositionY, GuardPositionX - 1);
            }
            else
            {
                throw new Exception("invalid guard direction");
            }
            if (GuardEscapesNextStep(nextGuardPosition.Item1, nextGuardPosition.Item2))
            {
                MarkPointAsVisited(GuardPositionY, GuardPositionX);
                GuardEscaped = true;
                return;
            }
            if (GuardIsBlockedNextStep(nextGuardPosition.Item1, nextGuardPosition.Item2))
            {
                CurrentGuardDirection = (Direction)(((int)CurrentGuardDirection + 1) % 4);
                return;
            }
            MarkPointAsVisited(GuardPositionY, GuardPositionX);
            GuardPositionY = nextGuardPosition.Item1;
            GuardPositionX = nextGuardPosition.Item2;
        }
        public Int32 CountVisitedSteps()
        {
            Int32 output = 0;
            for(Int32 i = 0; i < puzzleHeight; i++)
            {
                for(Int32 j = 0; j < puzzleWidth; j++)
                {
                    if (puzzleMap[i, j] == 'X')
                    {
                        output++;
                    }
                }
            }
            return output;
        }
        bool GuardIsBlockedNextStep(Int32 newY, Int32 newX)
        {
            if (puzzleMap[newY, newX] == '#')
            {
                return true;
            }
            return false;
        }
        bool GuardEscapesNextStep(Int32 newY,Int32 newX)
        {
            if (newY < 0 || newY >= puzzleHeight)
            {
                return true;
            }
            if (newX < 0 || newX >= puzzleWidth)
            {
                return true;
            }
            return false;
        }
        public bool TryToAddObstacle(Int32 y,Int32 x)
        {
            if (puzzleMap[y, x] == '.')
            {
                puzzleMap[y, x] = '#';
                return true;
            }
            else
            {
                return false;
            }
        }
        void MarkPointAsVisited(Int32 y,Int32 x)
        {
            puzzleMap[y, x] = 'X';
        }
        public char PuzzleMapValue(Int32 y,Int32 x)
        {
            return puzzleMap[y, x];
        }
        public AdventOfCode(char[,] input)
        {
            puzzleWidth = input.GetLength(1);
            puzzleHeight = input.GetLength(0);
            puzzleMap = input.Clone() as char[,];
            for (Int32 i = 0; i < puzzleHeight; i++)
            {
                for (Int32 j = 0; j < puzzleWidth; j++)
                {
                    if (puzzleMap[i, j] == '^')
                    {
                        GuardPositionY = i;
                        GuardPositionX = j;
                    }
                }
            }
            CurrentGuardDirection = Direction.North;
        }
    }
}