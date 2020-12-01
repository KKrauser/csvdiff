using csvdiff.Model;
using System;
using System.Collections.Generic;

namespace csvdiff.DifferencePrinters
{
    public class ConsolePrinter : DifferencePrinterBase
    {
        public override void PrintDifference(List<(CsvRow, CsvRow)>? diff)
        {
            if (diff is null)
            {
                return;
            }

            var colorCache = Console.ForegroundColor;
            for (int i = 0; i < diff.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}", diff[i].Item1.ToString("C|C"));

                Console.ForegroundColor = colorCache;
                Console.Write(TableSeparator);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0}\n", diff[i].Item2.ToString("C|C"));
            }
            Console.ForegroundColor = colorCache;
        }
    }
}