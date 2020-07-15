using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WExtraControlLibrary.UserControls.DropDownCustomColorPicker
{
    /// <summary>
    /// Interaction logic for CustomColorPicker.xaml
    /// </summary>
    public partial class CustomColorPicker : UserControl
    {
        public event Action<Color> SelectedColorChanged;

        string _hexValue = string.Empty;

        public string HexValue
        {
            get { return _hexValue; }
            set { _hexValue = value; }
        }



        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(CustomColorPicker), new PropertyMetadata(Colors.Transparent, SelectedColorChangedDP));

        private static void SelectedColorChangedDP(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomColorPicker p = d as CustomColorPicker;
            p.recContent.Fill = new SolidColorBrush((Color)e.NewValue);
        }




        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(CustomColorPicker), new PropertyMetadata(null, ButtonStyleChanged));


        private static void ButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomColorPicker p = d as CustomColorPicker;
            p.b.Style = (Style)e.NewValue;
        }



        public int ButtonWidth
        {
            get { return (int)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register("ButtonWidth", typeof(int), typeof(CustomColorPicker), new PropertyMetadata(30, ButtonWidthChanged));

        private static void ButtonWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomColorPicker p = d as CustomColorPicker;
            p.b.Width = (int)e.NewValue;
            p.recContent.Width = p.b.Width - 15;
        }

        public int ButtonHeight
        {
            get { return (int)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register("ButtonHeight", typeof(int), typeof(CustomColorPicker), new PropertyMetadata(30, ButtonHeightChanged));

        private static void ButtonHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomColorPicker p = d as CustomColorPicker;
            p.b.Height = (int)e.NewValue;
            p.recContent.Height = p.b.Height - 15;
        }


        //private Color selectedColor = Colors.Transparent;
        //public Color SelectedColor
        //{
        //    get { return selectedColor; }
        //    set
        //    {
        //        if (selectedColor != value)
        //        {
        //            selectedColor = value;
        //            recContent.Fill = new SolidColorBrush(selectedColor);
        //        }
        //    }
        //}

        bool _isContexMenuOpened = false;
        public CustomColorPicker()
        {
            InitializeComponent();
            b.ContextMenu.Closed += new RoutedEventHandler(ContextMenu_Closed);
            b.ContextMenu.Opened += new RoutedEventHandler(ContextMenu_Opened);
            b.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(b_PreviewMouseLeftButtonUp);
        }

        void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            _isContexMenuOpened = true;
        }

        void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            if (!b.ContextMenu.IsOpen)
            {
                //if (SelectedColorChanged != null)
                //{
                //    SelectedColorChanged(cp.CustomColor);
                //}
                //recContent.Fill = new SolidColorBrush(cp.CustomColor);
                SelectedColor = cp.CustomColor;
                HexValue = string.Format("#{0}", cp.CustomColor.ToString().Substring(1));

            }
            _isContexMenuOpened = false;
        }

        void b_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isContexMenuOpened)
            {
                if (b.ContextMenu != null && b.ContextMenu.IsOpen == false)
                {
                    b.ContextMenu.PlacementTarget = b;
                    b.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                    ContextMenuService.SetPlacement(b, System.Windows.Controls.Primitives.PlacementMode.Bottom);
                    b.ContextMenu.IsOpen = true;
                }
            }
        }
    }
}
