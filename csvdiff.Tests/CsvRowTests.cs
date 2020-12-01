using csvdiff.Model;
using System;
using System.Linq;
using Xunit;

namespace csvdiff.Tests
{
    public class CsvRowTests
    {
        #region Construction Tests

        [Fact]
        public void Ctor()
        {
            var cells = new string[] { "Column1", "Column2", "Column3" };
            var row = new CsvRow(cells, 1);
            Assert.True(cells.SequenceEqual(row.Cells));
            Assert.Equal(1, row.Number);
        }

        [Fact]
        public void CtorPassNull()
        {
            var row = new CsvRow(null, 1);
            Assert.True(row.Cells.Count == 0);
        }

        #endregion Construction Tests

        #region Equals Tests

        [Theory]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, new string[] { "Column1", "Column2", "Column3" }, true)]
        [InlineData(new string[] { "Column1", "DIFF", "Column3" }, new string[] { "Column1", "Column2", "Column3" }, false)]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, null, false)]
        [InlineData(null, new string[] { "Column1", "Column2", "Column3" }, false)]
        [InlineData(null, null, true)]
        [InlineData(new string[] { }, new string[] { }, true)]
        [InlineData(new string[] { }, null, true)]
        public void EqualsTheory(string[] leftCells, string[] rightCells, bool expected)
        {
            var left = new CsvRow(leftCells, 1);
            var right = new CsvRow(rightCells, 1);

            var result = left.Equals(right);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CompareWithNull()
        {
            var cells = new string[] { "Column1", "Column2", "Column3" };
            var left = new CsvRow(cells, 1);

            var result = left.Equals(null);

            Assert.False(result);
        }

        #endregion Equals Tests

        #region Overloaded Operators Tests

        [Theory]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, new string[] { "Column1", "Column2", "Column3" }, true)]
        [InlineData(new string[] { "Column1", "DIFF", "Column3" }, new string[] { "Column1", "Column2", "Column3" }, false)]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, null, false)]
        [InlineData(null, new string[] { "Column1", "Column2", "Column3" }, false)]
        [InlineData(null, null, true)]
        [InlineData(new string[] { }, new string[] { }, true)]
        [InlineData(new string[] { }, null, true)]
        public void EqualityOperatorTheory(string[] leftCells, string[] rightCells, bool expected)
        {
            var left = new CsvRow(leftCells, 1);
            var right = new CsvRow(rightCells, 1);

            var result = left == right;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void EqualityOperatorWhenLeftIsNull()
        {
            var cells = new string[] { "Column1", "Column2", "Column3" };
            CsvRow left = null;
            var right = new CsvRow(cells, 1);

            var result = left == right;

            Assert.False(result);
        }

        [Fact]
        public void EqualityOperatorWhenRightIsNull()
        {
            var cells = new string[] { "Column1", "Column2", "Column3" };
            var left = new CsvRow(cells, 1);
            CsvRow right = null;

            var result = left == right;

            Assert.False(result);
        }

        [Theory]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, new string[] { "Column1", "Column2", "Column3" }, false)]
        [InlineData(new string[] { "Column1", "DIFF", "Column3" }, new string[] { "Column1", "Column2", "Column3" }, true)]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, null, true)]
        [InlineData(null, new string[] { "Column1", "Column2", "Column3" }, true)]
        [InlineData(null, null, false)]
        [InlineData(new string[] { }, new string[] { }, false)]
        [InlineData(new string[] { }, null, false)]
        public void NonEqualityOperatorTheory(string[] leftCells, string[] rightCells, bool expected)
        {
            var left = new CsvRow(leftCells, 1);
            var right = new CsvRow(rightCells, 1);

            var result = left != right;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void NonEqualityOperatorWhenLeftIsNull()
        {
            var cells = new string[] { "Column1", "Column2", "Column3" };
            CsvRow left = null;
            var right = new CsvRow(cells, 1);

            var result = left != right;

            Assert.True(result);
        }

        [Fact]
        public void NonEqualityOperatorWhenRightIsNull()
        {
            var cells = new string[] { "Column1", "Column2", "Column3" };
            var left = new CsvRow(cells, 1);
            CsvRow right = null;

            var result = left != right;

            Assert.True(result);
        }

        #endregion Overloaded Operators Tests

        #region ToString Tests

        [Theory]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, "1| Column1|Column2|Column3")]
        [InlineData(new string[] { "Column1", "", "Column3" }, "1| Column1||Column3")]
        [InlineData(new string[] { "SHA", " Kappa    " }, "1| SHA| Kappa    ")]
        [InlineData(new string[] { }, "1| ")]
        [InlineData(null, "1| ")]
        public void ToStringTheory(string[] cells, string expected)
        {
            var row = new CsvRow(cells, 1);
            var result = row.ToString();
            Assert.Equal(expected, result);
        }

        #endregion ToString Tests

        #region IFormattable Implementation

        [Theory]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, "c|c", "Column1|Column2|Column3")]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, "C|c", "Column1|Column2|Column3")]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, "G", "Column1|Column2|Column3")]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, "g", "Column1|Column2|Column3")]
        [InlineData(new string[] { "Column1", "", "Column3" }, "c,C", "Column1,,Column3")]
        [InlineData(new string[] { "SHA", " Kappa    " }, "C c", "SHA  Kappa    ")]
        [InlineData(new string[] { "Column1", "Column2", "Column3" }, null, "Column1|Column2|Column3")]
        [InlineData(new string[] { }, null, "")]
        [InlineData(null, "c|c", "")]
        public void FormattableToStringTheory(string[] cells, string format, string expected)
        {
            var row = new CsvRow(cells, 1);
            var result = row.ToString(format);
            Assert.Equal(expected, result);
        }

        #endregion IFormattable Implementation
    }
}