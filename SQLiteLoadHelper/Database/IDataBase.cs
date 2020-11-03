using Mono.Data.Sqlite;
using SQLiteLoadHelper.Data;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SQLiteLoadHelper.Database
{
    public interface IDataBase
    {
        string DBKey { get; }

        string GetErrorMsg();
        string GetDataName();

        object CreateNewData();

        List<ICustomData> GetDataList();
        ICustomData GetData(object obj);

        //using System.Data.SQLite;
        void AddData(SQLiteDataReader rdr);
        //using Mono.Data.Sqlite;
        void AddData(SqliteDataReader rdr);

        bool ReadData();
        bool CombinedData();
    }
}
