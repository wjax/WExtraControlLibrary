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

namespace WExtraControlLibrary.UserControls.FloatUpDown
{
    /// <summary>
    /// Interaction logic for FloatUpDownControl.xaml
    /// </summary>
    public partial class FloatUpDownControl : UserControl
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Commmand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(FloatUpDownControl), new PropertyMetadata(null));


        public string Suffix
        {
            get { return (string)GetValue(SuffixProperty); }
            set { SetValue(SuffixProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Suffix.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuffixProperty =
            DependencyProperty.Register("Suffix", typeof(string), typeof(FloatUpDownControl), new PropertyMetadata("", OnSuffixChange));

        private static void OnSuffixChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FloatUpDownControl;
            control.TB_Value.Text = $"{control.Value} {e.NewValue.ToString()}";
        }


        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(FloatUpDownControl), new PropertyMetadata(double.MaxValue));


        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(FloatUpDownControl), new PropertyMetadata(0d));


        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(FloatUpDownControl), new PropertyMetadata(0d,OnValueChange));

        private static void OnValueChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FloatUpDownControl;
            if (!control.CheckValueWithinLimits((double)e.NewValue))
                return;

            control.TB_Value.Text = String.Format(CultureInfo.InvariantCulture, "{0:0.00}", (double)e.NewValue) + $" {control.Suffix}";
        }

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(FloatUpDownControl), new PropertyMetadata(100d));

        private bool UpPressed = false;
        private bool DownPressed = false;


        public FloatUpDownControl()
        {
            InitializeComponent();
        }

        public bool CheckValueWithinLimits(double value)
        {
            return value >= MinValue & value <= MaxValue;
        }

        private async void B_UP_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var newValue = Value + Step;
            if (!CheckValueWithinLimits(newValue))
                return;

            Value = newValue;
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
            var newValue = Value - Step;
            if (!CheckValueWithinLimits(newValue))
                return;

            Value = newValue;
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
