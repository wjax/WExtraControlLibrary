﻿using System;
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

        public bool ContinuousUpdate
        {
            get { return (bool)GetValue(ContinuousUpdateProperty); }
            set { SetValue(ContinuousUpdateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContinuousUpdateProperty =
            DependencyProperty.Register(nameof(ContinuousUpdate), typeof(bool), typeof(IntegerUpDownControl), new PropertyMetadata(false));

        public int FastDelay
        {
            get { return (int)GetValue(FastDelayProperty); }
            set { SetValue(FastDelayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FastDelayProperty =
            DependencyProperty.Register(nameof(FastDelay), typeof(int), typeof(IntegerUpDownControl), new PropertyMetadata(100));
        
        
        public int FastStep
        {
            get { return (int)GetValue(FastStepProperty); }
            set { SetValue(FastStepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FastStepProperty =
            DependencyProperty.Register(nameof(FastStep), typeof(int), typeof(IntegerUpDownControl), new PropertyMetadata(100));

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(IntegerUpDownControl), new PropertyMetadata(int.MaxValue));


        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(IntegerUpDownControl), new PropertyMetadata(0));


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
            if (!control.CheckValueWithinLimits((int)e.NewValue))
                return;

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

        public bool CheckValueWithinLimits(int value)
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

            int delay = 1000;
            while (UpPressed)
            {
                await Task.Delay(delay);
                if (UpPressed)
                    Value += delay <= FastDelay ? FastStep : Step;
                delay = delay > 500 ? delay - 500 : FastDelay;
                if (ContinuousUpdate)
                    Command?.Execute(Value);
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

            int delay = 1000;
            while (DownPressed)
            {
                await Task.Delay(delay);
                if (DownPressed)
                    Value -= delay <= FastDelay ? FastStep : Step;
                delay = delay > 500 ? delay - 500 : FastDelay;
                if (ContinuousUpdate)
                    Command?.Execute(Value);
            }
        }
    }
}
