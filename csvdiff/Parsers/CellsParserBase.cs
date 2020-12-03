using System;

namespace csvdiff.Parsers
{
    public abstract class CellsParserBase : ICsvCellsParser
    {
        public const char Separator = ',';
        public const char DoubleQuoteMark = '"';

        public abstract string[] ParseCells(string line);

        protected virtual string[] ParseSimpleCase(string line)
        {
            if (line.Contains('"'))
            {
                throw new FormatException("Simple csv line shouldn't contain quotes!");
            }

            return !line.Contains(',') ? new string[] { line } : line.Split(',');
        }
    }
}