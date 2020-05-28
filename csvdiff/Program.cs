using System;

namespace csvdiff
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args is null || args.Length <= 1)
            {
                ShowUsage();
            }
            if (args.Length >= 3 && args[2] != "-s")
            {
                Console.WriteLine($"Unknown argument - \"{args[2]}\"");
                return;
            }

            //parallel loading of tables
            //comparing
            //output the result
        }

        private static void ShowUsage()
        {
            Console.WriteLine("\nUsage: csvdiff {<Table1>} {<Table2>} [-s [<directory_to_save>]]");
            Console.WriteLine("-s  - Save the results to file [optional]");
            Console.WriteLine("If the directory is not specified - the file will be saved to the directory with csvdiff utility.");
        }
    }
}
