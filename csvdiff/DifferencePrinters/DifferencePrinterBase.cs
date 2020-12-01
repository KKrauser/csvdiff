using csvdiff.Model;
using System.Collections.Generic;

namespace csvdiff.DifferencePrinters
{
    public abstract class DifferencePrinterBase
    {
        protected const string TableSeparator = "  ||  ";

        public abstract void PrintDifference(List<(CsvRow, CsvRow)> diff);
    }
}