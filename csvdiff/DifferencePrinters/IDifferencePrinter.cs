using csvdiff.Model;
using System.Collections.Generic;

namespace csvdiff.DifferencePrinters
{
    public interface IDifferencePrinter
    {
        void PrintDifference(List<(CsvRow, CsvRow)> diff);
    }
}