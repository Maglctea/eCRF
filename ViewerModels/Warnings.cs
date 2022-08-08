using eCRF.Interface;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCRF.ViewerModels
{
    [Table("Warnings")]
    public class Warnings : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int id_player { get; set; }
        public string nickname { get; set; }
        public string name { get; set; }
        public string warning_date { get; set; } = DateTime.Today.ToString("M/d/yyyy") + " 12:00:00 AM";
        public string description { get; set; }
        public string result { get; set; }
    }

    public class ResultList : List<string>
    {
        public ResultList()
        {
            this.Add("Предупреждение");
            this.Add("Штраф");
            this.Add("Квалификация");
        }
    }

    public class NicknameList : List<string>
    {
        public NicknameList()
        {
            //this.Add("ник1");
            //this.Add("ник2");
        }
    }

}
