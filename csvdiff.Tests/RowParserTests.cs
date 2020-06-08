using System;
using System.Linq;
using Xunit;

namespace csvdiff.Tests
{
    public class RowParserTests
    {
        private RowParser _parser = new RowParser();

        #region ParseRow Tests

        [Fact]
        public void ParseSimpleRow()
        {
            var row = "Column1,Column2,Column3";
            var expected = new string[] { "Column1", "Column2", "Column3" };

            var result = _parser.ParseRow(row);

            Assert.True(result.SequenceEqual(expected));
        }

        [Fact]
        public void ParseNullRow()
        {
            string row = null;
            var result = _parser.ParseRow(row);
            Assert.Equal(Array.Empty<string>(), result);
        }

        [Fact]
        public void ParseQuotedCellRow()
        {
            var row = "\"Column1\",\"Column2\",\"Column3\"";
            var expected = new string[] { "Column1", "Column2", "Column3" };

            var result = _parser.ParseRow(row);

            Assert.True(result.SequenceEqual(expected));
        }

        [Fact]
        public void ParseRowWithEmptyItems()
        {
            var row = "Column1,,Column3,,Column5";
            var expected = new string[] { "Column1", string.Empty, "Column3", string.Empty, "Column5" };

            var result = _parser.ParseRow(row);

            Assert.True(result.SequenceEqual(expected));
        }

        [Fact]
        public void ParsePartlyQuotedCellRow()
        {
            var row = "\"Column1\",Column2,\"Column3\"";
            var expected = new string[] { "Column1", "Column2", "Column3" };

            var result = _parser.ParseRow(row);

            Assert.True(result.SequenceEqual(expected));
        }

        [Fact]
        public void ParsePartlyQuotedCellRowWithEmptyEntries()
        {
            var row = "\"Column1\",,\"\",Column4,\"Column5\",";
            var expected = new string[] { "Column1", string.Empty, string.Empty, "Column4", "Column5", string.Empty };

            var result = _parser.ParseRow(row);

            Assert.True(result.SequenceEqual(expected));
        }

        [Fact]
        public void ParseWronglyFormattedRow()
        {
            var row = "\"Column1,Column2,\"Column3";
            Assert.Throws<FormatException>(() => _parser.ParseRow(row));
        }

        [Fact]
        public void ParseRowWithCellQuotes()
        {
            var row = "\"\"\"Column1\"\"\",Column2,Column3";
            var expected = new string[] { "\"Column1\"", "Column2", "Column3" };

            var result = _parser.ParseRow(row);

            Assert.True(result.SequenceEqual(expected));
        }

        #endregion ParseRow Tests
    }
}