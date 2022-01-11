using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class SqliteCrud
    {
        private readonly string _connectionString;
        private SqliteDataAccess db = new();

        public SqliteCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TextItemModel> GetAllTextItems()
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
