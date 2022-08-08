using eCRF.ViewerModels;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для TablePersonalData.xaml
    /// </summary>
    public partial class TablePersonalData : UserControl
    {
        TableControl tableControl = new TableControl();
        public TablePersonalData()
        {
            InitializeComponent();
        }

        private async void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                await tableControl.updateTable<Players>(tablePersonalData);
            else
                if (tablePersonalData.SelectedItem != null)
                await tableControl.saveTableRow((Players)tablePersonalData.SelectedItem);
        }

        private async void tablePersonalData_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
        {
            if (e.Row.DetailsVisibility == Visibility.Collapsed)
            {
                Players player = (Players)e.Row.Item;
                if (player.nickname != null)
                {
                    await tableControl.saveTableRow(player);
                }

            }
        }

        private async void tablePersonalData_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            await tableControl.saveTableRow((Players)e.Row.Item);
            await tableControl.updateTable<Players>(tablePersonalData);
        }
    }
}
