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

namespace WExtraControlLibrary.UserControls.TimeLine
{
    /// <summary>
    /// Interaction logic for TimeLineControl.xaml
    /// </summary>
    public partial class TimeLineControl : UserControl
    {
        public TimeLineControl()
        {
            InitializeComponent();
        }

        ///// <summary>Dependency property to Get/Set the current IsActive (True/False)</summary>
        //public static readonly DependencyProperty StartDateTimeProperty =
        //    DependencyProperty.Register("StartDateTime", typeof(DateTime?), typeof(TimeLineControl),
        //        new PropertyMetadata(null, new PropertyChangedCallback(TimeLineControl.StartDateTimePropertyChanced)));

        ///// <summary>Dependency property to Get/Set the current IsActive (True/False)</summary>
        //public static readonly DependencyProperty StopDateTimeProperty =
        //    DependencyProperty.Register("StopDateTime", typeof(DateTime?), typeof(TimeLineControl),
        //        new PropertyMetadata(null, new PropertyChangedCallback(TimeLineControl.StopDateTimePropertyChanced)));

        /// <summary>Dependency property to Get/Set the current IsActive (True/False)</summary>
        //public static readonly DependencyProperty MaxDateTimeProperty =
        //    DependencyProperty.Register("MaxDateTime", typeof(DateTime?), typeof(TimeLineControl),
        //        new PropertyMetadata(null, new PropertyChangedCallback(TimeLineControl.MaxDateTimePropertyChanced)));

        /// <summary>Dependency property to Get/Set the current IsActive (True/False)</summary>
        //public static readonly DependencyProperty MinDateTimeProperty =
        //    DependencyProperty.Register("MinDateTime", typeof(DateTime?), typeof(TimeLineControl),
        //        new PropertyMetadata(null, new PropertyChangedCallback(TimeLineControl.MinDateTimePropertyChanced)));


        /// <summary>Gets/Sets Value</summary>
        //public DateTime? StartDateTime
        //{
        //    get { return (DateTime?)GetValue(StartDateTimeProperty); }
        //    set
        //    {
        //        SetValue(StartDateTimeProperty, value);
        //    }
        //}

        ///// <summary>Gets/Sets Value</summary>
        //public DateTime? StopDateTime
        //{
        //    get { return (DateTime?)GetValue(StopDateTimeProperty); }
        //    set
        //    {
        //        SetValue(StopDateTimeProperty, value);
        //    }
        //}
        /// <summary>Gets/Sets Value</summary>
        //public DateTime? MaxDateTime
        //{
        //    get { return (DateTime?)GetValue(MaxDateTimeProperty); }
        //    set
        //    {
        //        SetValue(MaxDateTimeProperty, value);
        //    }
        //}

        /// <summary>Gets/Sets Value</summary>
        //public DateTime? MinDateTime
        //{
        //    get { return (DateTime?)GetValue(MinDateTimeProperty); }
        //    set
        //    {
        //        SetValue(MinDateTimeProperty, value);
        //    }
        //}

        //private static void StartDateTimePropertyChanced(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    TimeLineControl led = (TimeLineControl)d;
        //}
        //private static void StopDateTimePropertyChanced(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    TimeLineControl led = (TimeLineControl)d;
        //}
        //private static void MaxDateTimePropertyChanced(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    TimeLineControl control = (TimeLineControl)d;
        //    control.timeLinePanel.EndDate = (DateTime)e.NewValue;
        //}
        //private static void MinDateTimePropertyChanced(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    TimeLineControl control = (TimeLineControl)d;
        //    control.timeLinePanel.BeginDate = (DateTime)e.NewValue;
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Min:" + timeLinePanel.BeginDate.ToString());
            System.Diagnostics.Debug.WriteLine("Max:" + timeLinePanel.EndDate.ToString());
            //System.Diagnostics.Debug.WriteLine("Start:" + timeLinePanel.BeginDate.ToString());
            //System.Diagnostics.Debug.WriteLine("Stop:" + timeLinePanel.BeginDate.ToString());

            DateTime begin = DateTime.Parse("2017-03-08T16:00:00");
            DateTime end = DateTime.Parse("2017-03-08T17:00:00");

            timeLinePanel.BeginDate = begin;
            timeLinePanel.EndDate = end;
            
        }

        public void SetMinMaxTimeLine(DateTime min, DateTime max)
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() => timeLinePanel.BeginDate = min));
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() => timeLinePanel.EndDate = max));
            
        }
    }

}
