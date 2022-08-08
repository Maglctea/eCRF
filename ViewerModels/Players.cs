using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCRF.Data;
using eCRF.Interface;
using SQLite;

namespace eCRF.ViewerModels
{
    [Table("Players")]
    public class Players : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string nickname { get; set; }
        public string name { get; set; }
        public string guild { get; set; }
        public string guild_date { get; set; } = DateTime.Today.ToString("M/d/yyyy") + " 12:00:00 AM";
        public string birth_date { get; set; } = DateTime.Today.ToString("M/d/yyyy") + " 12:00:00 AM";
        public float qualification_score { get; set; }
        public string qualification_level { get; set; }
        public int age { get; set; }
        public string sex { get; set; }
        public string size { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string notes { get; set; }         
    }

    public class SexList : List<string>
    {
        public SexList()
        {
            this.Add("М");
            this.Add("Ж");
        }
    }

    public class SizeList : List<string>
    {
        public SizeList()
        {
            this.Add("XS");
            this.Add("S");
            this.Add("M");
            this.Add("L");
            this.Add("XL");
            this.Add("XXL");
            this.Add("XXXL");
        }
    }

    public class QualificationList : List<string>
    {
        public QualificationList()
        {
            this.Add("I разряд");
            this.Add("КМС");
            this.Add("Мастер");
        }
    }

}
