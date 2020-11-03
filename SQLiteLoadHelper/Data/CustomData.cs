namespace SQLiteLoadHelper.Data
{
    public class CustomData : ICustomData
    {
        public int ID { get; set; }
        public string NAME { get; set; }

        public void Init()
        {
            this.ID = 0;
            this.NAME = "데이터 없음";
        }
    }
}
