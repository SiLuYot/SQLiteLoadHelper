using SQLiteLoadHelper.CustomTask;
using SQLiteLoadHelper.Data;
using SQLiteLoadHelper.Database;
using SQLiteLoadHelper.Sqlite;
using SQLiteLoadHelper.Version;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SQLiteLoadHelper
{
    public class SQLiteLoadHelperClass
    {
        //에러가 발생한 경우 메세지가 담겨있음
        public string ErrorMsg
        {
            get
            {
                string msg = sb?.ToString();
                sb.Clear();

                return msg;
            }
        }

        //기본 PATH
        public string BasePath
        {
            get
            {
                return projectName + "_data";
            }
        }

        //경로에 사용할 프로젝트 이름
        private string projectName;
        //사용할 SQLite (System.Data.SQLite / Mono.Data.Sqlite)
        private ISQLIte usingSqlite;

        //에러 메세지를 담을 sb
        private StringBuilder sb;
        //DB Key로 데이터베이스를 저장하는 Dic
        private Dictionary<string, IDataBase> databaseDIc;
        //작업 관리자
        private TaskManager taskManager;

        //Path에 사용할 projectName / usingSqlite -> (System.Data.SQLite / Mono.Data.Sqlite)
        public SQLiteLoadHelperClass(string projectName, ISQLIte usingSqlite)
        {
            this.projectName = projectName;
            this.usingSqlite = usingSqlite;

            sb = new StringBuilder();
            databaseDIc = new Dictionary<string, IDataBase>();
            taskManager = new TaskManager();
        }

        //작업 관리자에 이벤트 등록
        //1. 작업 진행 2. 작업중인 데이터 변경 3. 작업 종료
        public void AddTaskEvent(Action<double> taskProgressValue, Action<string> taskProgressDataName, Action<BaseTask> taskComplete)
        {
            taskManager.AddTaskEvent(taskProgressValue, taskProgressDataName, taskComplete);
        }

        //Key에 해당하는 데이터베이스 반환 (key : 주로 파일 이름)
        public IDataBase GetDataBase(string key)
        {
            if (!databaseDIc.ContainsKey(key)) return null;
            return databaseDIc[key];
        }

        //Key에 해당하는 데이터리스트 반환 (key : 주로 파일 이름)
        public List<ICustomData> GetDataList(string key)
        {
            if (!databaseDIc.ContainsKey(key)) return null;
            return databaseDIc[key].GetDataList();
        }

        /// <summary>
        /// Load -> Read -> Combined
        /// </summary>
        public bool Process(IDataBase[] database)
        {
            return LoadDataBase(database);
        }

        private bool LoadDataBase(IDataBase[] database)
        {
            bool isComplete = taskManager.Start(database, new LoadDatabase(LoadDataBaseFile));

            if (isComplete)
            {
                //데이터 로드 끝
                foreach (var db in database)
                {
                    string msg = db.GetErrorMsg();

                    if (msg != string.Empty)
                        sb.Append(msg);
                }
                return ReadDataBase(database);
            }
            else return false;
        }

        private bool ReadDataBase(IDataBase[] database)
        {
            bool isComplete = taskManager.Start(database, new ReadDataBase());

            if (isComplete)
            {
                //데이터 리드 끝
                foreach (var db in database)
                {
                    databaseDIc.Add(db.DBKey, db);
                }
                return CombinedDataBase(database);
            }
            else return false;
        }

        private bool CombinedDataBase(IDataBase[] database)
        {
            bool isComplete = taskManager.Start(database, new CombineDataBase());

            //데이터 합성 끝
            if (isComplete)
            {
                return true;
            }
            else return false;
        }

        //해당 데이터베이스를 로드한다.
        public bool LoadDataBaseFile(IDataBase dataBase)
        {
            var key = dataBase.DBKey;

            var versionData = LoadVersionFile(BasePath);
            var findVersion = versionData.GetVersionValue(key);

            if (findVersion == null)
            {
                sb.AppendLine(string.Format("{0} is NULL", key));
                return false;
            }

            var filePath = string.Format("{0}/db/{1}/{2}.db",
                BasePath,
                findVersion.key,
                findVersion.ToString());

            var fullPath = GetDBFullPath(filePath);

            usingSqlite.Load(dataBase, fullPath, findVersion.key);

            if (usingSqlite.ErrorMsg != string.Empty)
            {
                sb.Append(usingSqlite.ErrorMsg);
                return false;
            }

            return true;
        }

        public VersionData LoadVersionFile(string dbBasePath)
        {
            var versionInfo = ReadVersionFile(dbBasePath + "/version.txt");

            var versionData = new VersionData();
            versionData.AddVerionData(versionInfo);

            return versionData;
        }

        //플랫폼 마다 path가 달라지기 때문에 필요시 재정의 필수
        public virtual string ReadVersionFile(string path)
        {
            return File.ReadAllText(path);
        }

        //플랫폼 마다 path가 달라지기 때문에 필요시 재정의 필수
        public virtual string GetDBFullPath(string path)
        {
            return Path.GetFullPath(path);
        }
    }
}
