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

namespace WExtraControlLibrary.UserControls.SimpleDock
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WExtraControlLibrary.UserControls.SimpleDock"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WExtraControlLibrary.UserControls.SimpleDock;assembly=WExtraControlLibrary.UserControls.SimpleDock"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    [TemplatePart(Name = B_Undock, Type = typeof(System.Windows.Controls.Button))]
    public class SimpleDockHost : ContentControl
    {
        private const string B_Undock = "B_Undock";

        #region dp
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SimpleDockHost), new PropertyMetadata(""));

        #endregion

        private bool IsDesignMode => (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

        static SimpleDockHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleDockHost), new FrameworkPropertyMetadata(typeof(SimpleDockHost)));
        }

        public SimpleDockHost()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!IsDesignMode)
            {
                (Template.FindName(B_Undock, this) as System.Windows.Controls.Button).Click += OnUndockClick;
            }
        }

        private void OnUndockClick(object sender, RoutedEventArgs e)
        {
            // Remove from current visual tree
            var currContent = Content as IUndockable;
            Content = null;
            // Create Window and keep reference
            Size size = (currContent as UIElement).RenderSize;
            Window w = new Window() { Title = this.Title, SizeToContent= SizeToContent.Manual, Width = size.Width, Height= size.Height };
            w.Content = currContent;
            w.Closing += OnClosingChildWindow;
            w.Show();

            this.Visibility = Visibility.Collapsed;
        }

        private void OnClosingChildWindow(object sender, CancelEventArgs e)
        {
            // Get child
            var child = (sender as Window).Content;
            // Get Original Window
            var originalWindow = Window.GetWindow(this);
            // Pass Window to IUndockable control
            (child as IUndockable).SetParentWindow(originalWindow);
            // Disconnect control/child from floating window
            (sender as Window).Content = null;
            // Reconnect here
            Content = child;
            this.Visibility = Visibility.Visible;
        }
    }
}
