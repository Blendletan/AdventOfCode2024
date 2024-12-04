namespace Day4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string filepath = "input.txt";
            StreamReader reader = new StreamReader(filepath);
            char[,] input = GetInputs(reader);
            Int32 output = GetMatches(input, "XMAS".ToCharArray());
            Console.WriteLine(output);
            output = GetMatchesPartTwo(input, "MAS".ToCharArray());
            Console.WriteLine(output);
            Console.WriteLine("Hello, World!");
        }
        static Int32 GetMatchesPartTwo(char[,] wordSearch, char[] wordToMatch)
        {
            Int32 length = wordSearch.GetLength(0);
            Int32 width = wordSearch.GetLength(1);
            Int32 numberOfMatches = 0;
            for (Int32 i = 0; i < length; i++)
            {
                for (Int32 j = 0; j < width; j++)
                {
                    if (CheckMatchDiagonalRight(wordSearch, wordToMatch, i, j))
                    {
                        if (CheckMatchDiagonalLeft(wordSearch, wordToMatch, i, j + 2))
                        {
                            numberOfMatches++;
                            continue;
                        }
                        else
                        {
                            Array.Reverse(wordToMatch);
                            if (CheckMatchDiagonalLeft(wordSearch, wordToMatch, i, j + 2))
                            {
                                Array.Reverse(wordToMatch);
                                numberOfMatches++;
                                continue;
                            }
                        }
                    }
                    //again with the word to match reversed, which will end with it back in the original order
                    Array.Reverse(wordToMatch);
                    if (CheckMatchDiagonalRight(wordSearch, wordToMatch, i, j))
                    {
                        if (CheckMatchDiagonalLeft(wordSearch, wordToMatch, i, j + 2))
                        {
                            numberOfMatches++;
                            continue;
                        }
                        else
                        {
                            Array.Reverse(wordToMatch);
                            if (CheckMatchDiagonalLeft(wordSearch, wordToMatch, i, j + 2))
                            {
                                Array.Reverse(wordToMatch);
                                numberOfMatches++;
                                continue;
                            }
                        }
                    }
                    Array.Reverse(wordToMatch);
                }
            }
            return numberOfMatches;
        }
        static Int32 GetMatches(char[,] wordSearch, char[] wordToMatch)
        {
            Int32 length = wordSearch.GetLength(0);
            Int32 width = wordSearch.GetLength(1);
            Int32 numberOfMatches= 0;
            for(Int32 i = 0; i < length; i++)
            {
                for(Int32 j = 0; j < width; j++)
                {
                    if (CheckMatchDiagonalLeft(wordSearch,wordToMatch,i, j))
                    {
                        numberOfMatches++;
                    }
                    if (CheckMatchDiagonalRight(wordSearch, wordToMatch, i, j))
                    {
                        numberOfMatches++;
                    }
                    if (CheckMatchDown(wordSearch, wordToMatch, i, j))
                    {
                        numberOfMatches++;
                    }
                    if (CheckMatchRight(wordSearch, wordToMatch, i, j))
                    {
                        numberOfMatches++;
                    }
                    Array.Reverse(wordToMatch);
                    if (CheckMatchDiagonalLeft(wordSearch, wordToMatch, i, j))
                    {
                        numberOfMatches++;
                    }
                    if (CheckMatchDiagonalRight(wordSearch, wordToMatch, i, j))
                    {
                        numberOfMatches++;
                    }
                    if (CheckMatchDown(wordSearch, wordToMatch, i, j))
                    {
                        numberOfMatches++;
                    }
                    if (CheckMatchRight(wordSearch, wordToMatch, i, j))
                    {
                        numberOfMatches++;
                    }
                    Array.Reverse(wordToMatch);
                }
            }
            return numberOfMatches;
        }
        static bool CheckMatchDiagonalRight(char[,] wordSearch, char[] wordToMatch,Int32 i,Int32 j)
        {
            Int32 length = wordSearch.GetLength(0);
            Int32 width = wordSearch.GetLength(1);
            Int32 wordLength = wordToMatch.Length;
            if (i + wordLength > length)
            {
                return false;
            }
            if (j+wordLength > width)
            {
                return false;
            }
            for(Int32 indexOffset = 0; indexOffset < wordLength; indexOffset++)
            {
                if (wordSearch[i+indexOffset,j+indexOffset] != wordToMatch[indexOffset])
                {
                    return false;
                }
            }
            return true;
        }
        static bool CheckMatchDiagonalLeft(char[,] wordSearch, char[] wordToMatch, Int32 i, Int32 j)
        {
            Int32 length = wordSearch.GetLength(0);
            Int32 width = wordSearch.GetLength(1);
            Int32 wordLength = wordToMatch.Length;
            if (i + wordLength > length)
            {
                return false;
            }
            if (j - wordLength < -1)
            {
                return false;
            }
            for (Int32 indexOffset = 0; indexOffset < wordLength; indexOffset++)
            {
                char c = wordSearch[i + indexOffset, j - indexOffset];
                if (c != wordToMatch[indexOffset])
                {
                    return false;
                }
            }
            return true;
        }
        static bool CheckMatchDown(char[,] wordSearch, char[] wordToMatch, Int32 i, Int32 j)
        {
            Int32 length = wordSearch.GetLength(0);
            Int32 width = wordSearch.GetLength(1);
            Int32 wordLength = wordToMatch.Length;
            if (i + wordLength > length)
            {
                return false;
            }
            for (Int32 indexOffset = 0; indexOffset < wordLength; indexOffset++)
            {
                if (wordSearch[i + indexOffset, j] != wordToMatch[indexOffset])
                {
                    return false;
                }
            }
            return true;
        }
        static bool CheckMatchRight(char[,] wordSearch, char[] wordToMatch, Int32 i, Int32 j)
        {
            Int32 length = wordSearch.GetLength(0);
            Int32 width = wordSearch.GetLength(1);
            Int32 wordLength = wordToMatch.Length;
            if (j + wordLength > width)
            {
                return false;
            }
            for (Int32 indexOffset = 0; indexOffset < wordLength; indexOffset++)
            {
                if (wordSearch[i, j + indexOffset] != wordToMatch[indexOffset])
                {
                    return false;
                }
            }
            return true;
        }
        static char[,] GetInputs(StreamReader reader)
        {
            string? nextInput = reader.ReadLine();
            Int32 width = nextInput.Length;
            Int32 length = 1;
            while(true)
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
            for(Int32 i = 0; i < length; i++)
            {
                string? nextLine = reader.ReadLine();
                if (nextLine == null)
                {
                    throw new Exception("Unexpected Error");
                }
                for(Int32 j = 0; j < width; j++)
                {
                    output[i, j] = nextLine[j];
                }
            }
            return output;
        }
    }
}