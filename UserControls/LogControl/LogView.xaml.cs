using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WExtraControlLibrary.UserControls.LogControl;
using System.Collections.Specialized;

namespace WExtraControlLibrary.UserControls
{

    public partial class LogView : UserControl
    {
        

        public LogView()
        {
            InitializeComponent();

            ((INotifyCollectionChanged)LB_LogItems.Items).CollectionChanged += LogView_CollectionChanged;
        }

        private void LogView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // scroll the new item into view   
                LB_LogItems.ScrollIntoView(e.NewItems[0]);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<LogEntry> LogEntries = (ObservableCollection<LogEntry>)LB_LogItems.ItemsSource;
            LogEntries.Clear();
        }
    }
}
