using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csvdiff.Parsers
{
    public class CellsParser : CellsParserBase
    {
        public override string[] ParseCells(string? line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return Array.Empty<string>();
            }

            return line.Contains(DoubleQuoteMark) ? ParseDiffucultCase(line) : ParseSimpleCase(line);
        }

        private string[] ParseDiffucultCase(string line)
        {
            if (line.Count(ch => ch == DoubleQuoteMark) % 2 == 1)
            {
                throw new FormatException("Format of the .csv file is invalid! Single line should contain even number of double quot marks");
            }

            var parsedCells = new Queue<string>();

            int iterator = 0;
            while (iterator < line.Length)
            {
                string cell = string.Empty;
                if (line[iterator] is DoubleQuoteMark)
                {
                    cell = ParseEscapedCell(line, ref iterator);
                }
                else
                {
                    cell = ParseNonEscapedCell(line, ref iterator);
                }

                parsedCells.Enqueue(cell);

                if (line.Length - 1 == iterator && line[^1] is Separator)
                {
                    parsedCells.Enqueue(string.Empty);
                }

                iterator++;
            }

            return parsedCells.ToArray();
        }

        private string ParseNonEscapedCell(string line, ref int iterator)
        {
            var firstSeparatorIndex = line.IndexOf(Separator, iterator);
            var firstDoubleQuoteIndex = line.IndexOf(DoubleQuoteMark, iterator);

            if (firstDoubleQuoteIndex != -1 && (firstSeparatorIndex is -1 || firstDoubleQuoteIndex < firstSeparatorIndex))
            {
                throw new FormatException($"Cells that contain quotes should be wrapped around with quotes! \r\nLine \"{line}\" is invalid!");
            }

            string cell = string.Empty;
            if (firstSeparatorIndex != -1)
            {
                cell = line[iterator..firstSeparatorIndex];
                iterator = firstSeparatorIndex;
            }
            else
            {
                cell = line.Substring(iterator, line.Length - iterator);
                iterator = line.Length - 1;
            }

            return cell;
        }

        private string ParseEscapedCell(string line, ref int iterator)
        {
            //Add and skip first quote
            var quoteCount = 1;
            iterator++;

            var cellBuilder = new StringBuilder();

            while (true)
            {
                //Check if we still can go on
                if (iterator < line.Length - 1)
                {
                    //This double quote should be escaped by following double quote
                    //or by separator, otherwise throw FormatException
                    if (line[iterator] is DoubleQuoteMark)
                    {
                        quoteCount++;
                        //If true - then row[iterator] double quote is escaped and we should
                        //continue the iteration from the element after escaped double quote
                        if (line[iterator + 1] is DoubleQuoteMark)
                        {
                            quoteCount++;
                            cellBuilder.Append(line[iterator]);

                            //Skip this escaped quote in the next iteration
                            iterator += 2;
                        }
                        //Else if the next symbol after row[iterator] is Separator and
                        //all quotes are closed(even number of quotes) grab cell
                        else if (line[iterator + 1] is Separator && quoteCount % 2 == 0)
                        {
                            iterator++;
                            break;
                        }
                        else
                        {
                            throw new FormatException($"Quote at index {iterator} in Line \"{line}\" should be followed by escape quote or separator!");
                        }
                    }
                    else
                    {
                        cellBuilder.Append(line[iterator]);
                        iterator++;
                    }
                }
                //Current i item is the last item in the string.
                //It should be double quote
                else
                {
                    if (line[iterator] is DoubleQuoteMark)
                    {
                        quoteCount++;
                        break;
                    }
                    else
                    {
                        throw new FormatException($"The last item of the line \"{line}\" with last escaped cell should be double quote");
                    }
                }
            }

            return cellBuilder.ToString();
        }
    }
}