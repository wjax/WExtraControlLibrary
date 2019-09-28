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

namespace WExtraControlLibrary.UserControls.CircularProgressBar
{
    /// <summary>
    /// Interaction logic for CircularProgressBar.xaml
    /// </summary>
    /// 
    

    public partial class CircularProgressBar : UserControl
    {

        //public static readonly DependencyProperty ValueProperty =
        //    DependencyProperty.Register("Value", typeof(int?), typeof(CircularProgressBar),
        //        new PropertyMetadata(0, new PropertyChangedCallback(CircularProgressBar.OnValuePropertyChanged)));

        public static DependencyProperty ValueProperty =
                DependencyProperty.Register(
                "Value",
                typeof(int),
                typeof(CircularProgressBar),
                new PropertyMetadata(0, CircularProgressBar.OnValuePropertyChanged));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        #region Properties
        private double _strokeThickness = 10;
        [Category("ProgressBar")]
        public double StrokeThickness
        {
            get { return _strokeThickness; }
            set
            {
                _strokeThickness = value;
                DrawCircle();
            }
        }

        private bool _showValue = true;
        [Category("ProgressBar")]
        public bool ShowValue
        {
            get { return _showValue; }
            set { _showValue = value; DrawCircle(); }
        }

        private Brush _backgroundStroke = Brushes.LightGreen;
        [Category("ProgressBar")]
        public Brush BackgroundStroke
        {
            get { return _backgroundStroke; }
            set
            {
                _backgroundStroke = value;
                DrawCircle();
            }
        }

        private Brush _stroke = Brushes.Green;
        [Category("ProgressBar")]
        public Brush Stroke
        {
            get { return _stroke; }
            set
            {
                _stroke = value;
                DrawCircle();
            }
        }


        //public int Value
        //{
        //    get
        //    {
        //        return (int)GetValue(ValueProperty);
        //    }
        //    set
        //    {
        //        SetValue(ValueProperty, value);
        //    }
        //}

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircularProgressBar c = (CircularProgressBar)d;
            //c.Value = (int)e.NewValue;
            c.DrawCircle();
        }

        //private double _value = 45;
        //[Category("ProgressBar")]
        //public double Value
        //{
        //    get { return _value; }
        //    set
        //    {
        //        _value = value;
        //        DrawCircle();
        //    }
        //}

        private double _maxValue = 100;
        [Category("ProgressBar")]
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                DrawCircle();
            }
        }

        private double _minValue = 0;
        [Category("ProgressBar")]
        public double MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                DrawCircle();
            }
        }
        #endregion

        public CircularProgressBar()
        {
            InitializeComponent();
        }

        #region Calculs
        private double Pourcent
        {
            get
            {
                double value = (Value != null ? (double)Value : 0);
                if (Value > _maxValue)
                {
                    return 100.0d;
                }
                else if (Value < _minValue)
                {
                    return 0.0d;
                }
                else
                {
                    return (100.0d / (_maxValue - _minValue)) * (value - _minValue);
                }
            }
        }

        private double Angle
        {
            get { return (360.0d / 100) * Pourcent; }
        }

        private Point StartUp
        {
            get { return new Point(mainGrid.Width / 2, 0); }
        }

        private Point StartDown
        {
            get { return new Point(mainGrid.Width / 2, mainGrid.Height); }
        }

        private Point CircleCenter
        {
            get { return new Point(mainGrid.Width / 2, mainGrid.Height / 2); }
        }

        private Size CircleSize
        {
            get { return new Size(mainGrid.Width / 2, mainGrid.Height / 2); }
        }

        private double HalfWidth
        {
            get { return mainGrid.Width / 2; }
        }

        private double HalfHeight
        {
            get { return mainGrid.Height / 2; }
        }

        private double CircleWidth
        {
            get { return mainGrid.Width; }
        }

        private double CircleHeight
        {
            get { return mainGrid.Height; }
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 360.0d;
        }
        #endregion

        #region DrawFunctions
        public void ResizeMainGrid()
        {
            if (this.ActualHeight > this.ActualWidth)
            {
                mainGrid.Height = this.ActualWidth;
                mainGrid.Width = this.ActualWidth;
            }
            else
            {
                mainGrid.Height = this.ActualHeight;
                mainGrid.Width = this.ActualHeight;
            }
        }

        public void CalculSize()
        {
            ResizeMainGrid();

            pathFigure_arc1.StartPoint = StartUp;
            ArcSegment_arc1.Size = CircleSize;
            ArcSegment_arc1.Point = StartDown;


            pathFigure_arc2.StartPoint = StartDown;
            ArcSegment_arc2.Size = CircleSize;
            ArcSegment_arc2.Point = StartUp;

            segment1.StartPoint = StartUp;
            segment1_seg1.Point = CircleCenter;

            segment2.StartPoint = StartDown;
            segment2_seg1.Point = CircleCenter;

            EllipseMask.Center = CircleCenter;

            double EllipseMaskX = (CircleWidth / 2) - _strokeThickness;
            double EllipseMaskY = (CircleHeight / 2) - _strokeThickness;
            if (EllipseMaskX < 0) EllipseMaskX = 0;
            if (EllipseMaskY < 0) EllipseMaskY = 0;
            EllipseMask.RadiusX = EllipseMaskX;
            EllipseMask.RadiusY = EllipseMaskY;

            backgroundEllipse1.Fill = _backgroundStroke;

            backgroundEllipseMask1.Center = CircleCenter;
            backgroundEllipseMask1.RadiusX = CircleWidth;
            backgroundEllipseMask1.RadiusY = CircleHeight;

            backgroundEllipseMask2.Center = CircleCenter;
            backgroundEllipseMask2.RadiusX = EllipseMaskX;
            backgroundEllipseMask2.RadiusY = EllipseMaskY;
        }

        public void CalculBar()
        {
            double angle = Angle;
            if (angle < 180.0d)
            {
                double arc1X = (Math.Cos(DegreeToRadian((angle) - 90.0d) * 2) * HalfWidth) + HalfWidth;
                double arc1Y = (Math.Sin(DegreeToRadian((angle) - 90.0d) * 2) * HalfHeight) + HalfHeight;
                ArcSegment_arc1.Point = new Point(arc1X, arc1Y);
                ArcSegment_arc2.Point = StartDown;
                segment1_seg2.Point = new Point(arc1X, arc1Y);
                segment2.IsFilled = false;
                segment1.IsFilled = true;
            }
            else
            {
                ArcSegment_arc1.Point = StartDown;
                double arc2X = (Math.Cos(DegreeToRadian((angle) - 90.0d) * 2) * HalfWidth) + HalfWidth;
                double arc2Y = (Math.Sin(DegreeToRadian((angle) - 90.0d) * 2) * HalfHeight) + HalfHeight;
                ArcSegment_arc2.Point = new Point(arc2X, arc2Y);
                segment2_seg2.Point = new Point(arc2X, arc2Y);
                segment1.IsFilled = false;
                segment2.IsFilled = true;
            }
        }

        private void DrawCircle()
        {
            CalculSize();
            CalculBar();
            textBlockCenter.FontStyle = this.FontStyle;
            textBlockCenter.FontSize = this.FontSize;
            textBlockCenter.Visibility = _showValue ? Visibility.Visible : Visibility.Hidden;
            textBlockCenter.Text = Pourcent.ToString("##0") + " %";

            ellipse1.Fill = _stroke;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawCircle();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawCircle();
        }
        #endregion
    }
}
