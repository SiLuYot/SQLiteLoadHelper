namespace SQLiteLoadHelper.Version
{
    //db파일 버전 관리
    public class Version
    {
        public string key;
        public string version;

        public Version(string key, string version)
        {
            this.key = key;
            this.version = version;
        }

        public Version(Version v)
        {
            this.key = v.key;
            this.version = v.version;
        }

        public override string ToString()
        {
            return string.Format("{0}_{1}", key, version);
        }
    }
}
