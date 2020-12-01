using System;
using System.IO;

namespace csvdiff
{
    public class TableRowsReader : ITableRowsReader
    {
        public string[] ReadAllLines(string csvFilePath)
        {
            CheckFileExtension(csvFilePath);
            return File.ReadAllLines(csvFilePath);
        }

        private void CheckFileExtension(string csvFilePath)
        {
            if (!Path.HasExtension(csvFilePath) || Path.GetExtension(csvFilePath) != ".csv")
            {
                throw new ArgumentException("Error. Only files with .csv extension allowed");
            }
        }
    }
}