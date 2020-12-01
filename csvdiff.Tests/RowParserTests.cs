using System;
using System.Linq;
using Xunit;

using csvdiff.Parsers;

namespace csvdiff.Tests
{
    public class RowParserTests
    {
        private ExcelCellsParser _parser = new ExcelCellsParser();

        [Theory]
        [InlineData(",", new string[] { "", "" })]
        [InlineData("Column1,Column2,Column3", new string[] { "Column1", "Column2", "Column3" })]
        [InlineData("Column1,,Column3,,Column5", new string[] { "Column1", "", "Column3", "", "Column5" })]
        [InlineData(",,Column1,Column2,Column3,,", new string[] { "", "", "Column1", "Column2", "Column3", "", "" })]
        public void ParseRowWithoutQuotesTheory(string row, string[] expected)
        {
            var result = _parser.ParseCells(row);
            Assert.True(result.SequenceEqual(expected));
        }

        [Fact]
        public void ParseEmptyRow()
        {
            var row = string.Empty;
            var result = _parser.ParseCells(row);
            Assert.True(result.SequenceEqual(Array.Empty<string>()));
        }

        [Fact]
        public void ParseNullRow()
        {
            string row = null;
            var result = _parser.ParseCells(row);
            Assert.Equal(Array.Empty<string>(), result);
        }

        [Theory]
        [InlineData("\"Column1\",\"Column2\",\"Column3\"", new string[] { "Column1", "Column2", "Column3" })]
        [InlineData("\"Co,lu,mn1\",\"Colum\"\"n2\",\"Col,umn3\"", new string[] { "Co,lu,mn1", "Colum\"n2", "Col,umn3" })]
        [InlineData(",,\"Col\"\"um,n1\",Column2,,Column3,", new string[] { "", "", "Col\"um,n1", "Column2", "", "Column3", "" })]
        [InlineData("\"\"\"Column1\"\"\",\"RR\"\"RR\",Cell3", new string[] {"\"Column1\"", "RR\"RR", "Cell3"})]
        public void ParseQuotedRowTheory(string row, string[] expected)
        {
            var result = _parser.ParseCells(row);
            Assert.True(result.SequenceEqual(expected));
        }

        [Theory]
        [InlineData("\"Column")]
        [InlineData("Col\"umn")]
        [InlineData("Col\"\"umn")]
        [InlineData("\"Col\"\"umn")]
        public void ParseRowWithWrongFormat(string row)
        {
            Assert.Throws<FormatException>(() => _parser.ParseCells(row));
        }
    }
}