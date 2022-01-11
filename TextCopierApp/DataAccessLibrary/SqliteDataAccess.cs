using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SQLite;
using Dapper;
using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public class SqliteDataAccess : IDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }

        public List<TextItemModel> ReadAllRecords(string filePath)
        {
            throw new System.NotImplementedException();
        }

        public void WriteAllRecords(IEnumerable<TextItemModel> textItems, string filePath)
        {
            throw new System.NotImplementedException();
        }
    }
}
