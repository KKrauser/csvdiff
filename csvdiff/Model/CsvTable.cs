using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using csvdiff.Parsers;

namespace csvdiff.Model
{
    public class CsvTable : IEquatable<CsvTable>
    {
        private ICsvCellsParser _parser;
        private ITableRowsReader _reader;
        private string _file;
        private ReadOnlyCollection<CsvRow>? _rows;

        public ReadOnlyCollection<CsvRow> Rows => _rows ??= Array.AsReadOnly(LoadTable());

        public CsvTable(string csvFilePath) : this(csvFilePath, new CellsParser())
        { }

        public CsvTable(string csvFilePath, ICsvCellsParser? parser) : this(csvFilePath, parser, new TableRowsReader())
        { }

        public CsvTable(string csvFilePath, ICsvCellsParser? parser, ITableRowsReader? reader)
        {
            _file = csvFilePath;
            _parser = parser ?? new CellsParser();
            _reader = reader ?? new TableRowsReader();
        }

        private CsvRow[] LoadTable()
        {
            var lines = _reader.ReadAllLines(_file);
            var csvList = new List<CsvRow>(lines.Length);

            int i = 1;
            foreach (string line in lines)
            {
                var cells = _parser.ParseCells(line);
                csvList.Add(new CsvRow(cells, i++));
            }

            return csvList.ToArray();
        }

        public bool Equals(CsvTable? other)
        {
            if (other is null)
            {
                return false;
            }

            if (GetHashCode() == other.GetHashCode())
            {
                if (Rows.Count != other.Rows.Count)
                {
                    return false;
                }

                if (GetHashCode() == other.GetHashCode())
                {
                    for (int i = 0; i < Rows.Count; i++)
                    {
                        if (Rows[i] != other.Rows[i])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public static bool operator ==(CsvTable? left, CsvTable? right)
        {
            if (left is null)
            {
                return right is null ? true : false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(CsvTable left, CsvTable right)
        {
            if (left is null)
            {
                return right is null ? false : true;
            }
            return !left.Equals(right);
        }

        public override bool Equals(object? obj)
        {
            return obj is CsvTable table && Equals(table);
        }

        public override int GetHashCode()
        {
            int hash = 861097806;
            for (int i = 0; i < Rows.Count; i++)
            {
                hash ^= hash * (Rows[i] is null ? -1452011 : Rows[i].GetHashCode());
            }
            return hash;
        }
    }
}