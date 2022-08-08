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
    /// Логика взаимодействия для TableNotes.xaml
    /// </summary>
    public partial class TableNotes : UserControl
    {
        TableControl tableControl = new TableControl();
        Settings settings = new Settings();

        public TableNotes()
        {
            InitializeComponent();
        }

        private async void tableNotes_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {            
            tableControl.table_RowEditEnding<Players>(sender, e);
        }

        private async void tableNotes_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            tableControl.table_IsVisibleChanged<Players>(e, tableNotes);
        }
    }
}
