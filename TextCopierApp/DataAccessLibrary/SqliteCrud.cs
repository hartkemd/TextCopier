using DataAccessLibrary.Models;
using System.Collections.Generic;

namespace DataAccessLibrary
{
    public class SqliteCrud : ISqlCrud
    {
        private readonly string _connectionString;
        private ISqliteDataAccess db;

        public SqliteCrud(string connectionString, ISqliteDataAccess db)
        {
            _connectionString = connectionString;
            this.db = db;
        }

        public List<TextItemModel> ReadAllTextItems()
        {
            string sql = "select Id, Description, Text from TextItems;";

            return db.LoadData<TextItemModel, dynamic>(sql, new { }, _connectionString);
        }

        public void CreateTextItem(TextItemModel textItem)
        {
            string sql = "insert into TextItems (Description, Text) values (@Description, @Text);";
            db.SaveData(sql, new { textItem.Description, textItem.Text }, _connectionString);
        }

        public void UpdateTextItem(TextItemModel textItem)
        {
            string sql = "update TextItems set Description = @Description, Text = @Text where Id = @Id;";
            db.SaveData(sql, textItem, _connectionString);
        }

        public void DeleteTextItem(int textItemId)
        {
            string sql = "delete from TextItems where Id = @Id;";
            db.SaveData(sql, new { Id = textItemId }, _connectionString);
        }
    }
}
