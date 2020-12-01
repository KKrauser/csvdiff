using csvdiff.Model;
using System.Collections.Generic;

namespace csvdiff
{
    public class TableDifferenceDeterminator : ITableDifferenceDeterminator
    {
        public List<(CsvRow, CsvRow)> GetDifferences(CsvTable table1, CsvTable table2)
        {
            var diff = new List<(CsvRow, CsvRow)>();
            if (table1 == table2 || (table1.Rows == null || table2.Rows == null))
            {
                return diff;
            }

            int minRowLength = table1.Rows.Count > table2.Rows.Count ? table2.Rows.Count : table1.Rows.Count;

            for (int i = 0; i < minRowLength; i++)
            {
                if (table1.Rows[i] != table2.Rows[i])
                {
                    diff.Add((table1.Rows[i], table2.Rows[i]));
                }
            }

            if (table1.Rows.Count > minRowLength)
            {
                for (int i = minRowLength; i < table1.Rows.Count; i++)
                {
                    diff.Add((table1.Rows[i], CsvRow.Empty));
                }
            }
            else
            {
                for (int i = minRowLength; i < table2.Rows.Count; i++)
                {
                    diff.Add((CsvRow.Empty, table2.Rows[i]));
                }
            }

            return diff;
        }
    }
}