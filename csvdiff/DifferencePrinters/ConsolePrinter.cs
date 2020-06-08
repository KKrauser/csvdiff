using csvdiff.Model;
using System;
using System.Collections.Generic;

namespace csvdiff.DifferencePrinters
{
    public class ConsolePrinter : IDifferencePrinter
    {
        private const string Separator = " / ";

        public void PrintDifference(List<(CsvRow, CsvRow)> diff)
        {
            var colorCache = Console.ForegroundColor;
            for (int i = 0; i < diff.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}", diff[i].Item1);

                Console.ForegroundColor = colorCache;
                Console.Write(Separator);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0}\n", diff[i].Item2);
            }
            Console.ForegroundColor = colorCache;
        }
    }
}