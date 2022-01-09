using DataAccessLibrary.Models;
using System.Collections.Generic;

namespace DataAccessLibrary
{
    public interface IDataAccess
    {
        List<TextItemModel> ReadAllRecords(string filePath);
        void WriteAllRecords(IEnumerable<TextItemModel> textItems, string filePath);
    }
}