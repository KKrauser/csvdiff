using csvdiff.DifferencePrinters;
using csvdiff.Model;
using System;

namespace csvdiff
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Execute(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Execute(string[] args)
        {
            if(!IsParametersValid(args))
            {
                ShowUsage();
                return;
            }

            var table1 = new CsvTable(args[0]);
            var table2 = new CsvTable(args[1]);

            var differenceDeterminator = new TableDifferenceDeterminator();
            var diff = differenceDeterminator.GetDifferences(table1, table2);

            DifferencePrinterBase? printer = args.Length == 4 ? new FilePrinter(args[3]) : (DifferencePrinterBase)new ConsolePrinter();
            printer.PrintDifference(diff);
        }

        private static bool IsParametersValid(string[] args)
        {
            if (args.Length != 2 && args.Length != 4)
            {
                Console.WriteLine("Error. Wrong parameters count");
                return false;
            }
            if (args.Length == 4 && args[2] != "-s")
            {
                Console.WriteLine($"Error. Cannot recognize the parameter {args[2]}");
                return false;
            }

            return true;
        }

        private static void ShowUsage()
        {
            Console.WriteLine("\nUsage: csvdiff {<Path_To_Table1>} {<Path_To_Table2>} [-s {<File_To_Save.txt>}]");
            Console.WriteLine("-s  - Save the results to file [optional]");
        }
    }
}