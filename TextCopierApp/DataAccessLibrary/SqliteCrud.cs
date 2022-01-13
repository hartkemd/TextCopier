using DataAccessLibrary.Models;
using System.Collections.Generic;

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

        public List<TextItemModel> ReadAllTextItems()
        {
            string sql = "select Id, Description, Text, SortPosition from TextItems order by SortPosition;";

            return _db.LoadData<TextItemModel, dynamic>(sql, new { }, _connectionString);
        }

        public void CreateTextItem(TextItemModel textItem)
        {
            string sql = "insert into TextItems (Description, Text) values (@Description, @Text);";
            _db.SaveData(sql, new { textItem.Description, textItem.Text }, _connectionString);

            sql = "update TextItems set SortPosition = (select count(*) from TextItems) where Id = @Id;";
            _db.SaveData(sql, new { textItem.Id }, _connectionString);
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
