using csvdiff;
using System;
using System.IO;
using Xunit;

public class TableRowsReaderTests
{
    private TableRowsReader loader = new TableRowsReader();

    #region ReadAllLines Tests

    [Fact]
    public void ReadAllLinesPathNull()
    {
        string path = null;
        Assert.Throws<ArgumentException>(() => loader.ReadAllLines(path));
    }

    [Fact]
    public void ReadAllLinesWithNotExistingFile()
    {
        var path = "I'm not exist.csv";
        Assert.Throws<FileNotFoundException>(() => loader.ReadAllLines(path));
    }

    #endregion ReadAllLines Tests

    #region ReadAllLinesAsync Tests

    [Fact]
    public void ReadAllLinesAsyncPathNull()
    {
        string path = null;
        Assert.Throws<ArgumentException>(() => loader.ReadAllLinesAsync(path).GetAwaiter().GetResult());
    }

    [Fact]
    public void ReadAllLinesAsyncWithNotExistingFile()
    {
        string path = "I'm not exist.csv";
        Assert.Throws<FileNotFoundException>(() => loader.ReadAllLinesAsync(path).GetAwaiter().GetResult());
    }

    #endregion ReadAllLinesAsync Tests
}