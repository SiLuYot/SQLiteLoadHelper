using Mono.Data.Sqlite;
using System;
using System.Data.SQLite;
using System.Reflection;
using System.Text;

namespace SQLiteLoadHelper.Data
{
    public abstract class ReadData : IReadData
    {
        public abstract int ID { get; }
        public abstract string NAME { get; }

        /// <summary>
        /// DB 컬럼 이름으로 T 타입의 필드이름을 찾아서 읽어온다. using System.Data.SQLite;
        /// </summary>
        public T GetReadData<T>(SQLiteDataReader rdr, StringBuilder sb) where T : new()
        {
            T newData = new T();

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string rdrName = rdr.GetName(i);    //컬럼 이름
                object rdrValue = rdr.GetValue(i);  //해당 데이터

                FieldInfo field = newData.GetType().GetField(rdrName);
                if (field != null)
                {
                    Type fieldType = field.FieldType;
                    field.SetValue(newData, Convert.ChangeType(rdrValue, fieldType));
                }
                else
                {
                    string msg = string.Format("[DB READ ERROR]\n{0} is not have {1}", typeof(T).Name, rdrName);
                    if (!sb.ToString().Contains(msg))
                    {
                        sb.AppendLine(msg);
                    }
                }
            }
            return newData;
        }

        /// <summary>
        /// DB 컬럼 이름으로 T 타입의 필드이름을 찾아서 읽어온다. using System.Data.SQLite;
        /// </summary>
        public T GetReadData<T>(SqliteDataReader rdr, StringBuilder sb) where T : new()
        {
            T newData = new T();

            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string rdrName = rdr.GetName(i);    //컬럼 이름
                object rdrValue = rdr.GetValue(i);  //해당 데이터

                FieldInfo field = newData.GetType().GetField(rdrName);
                if (field != null)
                {
                    Type fieldType = field.FieldType;
                    field.SetValue(newData, Convert.ChangeType(rdrValue, fieldType));
                }
                else
                {
                    string msg = string.Format("[DB READ ERROR]\n{0} is not have {1}", typeof(T).Name, rdrName);
                    if (!sb.ToString().Contains(msg))
                    {
                        sb.AppendLine(msg);
                    }
                }
            }
            return newData;
        }
    }
}
