using System;
using System.Linq;
using Xunit;

using csvdiff.Parsers;

namespace csvdiff.Tests
{
    public class CellsParserTests
    {
        private CellsParser _parser = new CellsParser();

        [Theory]
        [InlineData(",", new string[] { "", "" })]
        [InlineData(",,", new string[] { "", "", "" })]
        [InlineData("Column1,Column2,Column3", new string[] { "Column1", "Column2", "Column3" })]
        [InlineData("Col'umn1,Column2,Column'3", new string[] { "Col'umn1", "Column2", "Column'3" })]
        [InlineData("Column1,,Column'3,,Column5", new string[] { "Column1", "", "Column'3", "", "Column5" })]
        [InlineData(",,Column1,,Column2,Column3,,", new string[] { "", "", "Column1", "", "Column2", "Column3", "", "" })]
        public void ParseLineWithoutQuotesTheory(string line, string[] expected)
        {
            var result = _parser.ParseCells(line);
            Assert.True(result.SequenceEqual(expected));
        }

        [Fact]
        public void ParseEmptyLine()
        {
            var line = string.Empty;
            var result = _parser.ParseCells(line);
            Assert.Equal(Array.Empty<string>(), result);
        }

        [Fact]
        public void ParseNullLine()
        {
            string line = null;
            var result = _parser.ParseCells(line);
            Assert.Equal(Array.Empty<string>(), result);
        }

        [Theory]
        [InlineData(",\"\"\"\",", new string[] { "","\"","" })]
        [InlineData("\"Column1\",\"Column2\",\"Column3\"", new string[] { "Column1", "Column2", "Column3" })]
        [InlineData("\"Co,lu,mn'1\",\"Colum\"\"n2\",\"Col,umn3\"", new string[] { "Co,lu,mn'1", "Colum\"n2", "Col,umn3" })]
        [InlineData(",,\"Col\"\"um,n1\",Column2,,Column3,", new string[] { "", "", "Col\"um,n1", "Column2", "", "Column3", "" })]
        [InlineData("\"\"\"Column1\"\"\",\"RR\"\"RR\",Cell3", new string[] {"\"Column1\"", "RR\"RR", "Cell3"})]
        public void ParseQuotedLineTheory(string line, string[] expected)
        {
            var result = _parser.ParseCells(line);
            Assert.True(result.SequenceEqual(expected));
        }

        [Theory]
        [InlineData("\"Column")]
        [InlineData("Col\"umn")]
        [InlineData("Col\"\"umn")]
        [InlineData("\"Col\"\"umn")]
        public void ParseLineWithWrongFormat(string line)
        {
            Assert.Throws<FormatException>(() => _parser.ParseCells(line));
        }
    }
}