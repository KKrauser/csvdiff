using csvdiff.Model;
using System.Collections.Generic;

namespace csvdiff
{
    public interface ITableDifferenceDeterminator
    {
        List<(CsvRow, CsvRow)> GetDifferences(CsvTable table1, CsvTable table2);
    }
}