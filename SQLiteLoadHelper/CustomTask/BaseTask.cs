using SQLiteLoadHelper.Database;
using System;

namespace SQLiteLoadHelper.CustomTask
{
    //작업의 한 단위
    public abstract class BaseTask
    {
        public abstract string GetName(object data);
        public abstract bool Execute(object data);
    }

    //DB 로드 (1. 맨 처음 로드할 DB들을 로드한다.)
    public class LoadDatabase : BaseTask
    {
        private Func<IDataBase, bool> dataLoadAction;

        public LoadDatabase(Func<IDataBase, bool> dataLoadAction)
        {
            this.dataLoadAction = dataLoadAction;
        }

        public override string GetName(object data)
        {
            if (data == null) return "-null data-";
            else return (data as IDataBase).GetDataName();
        }

        public override bool Execute(object data)
        {
            if (data == null)
                return false;

            IDataBase dataBase = data as IDataBase;
            return dataLoadAction.Invoke(dataBase);
        }
    }

    //DB 리드 (2. 로드된 데이터로 추가 작업이 필요한 경우)
    public class ReadDataBase : BaseTask
    {
        public override string GetName(object data)
        {
            if (data == null) return "-null data-";
            else return (data as IDataBase).GetDataName();
        }

        public override bool Execute(object data)
        {
            if (data == null)
                return false;

            IDataBase dataBase = data as IDataBase;
            return dataBase.ReadData();
        }
    }

    //DB 합성 (3. 그 추가 작업이 다른 DB의 참조가 필요한 경우)
    public class CombineDataBase : BaseTask
    {
        public override string GetName(object data)
        {
            if (data == null) return "-null data-";
            else return (data as IDataBase).GetDataName();
        }

        public override bool Execute(object data)
        {
            if (data == null)
                return false;

            IDataBase dataBase = data as IDataBase;
            return dataBase.CombinedData();
        }
    }
}
