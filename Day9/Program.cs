using System.Numerics;
namespace Day9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "inputTest.txt";
            StreamReader reader = new StreamReader(filePath);
            string input = reader.ReadLine();
            reader.Close();
            HardDrive drive = new HardDrive(input);
            drive.QuickDefrag();
            Console.WriteLine(drive.CheckSum());
            drive = new HardDrive(input);
            drive.Defrag();
            Console.WriteLine(drive.CheckSum());
        }
    }
    class HardDrive
    {
        readonly Int32 hardDriveLength;
        readonly Int32 numberOfFiles;
        File[] files;
        List<FreePage> freePages;
        Int32[] disk;
        public HardDrive(string input)
        {
            Int32 currentFileNumber = 0;
            Int32 stringLength = input.Length;
            Int32 currentStringIndex = 0;
            List<Int32> diskValues = new List<Int32>();
            List<File> tempFiles = new List<File>(); ;
            freePages = new List<FreePage>();
            while (true)
            {
                Int32 fileLength = Int32.Parse(input[currentStringIndex].ToString());
                tempFiles.Add(new File(diskValues.Count, fileLength, currentFileNumber));
                for (Int32 fileIndex = 0; fileIndex < fileLength; fileIndex++)
                {
                    diskValues.Add(currentFileNumber);
                }
                currentFileNumber++;
                currentStringIndex++;
                if (currentStringIndex >= stringLength)
                {
                    break;
                }
                fileLength = Int32.Parse(input[currentStringIndex].ToString());
                freePages.Add(new FreePage(diskValues.Count, fileLength));
                for (Int32 fileIndex = 0; fileIndex < fileLength; fileIndex++)
                {
                    diskValues.Add(-1);
                }
                currentStringIndex++;
                if (currentStringIndex >= stringLength)
                {
                    break;
                }
            }
            disk = diskValues.ToArray();
            hardDriveLength = disk.Length;
            files = tempFiles.ToArray();
            numberOfFiles = files.Length;
        }
        public void Defrag()
        {
            for (Int32 fileNumber = numberOfFiles - 1; fileNumber > 0; fileNumber--)
            {
                File file = files[fileNumber];
                Int32 desiredFreeSpace = file.fileSize;
                foreach(var freePage in freePages)
                {
                    if (freePage.pageSize < desiredFreeSpace)
                    {
                        continue;
                    }
                    if (freePage.startIndex > file.startIndex)
                    {
                        break;
                    }
                    MoveFile(freePages.IndexOf(freePage), fileNumber);
                    break;
                }
            }
            freePages.Sort((x,y)=>x.startIndex.CompareTo(y.startIndex));
            ConsolidateEmptyPages();
            Console.WriteLine();
        }
        void ConsolidateEmptyPages()
        {
            Int32 numberOfPages = freePages.Count;
            List<Int32> indicesToRemove = new List<Int32>();
            for(Int32 i = 0; i < numberOfPages; i++)
            {
                for(Int32 j = i + 1; j < numberOfPages; j++)
                {
                    FreePage firstPage = freePages[i];
                    FreePage secondPage = freePages[j];
                    if (firstPage.startIndex + firstPage.pageSize >= secondPage.startIndex)
                    {
                        firstPage.pageSize += secondPage.pageSize-(secondPage.startIndex-(firstPage.startIndex+firstPage.pageSize));
                        if (!indicesToRemove.Contains(j))
                        {
                            indicesToRemove.Add(j);
                        }
                    }
                }
            }
            indicesToRemove.Sort();
            Int32 numberOfPagesToRemove = indicesToRemove.Count;
            for(Int32 i = 0; i < numberOfPagesToRemove; i++)
            {
                freePages.RemoveAt(indicesToRemove[i] - i);
            }
        }
        void MoveFile(Int32 emptyPageIndex,Int32 fileIndex)
        {
            File file = files[fileIndex];
            FreePage page = freePages[emptyPageIndex];
            freePages.Add(new FreePage(file.startIndex, file.fileSize));
            Int32 newFileStartLocation = page.startIndex;
            for(Int32 i = 0; i < file.fileSize; i++)
            {
                Int32 diskLocation = newFileStartLocation + i;
                disk[diskLocation] = file.fileNumber;
                diskLocation = file.startIndex+i;
                disk[diskLocation] = -1;
                page.startIndex++;
                page.pageSize--;
            }
            if (page.pageSize == 0)
            {
                freePages.RemoveAt(emptyPageIndex);
            }
            file.startIndex = newFileStartLocation;
        }
        public void QuickDefrag()
        {
            for(Int32 i = hardDriveLength - 1; i > 0; i--)
            {
                if (disk[i] == -1)
                {
                    continue;
                }
                for(Int32 j = 0; j < i; j++)
                {
                    if (disk[j] != -1)
                    {
                        continue;
                    }
                    disk[j] = disk[i];
                    disk[i] = -1;
                    break;
                }
            }
        }
        public BigInteger CheckSum()
        {
            BigInteger checkSum = 0;
            for (Int32 i = 0; i < hardDriveLength; i++)
            {
                if (disk[i] == -1)
                {
                    continue;
                }
                checkSum += i * disk[i];
            }
            return checkSum;
        }
    }
    class File
    {
        public Int32 startIndex;
        public Int32 fileSize;
        public Int32 fileNumber;
        public File(Int32 index, Int32 size, Int32 number)
        {
            startIndex = index;
            fileSize = size;
            fileNumber = number;
        }
    }
    class FreePage
    {
        public Int32 startIndex;
        public Int32 pageSize;
        public FreePage(Int32 index, Int32 size)
        {
            startIndex = index;
            pageSize = size;
        }
    }
}