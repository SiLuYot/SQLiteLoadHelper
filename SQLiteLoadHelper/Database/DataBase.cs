using Mono.Data.Sqlite;
using SQLiteLoadHelper.Data;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace SQLiteLoadHelper.Database
{
    public abstract class DataBase<T> : IDataBase where T : ICustomData, new()
    {
        //읽어온 모든 데이터들의 리스트
        private List<ICustomData> dataList = new List<ICustomData>();
        //에러가 발생하면 메세지가 추가된다.
        protected StringBuilder errorMsgSB = new StringBuilder();

        //DB를 구분할 키 (주로 이름으로 구분)
        public abstract string DBKey { get; }

        //에러가 없다면 메세지도 없다
        public string GetErrorMsg() => errorMsgSB?.ToString();
        //이 데이터베이스가 가진 데이터(클래스)이름 반환
        public string GetDataName() => typeof(T).Name;

        public object CreateNewData() => new T();

        public List<ICustomData> GetDataList() => dataList;

        //데이터를 가져온다. 타입이 맞지 않다면 새로 만들어서 리턴
        public virtual ICustomData GetData(object obj)
        {
            var findData = GetDataList().Find(v =>
            {
                var convertObj = Convert.ChangeType(obj, v.ID.GetType());
                return convertObj.Equals(v.ID);
            });

            if (findData == null)
            {
                findData = CreateNewData() as ICustomData;
            }
            return findData;
        }

        //DB에서 읽어온 데이터를 리스트에 추가 -> DataBase 생성 완료! using System.Data.SQLite;
        public virtual void AddData(SQLiteDataReader rdr)
        {
            IReadData data = CreateNewData() as IReadData;
            T newData = data.GetReadData<T>(rdr, errorMsgSB);

            dataList.Add(newData);
        }
        //DB에서 읽어온 데이터를 리스트에 추가 -> DataBase 생성 완료! using Mono.Data.Sqlite;
        public virtual void AddData(SqliteDataReader rdr)
        {
            IReadData data = CreateNewData() as IReadData;
            T newData = data.GetReadData<T>(rdr, errorMsgSB);

            dataList.Add(newData);
        }
        //읽어온 데이터로 추가 작업이 필요한 경우 -> DataBase 추가 완료!
        public virtual bool ReadData() { return true; }
        //그 추가 작업이 다른 디비의 참조가 필요한 경우
        public virtual bool CombinedData() { return true; }
    }
}
