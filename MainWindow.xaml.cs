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
        ButtonControl buttonControl = new ButtonControl();

        public DB<Settings> dbSettings;

        public string dbPath { get => AppSettings.dbPath; set => AppSettings.dbPath = value; }


        public FileInfo file;
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Text files (*.db)|*.db";
            openFileDialog.DefaultExt = ".db";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            bool? dialogOK = openFileDialog.ShowDialog();
            if (dialogOK == true)
            {                

                dbPath = openFileDialog.FileName;
                file = new FileInfo(dbPath);

                OpenBdInTable();
            }
        }

        private async void OpenBdInTable()
        {
            if (dbPath == null)
                return;

            dbSettings = new DB<Settings>(dbPath);

            dataSettings = new BindingList<Settings>(await dbSettings.GetItemsAsync());

            settings = new Settings();
            ComboBoxTabs.ItemsSource = settings.createListTabs(int.Parse(dataSettings.Where(setting => setting.parameter == "season_count").Select(i => i.value).ToList().First()));
            ComboBoxTabs.SelectedIndex = 0;
            tabControl.SelectedIndex = 1;
            tabControl.SelectedIndex = 0;
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        // Сохранение БД
        {
            if (dbPath == null)
                return;
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
                await settings.updateLastSave();
            }
        }

        private async void MenuItem_Click_2(object sender, RoutedEventArgs e)
        // Создание БД
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.db)|*.db";
            saveFileDialog.DefaultExt = ".db";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo tempfile = new FileInfo(@".\Template\Temp.db");
                tempfile.CopyTo(saveFileDialog.FileName, true);

                file = new FileInfo(saveFileDialog.FileName);
                dbPath = file.FullName;
                await settings.updateLastSave();

                OpenBdInTable();
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        // Кнопка добавления нового игрока
        {
            await buttonControl.AddTableRow<Players>(tablePersonalData.tablePersonalData);            
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        // Кнопка удаления выделенной строки
        {
            await buttonControl.DeleteTableRow<Players>(tablePersonalData.tablePersonalData);
        }


        private async void AddWarning_Click(object sender, RoutedEventArgs e)
        {
            await buttonControl.AddTableRow<Warnings>(tableWarnings.tableWarnings);
        }

        private async void DeleteWarning_Click(object sender, RoutedEventArgs e)
        {
            await buttonControl.DeleteTableRow<Warnings>(tableWarnings.tableWarnings);
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // Переключение между разделами
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
            }
                
        }


    }
}
