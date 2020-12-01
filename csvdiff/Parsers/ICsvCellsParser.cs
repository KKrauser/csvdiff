namespace csvdiff.Parsers
{
    public interface ICsvCellsParser
    {
        string[] ParseCells(string row);
    }
}