using System;

namespace csvdiff.Parsers
{
    public abstract class ExcelCellsParserBase : ICsvCellsParser
    {
        public const char Separator = ',';
        public const char DoubleQuoteMark = '"';

        public abstract string[] ParseCells(string row);

        protected virtual string[] ParseSimpleCase(string row)
        {
            if (row.Contains('"'))
            {
                throw new FormatException("Simple csv row shouldn't contain quotes!");
            }

            return !row.Contains(',') ? new string[] { row } : row.Split(',');
        }
    }
}