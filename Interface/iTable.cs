using SQLite;

namespace eCRF.Interface
{
    public interface ITable
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
    }
}
