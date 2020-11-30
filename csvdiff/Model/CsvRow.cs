using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace csvdiff.Model
{
    public class CsvRow : IEquatable<CsvRow>
    {
        public static readonly CsvRow Empty = new CsvRow(new string[] { }, -1);
        public int Number { get; }
        public ReadOnlyCollection<string> Cells { get; }

        public CsvRow(string[]? cells, int number)
        {
            Cells = cells is null ? new ReadOnlyCollection<string>(new List<string>()) : Array.AsReadOnly(cells);
            Number = number;
        }

        public static bool operator ==(CsvRow? left, CsvRow? right)
        {
            if (left is null)
            {
                return right is null ? true : false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(CsvRow? left, CsvRow? right)
        {
            if (left is null)
            {
                return right is null ? false : true;
            }
            return !left.Equals(right);
        }

        public bool Equals(CsvRow? other)
        {
            if (other is null)
            {
                return false;
            }

            if (Cells.Count != other.Cells.Count)
            {
                return false;
            }

            if (GetHashCode() == other.GetHashCode())
            {
                for (int i = 0; i < Cells.Count; i++)
                {
                    if (Cells[i] != other.Cells[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override bool Equals(object? obj)
        {
            return obj is CsvRow row && Equals(row);
        }

        public override int GetHashCode()
        {
            int hash = 352033288;
            for (int i = 0; i < Cells.Count; i++)
            {
                hash ^= hash * (Cells[i] is null ? -1521134295 : Cells[i].GetHashCode());
            }

            return hash;
        }

        public override string ToString()
        {
            string toStrResult = string.Empty;
            if (Cells is null || Cells.Count == 0)
            {
                return $"{Number}| ";
            }
            StringBuilder builder = new StringBuilder($"{Number}| ", 15 * Cells.Count);

            foreach (string str in Cells)
            {
                builder.Append($"{str}|");
            }

            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
    }
}