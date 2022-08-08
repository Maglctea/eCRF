using eCRF.Interface;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCRF.ViewerModels
{
    [Table("Pe")]
    public class Pe : ITable
    {
        public Pe(int id, int id_player, int season, string nickname, string name, string visit_date, float temperature, float sad, float dad, float hdd, float hss)
        {
            this.id = id;
            this.id_player = id_player;
            this.season = season;
            this.nickname = nickname;
            this.name = name;
            this.visit_date = visit_date;
            this.temperature = temperature;
            this.sad = sad;
            this.dad = dad;
            this.hdd = hdd;
            this.hss = hss;
        }

        public Pe()
        {
            this.id = id;
            this.id_player = id_player;
            this.season = season;
            this.nickname = nickname;
            this.name = name;
            this.visit_date = visit_date;
            this.temperature = temperature;
            this.sad = sad;
            this.dad = dad;
            this.hdd = hdd;
            this.hss = hss;
        }


        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int id_player { get; set; }
        public int season { get; set; }
        public string nickname { get; set; }
        public string? name { get; set; } = null;
        public string? visit_date { get; set; } = DateTime.Today.ToString("M/d/yyyy") + " 00:00:00 AM";
        public float? temperature { get; set; } = null;
        public float? sad { get; set; } = null;
        public float? dad { get; set; } = null;
        public float? hdd { get; set; } = null;
        public float? hss { get; set; } = null;
    }
}
