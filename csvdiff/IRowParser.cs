namespace csvdiff
{
    public interface IRowParser
    {
        string[] ParseRow(string row);
    }
}