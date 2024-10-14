using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChessMemoryAppRemastered.Model
{
    internal class ChessTextToJson
    {
        public static async Task<string> GetContentFromFile(string path)
        {
            using Stream stream = await FileSystem.Current.OpenAppPackageFileAsync(path);
            using var streamReader = new StreamReader(stream);

            const int bufferSize = 4096;
            var buffer = new char[bufferSize];
            var stringBuilder = new StringBuilder();

            int bytesRead;
            while ((bytesRead = await streamReader.ReadAsync(buffer, 0, bufferSize)) > 0)
            {
                stringBuilder.Append(buffer, 0, bytesRead);
            }

            return stringBuilder.ToString();
        }

        internal static async Task<HashSet<string>> GetLinesFromFile(string path)
        {
            var lines = new HashSet<string>();

            using Stream stream = await FileSystem.Current.OpenAppPackageFileAsync(path);
            using var streamReader = new StreamReader(stream);

            while (!streamReader.EndOfStream)
            {
                string? currentLine = streamReader.ReadLine();
                if (currentLine != null && currentLine.Length > 0)
                    lines.Add(currentLine.Trim());
            }

            return lines;
        }

        internal static string GenerateJson(HashSet<string> lines)
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");

            int syllables = 1;
            foreach (var line in lines)
            {
                if (IsStringChessCoordinate(line))
                {
                    if (syllables == 5)
                        sb.AppendLine("},");
                    syllables = 1;
                    sb.AppendLine($"\"{line}\": " + "{");
                }
                else
                {
                    string newLine = "";
                    string[] words = line.Split(',');
                    foreach (var word in words)
                    {
                        newLine += $"\"{word.Replace(" ", "")}\", ";
                    }
                    sb.AppendLine($"\"{syllables}\":[{newLine[..^2]}]" + (syllables != 4 ? "," : ""));
                    syllables++;
                }
            }

            sb.AppendLine("}");

            return sb.ToString();
        }



        private static bool IsStringChessCoordinate(string text)
        {
            return text.Length == 2;
        }
    }
}
