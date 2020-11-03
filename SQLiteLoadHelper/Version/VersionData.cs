using System.Collections.Generic;
using System.Text;

namespace SQLiteLoadHelper.Version
{
    //db파일 버전 관리 데이터
    public class VersionData
    {
        private Dictionary<string, Version> versionDic = null;

        public VersionData()
        {
            versionDic = new Dictionary<string, Version>();
        }

        public void AddVerionData(string infoString)
        {
            if (infoString == string.Empty)
                return;

            var infoArray = infoString.Split('/');
            foreach (var info in infoArray)
            {
                if (info != string.Empty)
                {
                    var split = info.Split('_');
                    var fileName = split[0];
                    var fileVersion = split[1];

                    AddNewVerionData(fileName, fileVersion);
                }
            }
        }

        private Version AddNewVerionData(string fileName, string version = "-1")
        {
            var value = new Version(fileName, version);
            if (!versionDic.ContainsKey(fileName))
            {
                versionDic.Add(fileName, value);
            }
            return value;
        }

        //key에 맞는 버전 반환 (key는 주로 DB 이름)
        public Version GetVersionValue(string key)
        {
            if (versionDic.ContainsKey(key))
            {
                return versionDic[key];
            }
            return null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var file in versionDic)
            {
                var ver = file.Value;
                sb.AppendFormat("{0}/", ver.ToString());
            }

            return sb.ToString();
        }
    }
}
