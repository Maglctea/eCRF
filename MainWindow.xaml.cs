global using eCRF.Models;
using eCRF.Data;
using eCRF.ViewerModels;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace eCRF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Settings settings = new Settings();

        //static PlayersDB playersDB;
        public DB<Players> dbPlayers;
        public DB<Warnings> dbWarnings;
        public DB<Settings> dbSettings;

        public string dbPath { get => AppSettings.dbPath; set => AppSettings.dbPath = value; }


        public FileInfo file;
        BindingList<Players> dataPlayers;
        BindingList<Warnings> dataWarnings;
        BindingList<Settings> dataSettings;

        DispatcherTimer saveInfoTimer = new DispatcherTimer(); //таймер

        public MainWindow()
        {
            InitializeComponent();
            // Подписка таблиц на события
            //tablePersonalData.tablePersonalData.RowEditEnding += tablePlayersSave;
            //tablePersonalData.tablePersonalData.RowDetailsVisibilityChanged += tablePlayersSave;
            //tableContacts.tableContacts.RowEditEnding += tablePlayersSave;
            saveInfoTimerStart();
        }

        void saveInfoTimerStart()
        {
            if (!saveInfoTimer.IsEnabled)
            {
                saveInfoTimer.Tick += saveInfoTimer_Tick;
                saveInfoTimer.Interval = new TimeSpan(0, 0, 2);
                saveInfoTimer.Start();

            }
        }

        private async void saveInfoTimer_Tick(object sender, EventArgs e)
        {
            if (dbPath == null)
                return;

            dateLastSave.Content = $"Последнее сохранение: {await settings.getLastSave()}";
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        // Открытие БД
        {
            tablePlayersSave();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Text files (*.db)|*.db";
            openFileDialog.DefaultExt = ".db";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Nullable<bool> dialogOK = openFileDialog.ShowDialog();
            if (dialogOK == true)
            {                

                dbPath = openFileDialog.FileName;
                file = new FileInfo(dbPath);

                dbPlayers = new DB<Players>(dbPath);
                dbWarnings = new DB<Warnings>(dbPath);
                dbSettings = new DB<Settings>(dbPath);

                dataPlayers = new BindingList<Players>(await dbPlayers.GetItemsAsync());
                dataSettings = new BindingList<Settings>(await dbSettings.GetItemsAsync());

                tableContacts.tableContacts.ItemsSource = dataPlayers;
                tableNotes.tableNotes.ItemsSource = dataPlayers;
                tablePersonalData.tablePersonalData.ItemsSource = dataPlayers;

                settings = new Settings();
                ComboBoxTabs.ItemsSource = settings.createListTabs(int.Parse(dataSettings.Where(setting => setting.parameter == "season_count").Select(i => i.value).ToList().First()));
                ComboBoxTabs.SelectedIndex = 0;
                tabControl.SelectedIndex = 0;
                string lastSave = await settings.getLastSave();

                
            }
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        // Сохранение БД
        {
            if (dbPath == null)
                return;
            tablePlayersSave();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.db)|*.db";
            saveFileDialog.DefaultExt = ".db";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true && file.Exists)
            {
                file.CopyTo(@".\Cache\" + file.Name, true);
                FileInfo cacheFile = new FileInfo(@".\Cache\" + file.Name);
                cacheFile.CopyTo(saveFileDialog.FileName, true);
                cacheFile.Delete();
                settings.updateLastSave();
            }
        }

        private async void MenuItem_Click_2(object sender, RoutedEventArgs e)
        // Создание БД
        {
            tablePlayersSave();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.db)|*.db";
            saveFileDialog.DefaultExt = ".db";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo tempfile = new FileInfo(@".\Template\Temp.db");
                tempfile.CopyTo(saveFileDialog.FileName, true);

                file = new FileInfo(saveFileDialog.FileName);                
                dbPlayers = new DB<Players>(file.FullName);
                //List<Players> todoData = await playersDB.GetPlayersAsync();
                dataPlayers = new BindingList<Players>(await dbPlayers.GetItemsAsync());
                tablePersonalData.tablePersonalData.ItemsSource = dataPlayers;
                settings.updateLastSave();
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        // Кнопка добавления нового игрока
        {
            if (dbPath == null)
                return;

            dbPlayers = new DB<Players>(dbPath);
            //await dbPlayers.SaveItemAsync(new Players());
            dataPlayers = new BindingList<Players>(await dbPlayers.GetItemsAsync());
            dataPlayers.Add(new Players());
            tablePersonalData.tablePersonalData.ItemsSource = dataPlayers;
            //dataPlayers.ResetItem(dataPlayers.Count - 1);
            await settings.updateLastSave();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        // Кнопка удаления выделенной строки
        {
            try
            {
                Players selectedPlayer = (Players)tablePersonalData.tablePersonalData.SelectedItem; // Выделенная строка

                if (selectedPlayer != null)
                {
                    await dbPlayers.DeleteItemAsync(selectedPlayer);
                    dataPlayers = new BindingList<Players>(await dbPlayers.GetItemsAsync());
                    tablePersonalData.tablePersonalData.ItemsSource = dataPlayers;
                    await settings.updateLastSave();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }



        private async void tablePlayersSave(object sender, SelectionChangedEventArgs e)
        // Обновление и сохранение данных при переходе между вкладками
        {
            Players player = null;
            //Сохранение данных
            if (e.RemovedItems.Count > 0)
                if (e.RemovedItems[0] is TabItem && dbPlayers != null)
                {
                    switch (((TabItem)e.RemovedItems[0]).Name)
                    {
                        case "tabPersonalData":
                            {
                                player = (Players)tablePersonalData.tablePersonalData.SelectedItem;
                                break;
                            }
                        case "tabContacts":
                            {
                                player = (Players)tableContacts.tableContacts.SelectedItem;
                                break;
                            }
                        case "tabNotes":
                            {
                                player = (Players)tableNotes.tableNotes.SelectedItem;
                                break;
                            }
                    }
                    if (player != null)
                    {
                        if (player.nickname != null)
                        {
                            await dbPlayers.SaveItemAsync(player);
                            await settings.updateLastSave();
                        }
                        
                    }
                }
            //Обновление данных
            if (dbPlayers != null)
            {
                dataPlayers = new BindingList<Players>(await dbPlayers.GetItemsAsync());
            }                
        }

        private async void tablePlayersSave(object sender, DataGridRowDetailsEventArgs e)
        // Сохранение данных таблицы при смене строки
        {
            if (e.Row.DetailsVisibility == Visibility.Collapsed)
            {
                Players player = (Players)e.Row.Item;
                if (player.nickname != null)
                {
                    await dbPlayers.SaveItemAsync(player);
                    await settings.updateLastSave();
                }
                    
            }
        }

        private async void tablePlayersSave(object sender, DataGridRowEditEndingEventArgs e)
        // Сохранение данных таблицы при окончании изменения строки
        {
            Players player = (Players)e.Row.Item;
            if (player.nickname != null)
            {
                await dbPlayers.SaveItemAsync(player);
                await settings.updateLastSave();
            }
                
            dataPlayers = new BindingList<Players>(await dbPlayers.GetItemsAsync());
            ((DataGrid)sender).ItemsSource = dataPlayers;
        }

        private async void tablePlayersSave()
        {
            Players player = null;
            switch ((tabControl.SelectedValue as TabItem).Name)
            {
                case "tabPersonalData":
                    {
                        player = (Players)tablePersonalData.tablePersonalData.SelectedItem;
                        break;
                    }
                case "tabContacts":
                    {
                        player = (Players)tableContacts.tableContacts.SelectedItem;
                        break;
                    }
                case "tabNotes":
                    {
                        player = (Players)tableNotes.tableNotes.SelectedItem;
                        break;
                    }
            }
            if (player != null)
            {
                if (player.nickname != null)
                {
                    await dbPlayers.SaveItemAsync(player);
                    await settings.updateLastSave();
                }                    
            }
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // Переключение между вкладками
        {            
            if (mainTabControl == null)
                return;

            if (((ComboBox)sender).SelectedIndex == 0)
                mainTabControl.SelectedIndex = 0;

            if ((((ComboBox)sender).SelectedIndex > 0) && ((ComboBox)sender).SelectedIndex < ((ComboBox)sender).Items.Count - 1)
            {

                tablePe.selectedSeson = ((ComboBox)sender).SelectedIndex;
                mainTabControl.SelectedIndex = 1;
                tablePe.updateTablePe();
            }
                

            if (((ComboBox)sender).SelectedIndex == ((ComboBox)sender).Items.Count - 1)
            {
                mainTabControl.SelectedIndex = 2;
                if (dbPath == null)
                    return;
                //dbPlayers = new DB<Players>(dbPath);
                dataPlayers = new BindingList<Players>(await dbPlayers.GetItemsAsync());

                tableWarnings.nicknameList.ItemsSource = dataPlayers.Select(i => i.nickname);                               
            }
                
        }

        private async void AddWarning_Click(object sender, RoutedEventArgs e)
        {
            if (dbPath == null)
                return;

            dbWarnings = new DB<Warnings>(dbPath);
            //await dbPlayers.SaveItemAsync(new Players());
            dataWarnings = new BindingList<Warnings>(await dbWarnings.GetItemsAsync());
            dataWarnings.Add(new Warnings());
            tableWarnings.tableWarnings.ItemsSource = dataWarnings;
            //dataPlayers.ResetItem(dataPlayers.Count - 1);
            await settings.updateLastSave();
        }

        private async void DeleteWarning_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Warnings selectedWarning = (Warnings)tableWarnings.tableWarnings.SelectedItem; // Выделенная строка

                if (selectedWarning != null)
                {
                    await dbWarnings.DeleteItemAsync(selectedWarning);
                    dataWarnings = new BindingList<Warnings>(await dbWarnings.GetItemsAsync());
                    tableWarnings.tableWarnings.ItemsSource = dataWarnings;
                    await settings.updateLastSave();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

    }
}
