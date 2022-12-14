using eCRF.Data;
using eCRF.Interface;
using eCRF.ViewerModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace eCRF.Models
{
    public class TableControl
    {
        Settings settings = new Settings();
        public string dbPath { get => AppSettings.dbPath; set => AppSettings.dbPath = value; }

        public async Task saveTableRow<M>(DataGridRow r) where M : ITable, new()
        // Сохранение в БД модели по строке таблицы
        {
            M item = (M)r.Item;
            if (item == null | dbPath == null)
                return;

            DB<M> db = new DB<M>(dbPath);
            await db.SaveItemAsync(item);
            await settings.updateLastSave();
        }

        public async Task saveTableRow<M>(M item) where M : ITable, new()
        // Сохранение в БД модели по модели
        {
            if (item == null | dbPath == null)
                return;

            DB<M> db = new DB<M>(dbPath);
            await db.SaveItemAsync(item);
            await settings.updateLastSave();
        }

        public async Task updateTable<M>(object sender) where M : ITable, new()
        // Обновление данных в выбранной таблице
        {
            if (sender == null | dbPath == null)
                return;

            DB<M> db = new DB<M>(dbPath);
            List<M> items = await db.GetItemsAsync();

            ((DataGrid)sender).ItemsSource = items;
        }

        public async void table_RowEditEnding<M>(object sender, DataGridRowEditEndingEventArgs e) where M : ITable, new()
        {
            await saveTableRow<M>((DataGridRow)e.Row);
            await updateTable<M>(sender);
            await settings.updateLastSave();
        }

        public async void table_IsVisibleChanged<M>(DependencyPropertyChangedEventArgs e, DataGrid table) where M : ITable, new()
        {
            if ((bool)e.NewValue)
                await updateTable<M>(table);
            else
                if (table.SelectedItem != null)
                await saveTableRow((M)table.SelectedItem);
        }

    }
}
