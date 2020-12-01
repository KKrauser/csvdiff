namespace csvdiff
{
    public interface ITableRowsReader
    {
        string[] ReadAllLines(string csvFilePath);
    }
}