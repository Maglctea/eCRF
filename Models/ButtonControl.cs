using eCRF.Data;
using eCRF.Interface;
using eCRF.ViewerModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace eCRF.Models
{
    public class ButtonControl
    {
        public string dbPath { get => AppSettings.dbPath; set => AppSettings.dbPath = value; }
        TableControl tableControl = new TableControl();
        Settings settings = new Settings();

        public async Task AddTableRow<M>(DataGrid sender) where M : ITable, new()
        {
            if (dbPath == null)
                return;
            DB<M> db = new DB<M>(dbPath);
            List<M> data;

            data = new List<M>(await db.GetItemsAsync());
            data.Add(new M());
            sender.ItemsSource = data;
            await settings.updateLastSave();
        }

        public async Task DeleteTableRow<M>(DataGrid sender) where M : ITable, new()
        {
            try
            {
                M selectedRow = (M)sender.SelectedItem; // Выделенная строка

                if (selectedRow != null && dbPath != null)
                {
                    DB<M> db = new DB<M>(dbPath);
                    await db.DeleteItemAsync(selectedRow);

                    await settings.updateLastSave();
                    await tableControl.updateTable<M>(sender);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
