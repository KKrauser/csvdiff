using csvdiff.Model;
using System.Collections.Generic;

namespace csvdiff.DifferencePrinters
{
    public interface IDifferencePrinter
    {
        void PrintDiff(List<(CsvRow, CsvRow)> diff);
    }
}