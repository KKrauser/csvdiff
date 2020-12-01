using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csvdiff.Parsers
{
    public class ExcelCellsParser : ExcelCellsParserBase
    {
        public override string[] ParseCells(string? row)
        {
            if (string.IsNullOrEmpty(row))
            {
                return Array.Empty<string>();
            }

            return row.Contains(DoubleQuoteMark) ? ParseDiffucultCase(row) : ParseSimpleCase(row);
        }

        private string[] ParseDiffucultCase(string row)
        {
            if (row.Count(ch => ch == DoubleQuoteMark) % 2 == 1)
            {
                throw new FormatException("Format of the .csv file is invalid! Single line should contain even number of double quot marks");
            }

            var parsedRow = new List<string>();

            int iterator = 0;
            while (iterator < row.Length)
            {
                string cell = string.Empty;
                if (row[iterator] is DoubleQuoteMark)
                {
                    cell = ParseEscapedCell(row, ref iterator);
                }
                else
                {
                    cell = ParseNonEscapedCell(row, ref iterator);
                    if (row.Length - 1 == iterator && row[^1] is Separator)
                    {
                        parsedRow.Add(cell);
                        parsedRow.Add(string.Empty);
                        break;
                    }
                }

                parsedRow.Add(cell);
                iterator++;
            }

            return parsedRow.ToArray();
        }

        private string ParseNonEscapedCell(string row, ref int iterator)
        {
            var firstSeparatorIndex = row.IndexOf(Separator, iterator);
            var firstDoubleQuoteIndex = row.IndexOf(DoubleQuoteMark, iterator);

            if (firstDoubleQuoteIndex != -1 && (firstSeparatorIndex is -1 || firstDoubleQuoteIndex < firstSeparatorIndex))
            {
                throw new FormatException($"Cells that contain quotes should be wrapped around with quotes! \r\n Line \"{row}\" is invalid!");
            }

            string cell = string.Empty;
            if (firstSeparatorIndex != -1)
            {
                cell = row[iterator..firstSeparatorIndex];
                iterator = firstSeparatorIndex;
            }
            else
            {
                cell = row.Substring(iterator, row.Length - iterator);
                iterator = row.Length - 1;
            }

            return cell;
        }

        private string ParseEscapedCell(string row, ref int iterator)
        {
            //Add and skip first quote
            var quoteCount = 1;
            iterator++;

            var cellBuilder = new StringBuilder();

            while (true)
            {
                //Check if we still can go on
                if (iterator < row.Length - 1)
                {
                    //This double quote should be escaped by following double quote
                    //or by separator, otherwise throw FormatException
                    if (row[iterator] is DoubleQuoteMark)
                    {
                        quoteCount++;
                        //If true - then row[iterator] double quote is escaped and we should
                        //continue the iteration from the element after escaped double quote
                        if (row[iterator + 1] is DoubleQuoteMark)
                        {
                            quoteCount++;
                            cellBuilder.Append(row[iterator]);

                            //Skip this escaped quote in the next iteration
                            iterator += 2;
                        }
                        //Else if the next symbol after row[iterator] is Separator and
                        //all quotes are closed(even number of quotes) grab cell
                        else if (row[iterator + 1] is Separator && quoteCount % 2 == 0)
                        {
                            iterator++;
                            break;
                        }
                        else
                        {
                            throw new FormatException($"Quote at index {iterator} in Line \"{row}\" should be followed by escape quote or separator!");
                        }
                    }
                    else
                    {
                        cellBuilder.Append(row[iterator]);
                        iterator++;
                    }
                }
                //Current i item is the last item in the string.
                //It should be double quote
                else
                {
                    if (row[iterator] is DoubleQuoteMark)
                    {
                        quoteCount++;
                        break;
                    }
                    else
                    {
                        throw new FormatException($"The last item of the line \"{row}\" with last escaped cell should be double quote");
                    }
                }
            }

            return cellBuilder.ToString();
        }
    }
}