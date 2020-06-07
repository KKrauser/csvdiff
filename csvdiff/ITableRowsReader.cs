using System.Threading.Tasks;

namespace csvdiff
{
    public interface ITableRowsReader
    {
        string[] ReadAllLines(string csvFilePath);

        Task<string[]> ReadAllLinesAsync(string csvFilePath);
    }
}