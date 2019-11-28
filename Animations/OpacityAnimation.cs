using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WExtraControlLibrary.Animations
{
    public class OpacityAnimation : DependencyObject
    {
        #region DP

        #region Visibility
        public static Visibility GetVisibility(DependencyObject d)
        {
            return (Visibility)d.GetValue(VisibilityProperty);
        }
        public static void SetVisibility(DependencyObject d, Visibility value)
        {
            d.SetValue(VisibilityProperty, value);
        }

        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), typeof(OpacityAnimation),
                                                                new PropertyMetadata(new PropertyChangedCallback(OnVisibility)));
        #endregion

        #region Control


        public static FrameworkElement GetElement(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(ElementProperty);
        }

        public static void SetElement(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(ElementProperty, value);
        }

        // Using a DependencyProperty as the backing store for Element.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.RegisterAttached("Element", typeof(FrameworkElement), typeof(OpacityAnimation), new PropertyMetadata(new PropertyChangedCallback(OnElementChange)));


        #endregion

        #region Duration
        public static int GetDuration(DependencyObject obj)
        {
            return (int)obj.GetValue(DurationProperty);
        }

        public static void SetDuration(DependencyObject obj, int value)
        {
            obj.SetValue(DurationProperty, value);
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.RegisterAttached("Duration", typeof(int), typeof(OpacityAnimation), new PropertyMetadata(0));
        #endregion

        #region OpacityAtDisabled


        public static double GetOpacityAtDisabled(DependencyObject obj)
        {
            return (double)obj.GetValue(OpacityAtDisabledProperty);
        }

        public static void SetOpacityAtDisabled(DependencyObject obj, double value)
        {
            obj.SetValue(OpacityAtDisabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for OpacityAsDisabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpacityAtDisabledProperty =
            DependencyProperty.RegisterAttached("OpacityAtDisabled", typeof(double), typeof(OpacityAnimation), new PropertyMetadata(0d));


        #endregion

        #endregion

        private static void OnElementChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.IsEnabledChanged += OpacityAnimation.OnIsEnabledChanged;
        }


        private static void OnVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void OnVisibility(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.IsEnabledChanged += OpacityAnimation.OnIsEnabledChanged;

            double opacity = element.IsEnabled ? 1d : GetOpacityAtDisabled(d);
            double to, from;
            element.Visibility = Visibility.Visible;

            if (((Visibility)e.NewValue) == Visibility.Visible)
            {
                to = opacity;
                from = 0;
            }
            else
            {
                to = 0;
                from = opacity;
            }

            AnimateOpacity(element, from, to, GetDuration(d), (Visibility)e.NewValue);
        }

        // Animate Opacity

        private static void AnimateOpacity(FrameworkElement e, double from, double to, double duration, Visibility finalVisibility)
        {
            DoubleAnimation Dblanimation = new DoubleAnimation()
            {
                To = to,
                From = from,
                
            };

            Dblanimation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));
            Dblanimation.Completed += (sender,eArgs) => { e.Visibility = finalVisibility; };
            e.Tag = finalVisibility;
            e.BeginAnimation(Control.OpacityProperty, Dblanimation);
        }

        private static void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;

            double to, from;
            double opacityDisabled = GetOpacityAtDisabled(element);

            if ((bool)e.NewValue)
            {
                to = 1d;
                from = opacityDisabled;
            }
            else
            {
                from = 1d;
                to = opacityDisabled;
            }

            if (GetVisibility(element) == Visibility.Visible)
            {
                //to = opacity;
                //from = opacityDisabled;
                AnimateOpacity(element, from, to, GetDuration(element), Visibility.Visible);
            }
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

    }
}