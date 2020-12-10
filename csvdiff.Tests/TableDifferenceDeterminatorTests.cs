using System;
using Xunit;
using csvdiff;
using csvdiff.Model;
using Moq;
using csvdiff.Parsers;
using System.Linq;
using System.Collections.Generic;

public class TableDifferenceDeterminatorTests
{
    private TableDifferenceDeterminator _determinator;

    private CsvTable _defaultTable;
    private Mock<ITableRowsReader> _readerMock;
    private Mock<ICsvCellsParser> _parserMock;

    public TableDifferenceDeterminatorTests()
    {
        _determinator = new TableDifferenceDeterminator();

        _parserMock = new Mock<ICsvCellsParser>();
        _parserMock.Setup(parser => parser.ParseCells(It.IsAny<string>())).Returns(new string[] { "Cell1", "Cell2", "Cell3" });
        _readerMock = new Mock<ITableRowsReader>();
        _readerMock.Setup(reader => reader.ReadAllLines(It.IsAny<string>())).Returns(Enumerable.Repeat(string.Empty, 3).ToArray());

        _defaultTable = new CsvTable("Whatever", _parserMock.Object, _readerMock.Object);
    }

    [Fact]
    public void CompareEqualTables()
    {
        var expected = Enumerable.Empty<(CsvRow, CsvRow)>();
        var result = _determinator.GetDifferences(_defaultTable, _defaultTable);
        Assert.True(expected.SequenceEqual(result));
    }

    [Fact]
    public void CompareNonEqualTables()
    {
        var parserMock = new Mock<ICsvCellsParser>();
        parserMock.Setup(parser => parser.ParseCells(It.IsAny<string>())).Returns(new string[] { "diffCell1", "diffCell2", "diffCell3" });
        var otherTable = new CsvTable("Whatever", parserMock.Object, _readerMock.Object);

        var expected = new List<(CsvRow, CsvRow)>();
        for (int i = 0; i < _defaultTable.Rows.Count; i++)
        {
            expected.Add((_defaultTable.Rows[i], otherTable.Rows[i]));
        }

        var result = _determinator.GetDifferences(_defaultTable, otherTable);

        Assert.True(expected.SequenceEqual(result));
    }

    [Fact]
    public void DifferenceForPartlyEqualTables()
    {
        var parserMock = new Mock<ICsvCellsParser>();
        parserMock.SetupSequence(parser => parser.ParseCells(It.IsAny<string>())).Returns(new string[] { "Cell1", "Cell2", "Cell3" })
                                                                                 .Returns(new string[] { "diffCell1", "Cell2", "Cell3" })
                                                                                 .Returns(new string[] { "Cell1", "Cell2", "Cell3" });
        var otherTable = new CsvTable("Whatever", parserMock.Object, _readerMock.Object);
        var expected = otherTable.Rows.Except(_defaultTable.Rows).Select(row => (_defaultTable.Rows.First(), row)).ToList();

        var result = _determinator.GetDifferences(_defaultTable, otherTable);

        Assert.True(expected.SequenceEqual(result));
    }

    [Fact]
    public void DifferenceForNotEquallySizedTables()
    {
        var readerMock = new Mock<ITableRowsReader>();
        readerMock.Setup(reader => reader.ReadAllLines(It.IsAny<string>())).Returns(Enumerable.Repeat(string.Empty, 4).ToArray());
        var otherTable = new CsvTable("Whatever", _parserMock.Object, readerMock.Object);
        var expected = otherTable.Rows.Skip(_defaultTable.Rows.Count).Select(row => (CsvRow.Empty, row)).ToList();

        var result = _determinator.GetDifferences(_defaultTable, otherTable);

        Assert.True(expected.SequenceEqual(result));
    }

    [Fact]
    public void DifferenceForPartlyEqualAndNotEquallySizedTables()
    {
        var parserMock = new Mock<ICsvCellsParser>();
        parserMock.SetupSequence(parser => parser.ParseCells(It.IsAny<string>())).Returns(new string[] { "Cell1", "Cell2", "Cell3" })
                                                                                 .Returns(new string[] { "diffCell1", "Cell2", "Cell3" })
                                                                                 .Returns(new string[] { "Cell1", "Cell2", "Cell3" })
                                                                                 .Returns(new string[] { "Cell1", "Cell2", "Cell3" })
                                                                                 .Throws(new NotImplementedException());
        var readerMock = new Mock<ITableRowsReader>();
        readerMock.Setup(reader => reader.ReadAllLines(It.IsAny<string>())).Returns(Enumerable.Repeat(string.Empty, 4).ToArray());
        var otherTable = new CsvTable("Whatever", parserMock.Object, readerMock.Object);
        var expected = new List<(CsvRow, CsvRow)>() { (new CsvRow(new string[] { "Cell1", "Cell2", "Cell3" }, 2), new CsvRow(new string[] { "diffCell1", "Cell2", "Cell3" }, 2)),
                                                      (CsvRow.Empty, new CsvRow(new string[] { "Cell1", "Cell2", "Cell3" }, 4))};

        var result = _determinator.GetDifferences(_defaultTable, otherTable);

        Assert.True(expected.SequenceEqual(result));

    }
}