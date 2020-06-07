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

            int minRowLength = table1.Rows.Length > table2.Rows.Length ? table2.Rows.Length : table1.Rows.Length;

            for (int i = 0; i < minRowLength; i++)
            {
                if (table1.Rows[i] != table2.Rows[i])
                {
                    diff.Add((table1.Rows[i], table2.Rows[i]));
                }
            }

            if (table1.Rows.Length > minRowLength)
            {
                for (int i = minRowLength; i < table1.Rows.Length; i++)
                {
                    diff.Add((table1.Rows[i], CsvRow.Empty));
                }
            }
            else
            {
                for (int i = minRowLength; i < table2.Rows.Length; i++)
                {
                    diff.Add((CsvRow.Empty, table2.Rows[i]));
                }
            }

            return diff;
        }
    }
}