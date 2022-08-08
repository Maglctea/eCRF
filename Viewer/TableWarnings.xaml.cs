using eCRF.Data;
using eCRF.ViewerModels;
using SQLite;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace eCRF.Viewer
{
    /// <summary>
    /// Логика взаимодействия для TableWarnings.xaml
    /// </summary>
    ///     
    public partial class TableWarnings : UserControl
    {
        DB<Players> dbPlayers;

        DB<Warnings> dbWarnings;

        TableControl tableControl = new TableControl();
        BindingList<Players> dataPlayers;
        public string dbPath { get => AppSettings.dbPath; set => AppSettings.dbPath = value; }

        public TableWarnings()
        {
            InitializeComponent();
        }


        private async void tableWarnings_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            dbWarnings = new DB<Warnings>(dbPath);

            Warnings warning = (Warnings)e.Row.DataContext;
            if (e.Column.SortMemberPath == "nickname")
            {
                dbPlayers = new DB<Players>(dbPath);

                SQLiteAsyncConnection db = dbPlayers.getDB();
                Players player = (await db.QueryAsync<Players>($"SELECT * From Players WHERE nickname = \"{warning.nickname}\"")).FirstOrDefault();
                if (player != null)
                {
                    warning.id_player = player.id;
                    warning.name = player.name;
                    await dbWarnings.SaveItemAsync(warning);
                    tableControl.updateTable<Warnings>(sender);
                }
            }
        }

        private async void tableWarnings_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Warnings warning = (Warnings)e.Row.Item;
            if (warning.nickname != null)
            {
                await tableControl.saveTableRow(warning);
                await tableControl.updateTable<Warnings>(sender);
            }
        }

        private async void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (dbPath == null)
                return;
            await tableControl.updateTable<Warnings>(tableWarnings);

            dbPlayers = new DB<Players>(dbPath); 
            dataPlayers = new BindingList<Players>(await dbPlayers.GetItemsAsync());

            nicknameList.ItemsSource = dataPlayers.Select(i => i.nickname);
        }
    }
}
