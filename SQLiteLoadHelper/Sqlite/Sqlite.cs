using Mono.Data.Sqlite;
using SQLiteLoadHelper.Database;
using System;
using System.Data.SQLite;
using System.Text;

namespace SQLiteLoadHelper.Sqlite
{
    public interface ISQLIte
    {
        string ErrorMsg { get; }
        void Load(IDataBase dataBase, string dbPath, string dbKey);
    }

    //using Mono.Data.Sqlite;
    public class MonoSQLite : ISQLIte
    {
        private StringBuilder sb = new StringBuilder();
        public string ErrorMsg => sb.ToString();

        public void Load(IDataBase dataBase, string dbPath, string dbKey)
        {
            try
            {
                var conn = new SqliteConnection("Data Source=" + dbPath + ";Version=3;");
                conn.Open();

                string query = "SELECT * FROM " + dbKey;

                SqliteCommand cmd = new SqliteCommand(query, conn);
                SqliteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    dataBase.AddData(rdr);
                }

                rdr.Close();
            }
            catch (Exception e)
            {
                sb.AppendFormat("DB Key : {0}\nPath : {1}\n{2}\n", dbKey, dbPath, e.Message);
            }
        }
    }

    //using System.Data.SQLite;
    public class SystemSQLite : ISQLIte
    {
        private StringBuilder sb = new StringBuilder();
        public string ErrorMsg => sb.ToString();

        public void Load(IDataBase dataBase, string dbPath, string dbKey)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection("Data Source=" + dbPath + ";Version=3;");
                conn.Open();

                string query = "SELECT * FROM " + dbKey;

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    dataBase.AddData(rdr);
                }

                rdr.Close();
            }
            catch (Exception e)
            {
                sb.AppendFormat("DB Key : {0}\nPath : {1}\n{2}\n", dbKey, dbPath, e.Message);
            }
        }
    }
}
