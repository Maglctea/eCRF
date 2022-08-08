using eCRF.Data;
using eCRF.Interface;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCRF.ViewerModels
{
    public class Settings : ITable
    {
        public string dbPath { get => AppSettings.dbPath; set => AppSettings.dbPath = value; }

        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string parameter { get; set; }
        public string value { get; set; }

        public List<string> createListTabs(int count_season)
        {
            List<string> tabs = new List<string>();
            tabs.Add("Игроки");
            for (int i = 1; i <= count_season; i++)
            {
                tabs.Add($"Сезон {i}");
            }
            tabs.Add("Предупреждения");
            return tabs;
        }

        public async Task updateLastSave()
        {
            if (dbPath == null)
                return;

            DB<Settings> dbSetting = new DB<Settings>(dbPath);

            Settings setting = new Settings();
            setting.id = 2;
            setting.parameter = "last_save";
            setting.value = DateTime.Now.ToString("G");
            //await dbSetting.SaveItemAsync(setting);
            SQLiteAsyncConnection db = dbSetting.getDB();
            await db.ExecuteAsync($"UPDATE `Settings` SET value=\"{setting.value}\" WHERE parameter=\"last_save\"");
        }

        public async Task<string> getLastSave()
        {
            DB<Settings> dbSetting = new DB<Settings>(dbPath);
            List<Settings> settings = await dbSetting.GetItemsAsync();
            string setting = settings.FirstOrDefault(i => i.parameter == "last_save").value;
            return setting;
        }
    }
}
    
    
