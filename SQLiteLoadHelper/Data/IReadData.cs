using Mono.Data.Sqlite;
using System.Data.SQLite;
using System.Text;

namespace SQLiteLoadHelper.Data
{
    public interface IReadData : ICustomData
    {
        //using System.Data.SQLite;
        T GetReadData<T>(SQLiteDataReader rdr, StringBuilder sb) where T : new();

        //using Mono.Data.Sqlite;
        T GetReadData<T>(SqliteDataReader rdr, StringBuilder sb) where T : new();
    }
}
