using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WExtraControlLibrary.Animations
{
    public class TransitionAnimation : DependencyObject
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

        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached("Visibility", typeof(Visibility), typeof(TransitionAnimation),
                                                                new PropertyMetadata(new PropertyChangedCallback(OnVisibility)));
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
            DependencyProperty.RegisterAttached("Duration", typeof(int), typeof(TransitionAnimation), new PropertyMetadata(0));
        #endregion

        #region TransitionX
        public static int GetTransitionX(DependencyObject obj)
        {
            return (int)obj.GetValue(TransitionXProperty);
        }

        public static void SetTransitionX(DependencyObject obj, int value)
        {
            obj.SetValue(TransitionXProperty, value);
        }

        // Using a DependencyProperty as the backing store for TransitionX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransitionXProperty =
            DependencyProperty.RegisterAttached("TransitionX", typeof(int), typeof(TransitionAnimation), new PropertyMetadata(0));
        #endregion

        #region TransitionY
        public static int GetTransitionY(DependencyObject obj)
        {
            return (int)obj.GetValue(TransitionYProperty);
        }

        public static void SetTransitionY(DependencyObject obj, int value)
        {
            obj.SetValue(TransitionYProperty, value);
        }

        // Using a DependencyProperty as the backing store for TransitionY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransitionYProperty =
            DependencyProperty.RegisterAttached("TransitionY", typeof(int), typeof(TransitionAnimation), new PropertyMetadata(0));
        #endregion

        #endregion


        private static void OnVisibility(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement target = d as FrameworkElement;
            Visibility visibility = ((Visibility)e.NewValue);

            var reverse = IsVisibilityHidden(visibility);
            var translateTransform = new TranslateTransform()
            {
                X = reverse ? 0 : GetTransitionX(d),
                Y = reverse ? 0 : GetTransitionY(d),
            };
            target.RenderTransform = translateTransform;

            var x = new DoubleAnimation
            {
                From = reverse ? 0 : GetTransitionX(d),
                To = reverse ? GetTransitionX(d) : 0,
                FillBehavior = FillBehavior.HoldEnd,
                BeginTime = TimeSpan.FromMilliseconds(0),
                Duration = new Duration(TimeSpan.FromMilliseconds(GetDuration(d))),
                //EasingFunction = reverse ? (transitionParams.ReverseEase ?? transitionParams.Ease) : transitionParams.Ease,
                //AutoReverse = false,
            };

            var y = new DoubleAnimation
            {
                From = reverse ? 0 : GetTransitionY(d),
                To = reverse ? GetTransitionY(d) : 0,
                FillBehavior = FillBehavior.HoldEnd,
                BeginTime = TimeSpan.FromMilliseconds(0),
                Duration = new Duration(TimeSpan.FromMilliseconds(GetDuration(d))),
                //EasingFunction = reverse ? (transitionParams.ReverseEase ?? transitionParams.Ease) : transitionParams.Ease,
                //AutoReverse = transitionParams.AutoReverse,
            };

            // Directly adding RepeatBehavior to constructor breaks existing animations, so only add it if properly defined
            //if (transitionParams.RepeatBehavior == RepeatBehavior.Forever
            //    || transitionParams.RepeatBehavior.HasDuration
            //    || (transitionParams.RepeatBehavior.HasDuration && transitionParams.RepeatBehavior.Count > 0))
            //{
            //    x.RepeatBehavior = transitionParams.RepeatBehavior;
            //    y.RepeatBehavior = transitionParams.RepeatBehavior;
            //}

            //if (visibility.HasValue)
            //    x.Completed += (_, __) => target.Visibility = visibility.Value;

            //x.SetDesiredFrameRate(24);
            //y.SetDesiredFrameRate(24);

            (target.RenderTransform).BeginAnimation(TranslateTransform.XProperty, x);
            (target.RenderTransform).BeginAnimation(TranslateTransform.YProperty, y);
        }

        private static bool IsVisibilityHidden(Visibility? visibility)
        {
            return visibility == Visibility.Collapsed || visibility == Visibility.Hidden;
        }
    }
}
