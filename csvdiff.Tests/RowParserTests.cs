using csvdiff;
using System;
using System.Linq;
using Xunit;

public class RowParserTests
{
    private RowParser parser = new RowParser();

    #region ParseRow Tests

    [Fact]
    public void ParseSimpleRow()
    {
        var row = "Column1,Column2,Column3";
        var expected = new string[] { "Column1", "Column2", "Column3" };

        var result = parser.ParseRow(row);

        Assert.True(result.SequenceEqual(expected));
    }

    [Fact]
    public void ParseNullRow()
    {
        string row = null;
        var result = parser.ParseRow(row);
        Assert.Equal(Array.Empty<string>(), result);
    }

    [Fact]
    public void ParseQuotedCellRow()
    {
        var row = "\"Column1\",\"Column2\",\"Column3\"";
        var expected = new string[] { "Column1", "Column2", "Column3" };

        var result = parser.ParseRow(row);

        Assert.True(result.SequenceEqual(expected));
    }

    [Fact]
    public void ParseRowWithEmptyItems()
    {
        var row = "Column1,,Column3,,Column5";
        var expected = new string[] { "Column1", string.Empty, "Column3", string.Empty, "Column5" };

        var result = parser.ParseRow(row);

        Assert.True(result.SequenceEqual(expected));
    }

    [Fact]
    public void ParsePartlyQuotedCellRow()
    {
        var row = "\"Column1\",Column2,\"Column3\"";
        var expected = new string[] { "Column1", "Column2", "Column3" };

        var result = parser.ParseRow(row);

        Assert.True(result.SequenceEqual(expected));
    }

    [Fact]
    public void ParsePartlyQuotedCellRowWithEmptyEntries()
    {
        var row = "\"Column1\",,\"\",Column4,\"Column5\",";
        var expected = new string[] { "Column1", string.Empty, string.Empty, "Column4", "Column5", string.Empty };

        var result = parser.ParseRow(row);

        Assert.True(result.SequenceEqual(expected));
    }

    [Fact]
    public void ParseWronglyFormattedRow()
    {
        var row = "\"Column1,Column2,\"Column3";
        Assert.Throws<FormatException>(() => parser.ParseRow(row));
    }

    [Fact]
    public void ParseRowWithCellQuotes()
    {
        var row = "\"\"\"Column1\"\"\",Column2,Column3";
        var expected = new string[] { "\"Column1\"", "Column2", "Column3" };

        var result = parser.ParseRow(row);

        Assert.True(result.SequenceEqual(expected));
    }

    #endregion ParseRow Tests
}