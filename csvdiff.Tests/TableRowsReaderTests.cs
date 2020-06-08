using System;
using System.IO;
using Xunit;

namespace csvdiff.Tests
{
    public class TableRowsReaderTests
    {
        private TableRowsReader _loader = new TableRowsReader();

        #region ReadAllLines Tests

        [Fact]
        public void ReadAllLinesPathNull()
        {
            string path = null;
            Assert.Throws<ArgumentException>(() => _loader.ReadAllLines(path));
        }

        [Fact]
        public void ReadAllLinesWithNotExistingFile()
        {
            var path = "I'm not exist.csv";
            Assert.Throws<FileNotFoundException>(() => _loader.ReadAllLines(path));
        }

        #endregion ReadAllLines Tests

        #region ReadAllLinesAsync Tests

        [Fact]
        public void ReadAllLinesAsyncPathNull()
        {
            string path = null;
            Assert.Throws<ArgumentException>(() => _loader.ReadAllLinesAsync(path).GetAwaiter().GetResult());
        }

        [Fact]
        public void ReadAllLinesAsyncWithNotExistingFile()
        {
            string path = "I'm not exist.csv";
            Assert.Throws<FileNotFoundException>(() => _loader.ReadAllLinesAsync(path).GetAwaiter().GetResult());
        }

        #endregion ReadAllLinesAsync Tests
    }
}