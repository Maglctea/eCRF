using eCRF.Data;
using eCRF.ViewerModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eCRF.Viewer
{
    /// <summary>
    /// Логика взаимодействия для TablePe.xaml
    /// </summary>
    public partial class TablePe : UserControl
    {
        public int selectedSeson;
        TableControl tableControl = new TableControl();

        public string dbPath { get => AppSettings.dbPath; set => AppSettings.dbPath = value; }

        public TablePe()
        {
            InitializeComponent();
        }

        public async void tablePE_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {       
            if ((bool)e.NewValue)
            {
               updateTablePe();
            }
            else
            {
                if ((Pe)tablePe.SelectedItem != null)
                    await tableControl.saveTableRow((Pe)tablePe.SelectedItem);
            }
        }

        public async void updateTablePe()
        {

            Pe itemPe;
            BindingList<Pe> dataPe;
            DB<Pe> dbPe;
            BindingList<Pe> newDataPe = new BindingList<Pe>();

            DB<Players> dbPlayers;
            BindingList<Players> dataPlayers;

            if (dbPath == null)
                return;

            dbPlayers = new DB<Players>(dbPath);
            dbPe = new DB<Pe>(dbPath);

            dataPlayers = new BindingList<Players>(await dbPlayers.GetItemsAsync());
            dataPe = new BindingList<Pe>(await dbPe.GetItemsAsync());

            foreach (Players player in dataPlayers)
            {                
                itemPe = dataPe.FirstOrDefault(i => player.id == i.id_player & i.season == selectedSeson);
                if (itemPe == null)
                {
                    itemPe = new Pe();
                    itemPe.id_player = player.id;
                    itemPe.name = player.name;
                    itemPe.nickname = player.nickname;
                    itemPe.season = selectedSeson;
                }
                newDataPe.Add(itemPe);
            }

            tablePe.ItemsSource = newDataPe;
        }

        private async void tablePE_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (dbPath != null)
            {
                await tableControl.saveTableRow((Pe)tablePe.SelectedItem);

                updateTablePe();

            }

        }
    }
}
