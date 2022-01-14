using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLibrary
{
    public class SqliteCrud : ISqlCrud
    {
        private readonly string _connectionString;
        private ISqliteDataAccess _db;

        public SqliteCrud(string connectionString, ISqliteDataAccess db)
        {
            _connectionString = connectionString;
            _db = db;
        }

        public void CreateTextItem(TextItemModel textItem)
        {
            string sql = "select count(1) from TextItems;";
            int numberOfRows = _db.LoadData<int, dynamic>(sql, new { }, _connectionString).First();
            
            sql = "insert into TextItems (Description, Text, SortPosition) values (@Description, @Text, @NumberOfRows);";
            _db.SaveData(sql, new { textItem.Description, textItem.Text, NumberOfRows = numberOfRows }, _connectionString);
        }

        public List<TextItemModel> ReadAllTextItemsOrderedBySortPosition()
        {
            string sql = "select Id, Description, Text, SortPosition from TextItems order by SortPosition;";

            return _db.LoadData<TextItemModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<TextItemModel> ReadAllTextItemsOrderedByDescription()
        {
            string sql = "select Id, Description, Text, SortPosition from TextItems order by Description collate nocase;";

            return _db.LoadData<TextItemModel, dynamic>(sql, new { }, _connectionString);
        }

        public void UpdateTextItem(TextItemModel textItem)
        {
            string sql = "update TextItems set Description = @Description, Text = @Text, SortPosition = @SortPosition " +
                "where Id = @Id;";
            _db.SaveData(sql, textItem, _connectionString);
        }

        public void DeleteTextItem(int textItemId)
        {
            string sql = "delete from TextItems where Id = @Id;";
            _db.SaveData(sql, new { Id = textItemId }, _connectionString);
        }
    }
}
