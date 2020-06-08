﻿using csvdiff.Model;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace csvdiff.Tests
{
    public class CsvTableTests
    {
        private string _pathToDefaultTable = "Whatever";
        private Mock<IRowParser> _defaultParserMock;
        private Mock<ITableRowsReader> _defaultRowsReaderMock;
        private CsvTable _defaultTable;

        public CsvTableTests()
        {
            _defaultParserMock = new Mock<IRowParser>();
            _defaultParserMock.Setup(p => p.ParseRow(string.Empty)).Returns(Enumerable.Repeat("Cell", 3).ToArray());

            _defaultRowsReaderMock = new Mock<ITableRowsReader>();
            _defaultRowsReaderMock.Setup(r => r.ReadAllLines(_pathToDefaultTable)).Returns(Enumerable.Repeat(string.Empty, 3).ToArray());

            _defaultTable = new CsvTable(_pathToDefaultTable, _defaultParserMock.Object, _defaultRowsReaderMock.Object);
        }

        #region Equals Tests

        [Fact]
        public void EqualsForEqualTables()
        {
            var table2 = new CsvTable(_pathToDefaultTable, _defaultParserMock.Object, _defaultRowsReaderMock.Object);

            var result = _defaultTable.Equals(table2);

            Assert.True(result);
            //As we use single mock instances for both tables - count of calls
            //will be doubled.
            _defaultParserMock.Verify(p => p.ParseRow(string.Empty), Times.Exactly(6)); //3 for each table
            _defaultRowsReaderMock.Verify(r => r.ReadAllLines(_pathToDefaultTable), Times.Exactly(2)); //1 for each table
        }

        [Fact]
        public void EqualsForNonEqualTables()
        {
            var table2ParserMock = new Mock<IRowParser>();
            table2ParserMock.Setup(p => p.ParseRow(It.IsAny<string>())).Returns(Enumerable.Repeat("AnotherCell", 3).ToArray());

            var table2 = new CsvTable(_pathToDefaultTable, table2ParserMock.Object, _defaultRowsReaderMock.Object);

            var result = _defaultTable.Equals(table2);

            Assert.False(result);
            _defaultParserMock.Verify(p => p.ParseRow(string.Empty), Times.Exactly(3));
            table2ParserMock.Verify(p => p.ParseRow(string.Empty), Times.Exactly(3));
            _defaultRowsReaderMock.Verify(r => r.ReadAllLines(_pathToDefaultTable), Times.Exactly(2)); //1 for each table
        }

        [Fact]
        public void EqualsWhenOtherTableIsNull()
        {
            CsvTable table2 = null;

            var result = _defaultTable.Equals(table2);

            Assert.False(result);
            _defaultParserMock.Verify(p => p.ParseRow(string.Empty), Times.Exactly(3));
            _defaultRowsReaderMock.Verify(r => r.ReadAllLines(_pathToDefaultTable), Times.Exactly(1));
        }

        [Fact]
        public void EqualsForTablesWithDifferentRowLengths()
        {
            var table2ParserMock = new Mock<IRowParser>();
            table2ParserMock.Setup(p => p.ParseRow(It.IsAny<string>())).Returns(Enumerable.Repeat("Cell", 2).ToArray());
            var table2 = new CsvTable(_pathToDefaultTable, table2ParserMock.Object, _defaultRowsReaderMock.Object);

            var result = _defaultTable.Equals(table2);

            Assert.False(result);
            _defaultParserMock.Verify(p => p.ParseRow(string.Empty), Times.Exactly(3));
            table2ParserMock.Verify(p => p.ParseRow(string.Empty), Times.Exactly(3));
            _defaultRowsReaderMock.Verify(r => r.ReadAllLines(_pathToDefaultTable), Times.Exactly(2)); //1 for each table
        }

        #endregion Equals Tests

        #region Constructor Tests

        [Fact]
        public void ThreeParameterCtor()
        {
            var expected = new CsvRow[]
            {
            new CsvRow(Enumerable.Repeat("Cell", 3).ToArray(), 1),
            new CsvRow(Enumerable.Repeat("Cell", 3).ToArray(), 2),
            new CsvRow(Enumerable.Repeat("Cell", 3).ToArray(), 3)
            };

            Assert.True(expected.SequenceEqual(_defaultTable.Rows));
            _defaultRowsReaderMock.Verify();
            _defaultParserMock.Verify();
        }

        #endregion Constructor Tests

        #region GetHashCode Tests

        [Fact]
        public void GetHashCodeOfTwoEqualsTables()
        {
            var table2 = new CsvTable(_pathToDefaultTable, _defaultParserMock.Object, _defaultRowsReaderMock.Object);

            Assert.True(_defaultTable.GetHashCode() == table2.GetHashCode());
        }

        [Fact]
        public void GetHashCodeOfTwoDifferentTables()
        {
            var table2ParserMock = new Mock<IRowParser>();
            table2ParserMock.Setup(p => p.ParseRow(It.IsAny<string>())).Returns(Enumerable.Repeat("AnotherCell", 3).ToArray());
            var table2 = new CsvTable(_pathToDefaultTable, table2ParserMock.Object, _defaultRowsReaderMock.Object);

            Assert.False(_defaultTable.GetHashCode() == table2.GetHashCode());
        }

        #endregion GetHashCode Tests
    }
}