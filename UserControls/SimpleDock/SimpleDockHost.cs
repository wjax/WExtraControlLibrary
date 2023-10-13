using MaterialDesignExtensions.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WExtraControlLibrary.UserControls.SimpleDock
{
    [TemplatePart(Name = B_Undock, Type = typeof(System.Windows.Controls.Button))]
    public class SimpleDockHost : ContentControl
    {
        private const string B_Undock = "B_Undock";
        private Window detachedWindow;

        private const int MARGIN_WINDOW = 10;

        #region dp
        public UIElement Header
        {
            get { return (UIElement)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(UIElement), typeof(SimpleDockHost), new PropertyMetadata(null));

        public bool UseMaterialWindow
        {
            get { return (bool)GetValue(UseMaterialWindowProperty); }
            set { SetValue(UseMaterialWindowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseMaterialWindowProperty =
            DependencyProperty.Register("UseMaterialWindow", typeof(bool), typeof(SimpleDockHost), new PropertyMetadata(false));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SimpleDockHost), new PropertyMetadata("", OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SimpleDockHost h = (d as SimpleDockHost);
            if (h.detachedWindow is not null)
                h.detachedWindow.Title = e.NewValue as string;
        }

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
            var dataContext = currContent.DataContext;
            Content = null;
            // Create Window and keep reference
            Size size = (currContent as UIElement).RenderSize;
            detachedWindow = UseMaterialWindow ? new MaterialWindow() { Title = this.Title, SizeToContent= SizeToContent.Manual, Width = size.Width + MARGIN_WINDOW*2, Height= size.Height + MARGIN_WINDOW*2 } :
                new Window() { Title = this.Title, SizeToContent = SizeToContent.Manual, Width = size.Width + MARGIN_WINDOW*2, Height = size.Height + MARGIN_WINDOW * 2 };
            var grid = new Grid() { Margin = new Thickness(MARGIN_WINDOW) };
            grid.Children.Add(currContent as UIElement);
            detachedWindow.Content = grid;
            detachedWindow.DataContext = dataContext;
            detachedWindow.Closing += OnClosingChildWindow;
            detachedWindow.Show();

            this.Visibility = Visibility.Collapsed;
        }

        private void OnClosingChildWindow(object sender, CancelEventArgs e)
        {
            // Get Grid
            var grid = (sender as Window).Content as Grid;
            // Get child
            var child = grid.Children[0];
            // Get Original Window
            var originalWindow = Window.GetWindow(this);
            // Pass Window to IUndockable control
            (child as IUndockable).SetParentWindow(originalWindow);
            // Disconnect control/child from floating window
            (sender as Window).Content = null;
            grid.Children.Clear();
            // Reconnect here
            Content = child;
            this.Visibility = Visibility.Visible;

            detachedWindow = null;
        }
    }
}
