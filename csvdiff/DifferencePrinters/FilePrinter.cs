using csvdiff.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace csvdiff.DifferencePrinters
{
    public class FilePrinter : DifferencePrinterBase
    {
        private string _path;

        public FilePrinter(string path)
        {
            if (Path.GetExtension(path) != ".txt")
            {
                throw new ArgumentException("Error. Result can be saved only as .txt file");
            }
            _path = path;
        }

        public override void PrintDifference(List<(CsvRow, CsvRow)>? diff)
        {
            if (diff is null)
            {
                return;
            }

            var output = PrepareOutput(diff);
            WriteToFile(output);
        }

        private void WriteToFile(string output)
        {
            if (File.Exists(_path))
            {
                var repeat = default(bool);
                do
                {
                    repeat = false;
                    Console.Write($"{_path} - file already exists. Overwrite (y,n)?");
                    var answer = Console.ReadLine();

                    switch (answer.ToUpper())
                    {
                        case "Y":
                            break;

                        case "N":
                            {
                                for (int i = 1; ; i++)
                                {
                                    var newPath = Path.GetFileNameWithoutExtension(_path) + i + Path.GetExtension(_path);
                                    if (!File.Exists(newPath))
                                    {
                                        _path = newPath;
                                        break;
                                    }
                                }
                                break;
                            }
                        default:
                            {
                                repeat = true;
                                break;
                            }
                    }
                } while (repeat);
            }

            File.WriteAllText(_path, output, Encoding.Unicode);
            Console.WriteLine($"Result successfully saved as {_path}");
        }

        private string PrepareOutput(List<(CsvRow, CsvRow)> diff)
        {
            var difference = new StringBuilder(diff.Count * 30);
            foreach ((CsvRow, CsvRow) row in diff)
            {
                difference.AppendLine($"{row.Item1}{TableSeparator}{row.Item2}");
            }

            return difference.ToString();
        }
    }
}