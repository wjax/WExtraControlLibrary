using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

namespace WExtraControlLibrary.UserControls.IntegerUpDown
{
    /// <summary>
    /// Interaction logic for IntegerUpDownControl.xaml
    /// </summary>
    public partial class IntegerUpDownControl : UserControl
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Commmand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(IntegerUpDownControl), new PropertyMetadata(null));


        public string Suffix
        {
            get { return (string)GetValue(SuffixProperty); }
            set { SetValue(SuffixProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Suffix.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuffixProperty =
            DependencyProperty.Register("Suffix", typeof(string), typeof(IntegerUpDownControl), new PropertyMetadata("", OnSuffixChange));

        private static void OnSuffixChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as IntegerUpDownControl;
            control.TB_Value.Text = $"{control.Value} {e.NewValue.ToString()}";
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(IntegerUpDownControl), new PropertyMetadata(0,OnValueChange));

        private static void OnValueChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as IntegerUpDownControl;
            control.TB_Value.Text = e.NewValue.ToString() + $" {control.Suffix}";
        }

        public int Step
        {
            get { return (int)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(int), typeof(IntegerUpDownControl), new PropertyMetadata(100));

        private bool UpPressed = false;
        private bool DownPressed = false;


        public IntegerUpDownControl()
        {
            InitializeComponent();
        }

        private async void B_UP_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Value += Step;
            UpPressed = true;

            int delay = 2000;
            while (UpPressed)
            {
                await Task.Delay(delay);
                if (UpPressed)
                    Value += Step;
                delay = delay > 500 ? delay - 500 : 100;
            }
        }

        private void B_UP_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            UpPressed = false;
            // Command
            Command?.Execute(Value);
        }

        private void B_DOWN_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            DownPressed = false;
            // Command
            Command?.Execute(Value);
        }

        private async void B_DOWN_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Value -= Step;
            DownPressed = true;

            int delay = 2000;
            while (DownPressed)
            {
                await Task.Delay(delay);
                if (DownPressed)
                    Value -= Step;
                delay = delay > 500 ? delay - 500 : 100;
            }
        }
    }
}
