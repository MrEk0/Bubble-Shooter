using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utils
{
    public static class CSVReader
    {
        public delegate void ReadLineDelegate(int lineIndex, List<string> line);

        public static void LoadFromFile(string fileName, ReadLineDelegate lineReader)
        {
            LoadFromString(File.ReadAllText(fileName), lineReader);
        }

        public static void LoadFromString(string fileContents, ReadLineDelegate lineReader)
        {
            var fileLength = fileContents.Length;

            // read char by char and when a , or \n, perform appropriate action
            var curFileIndex = 0; // index in the file
            var curLine = new List<string>(); // current line of data
            var curLineNumber = 0;
            var curItem = new StringBuilder("");
            var insideQuotes = false; // managing quotes

            while (curFileIndex < fileLength)
            {
                var c = fileContents[curFileIndex++];

                switch (c)
                {
                    case '"':
                        if (!insideQuotes)
                        {
                            insideQuotes = true;
                        }
                        else
                        {
                            if (curFileIndex == fileLength)
                            {
                                // end of file
                                insideQuotes = false;
                                goto case '\n';
                            }
                            else if (fileContents[curFileIndex] == '"')
                            {
                                // double quote, save one
                                curItem.Append("\"");
                                curFileIndex++;
                            }
                            else
                            {
                                // leaving quotes section
                                insideQuotes = false;
                            }
                        }

                        break;
                    case '\r':
                        // ignore it completely
                        break;
                    case ',':
                        goto case '\n';
                    case '\n':
                        if (insideQuotes)
                        {
                            // inside quotes, this characters must be included
                            curItem.Append(c);
                        }
                        else
                        {
                            // end of current item
                            curLine.Add(curItem.ToString());
                            curItem.Length = 0;
                            if (c == '\n' || curFileIndex == fileLength)
                            {
                                // also end of line, call line reader
                                lineReader(curLineNumber++, curLine);
                                curLine.Clear();
                            }
                        }

                        break;
                    default:
                        // other cases, add char
                        curItem.Append(c);
                        break;
                }
            }

            curLine.Add(curItem.ToString());
            curItem.Length = 0;
            // also end of file, call line reader
            lineReader(curLineNumber, curLine);
            curLine.Clear();
        }
    }
}