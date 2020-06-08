using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace csvdiff
{
    public class RowParser : IRowParser
    {
        private const string CSVPattern = "(?:^|,)(?=[^\"]|(\")?)\"?((?(1)[^\"]*|[^,\"]*))\"?(?=,|$)";
        private static Regex? regex;

        public string[] ParseRow(string? row)
        {
            if (string.IsNullOrEmpty(row))
            {
                return Array.Empty<string>();
            }

            return row.Contains('"') ? ParseDiffucultCase(row) : ParseSimpleCase(row);
        }

        private string[] ParseSimpleCase(string row)
        {
            return !row.Contains(',') ? new string[] { row } : row.Split(',');
        }

        private string[] ParseDiffucultCase(string row)
        {
            if (row.Count(c => c == '"') % 2 != 0
                || !row.StartsWith('"') && row.IndexOf(',') > row.IndexOf('"')
                || !row.EndsWith('"') && row.LastIndexOf(',') < row.LastIndexOf('"'))
            {
                throw new FormatException($"Error. The row \"{row}\" formatting is invalid.");
            }

            if (regex is null)
            {
                regex = new Regex(CSVPattern);
            }

            var matches = regex.Matches(row);
            return matches.Select(m => m.Groups[m.Groups.Count - 1].Value).ToArray<string>();
        }
    }
}