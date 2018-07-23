using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using VectorMatrixClassLibrary;
using VectorMatrixWPF.Exceptions;

namespace VectorMatrixWPF.Models
{
    public class SquareGrid : INotifyPropertyChanged
    {
        /////////////////
        // CONSTRUCTOR //
        /////////////////

        public SquareGrid(int maxGridSize) { _maxSize = maxGridSize; }

        //////////////////////////
        // MEMBERS & PROPERTIES //
        //////////////////////////

        // MATHMATICAL PROPERTIES
        private readonly int _maxSize = 10;

        private DWVector _iHat = new DWVector();
        public DWVector IHat
        {
            get { return _iHat; }
            set { NotifyPropertyChanged(); _iHat = value; }
        }

        private DWVector _jHat = new DWVector();
        public DWVector JHat
        {
            get { return _jHat; }
            set { NotifyPropertyChanged(); _jHat = value; }
        }

        public ObservableCollection<Line> VectorLines { get; } = new ObservableCollection<Line>();
        public ObservableCollection<Line> ShownVectorLines { get; } = new ObservableCollection<Line>();

        // CANVAS PROPERTIES
        private double _canvasHeight;
        public double CanvasHeight
        {
            get { return _canvasHeight; }
            set { NotifyPropertyChanged(); _canvasHeight = value; }
        }

        private double _canvasWidth;
        public double CanvasWidth
        {
            get { return _canvasWidth; }
            set { NotifyPropertyChanged(); _canvasWidth = value; }
        }

        private double _canvasXOrigin;
        public double CanvasXOrigin
        {
            get { return _canvasXOrigin; }
            set { NotifyPropertyChanged(); _canvasXOrigin = value; }
        }

        private double _canvasYOrigin;
        public double CanvasYOrigin
        {
            get { return _canvasYOrigin; }
            set { NotifyPropertyChanged(); _canvasYOrigin = value; }
        }

        /////////////
        // METHODS //
        /////////////

        // PROPERTY CHANGED IMPLEMENTATION
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // CANVAS METHODS
        public void ResizeCanvasElement(object element, RoutedEventArgs e)
        {
            if (element is Canvas canvas)
            {
                CanvasHeight = canvas.ActualHeight;
                CanvasWidth = canvas.ActualWidth;
                CanvasXOrigin = CanvasWidth / 2;
                CanvasYOrigin = CanvasHeight / 2;
                IHat.X = CanvasWidth / (_maxSize * 2);
                IHat.Y = CanvasYOrigin;
                JHat.X = CanvasXOrigin;
                JHat.Y = CanvasHeight / (_maxSize * 2);
            }
        }

        public void DrawGridLines(Canvas canvas)
        {
            for (int i = 0; i <= _maxSize * 2; i++)
            {
                Line xline = new Line
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.5,
                    X1 = IHat.X * i,
                    Y1 = 0,
                    X2 = IHat.X * i,
                    Y2 = CanvasHeight
                };

                Line yline = new Line
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.5,
                    X1 = 0,
                    Y1 = JHat.Y * i,
                    X2 = CanvasWidth,
                    Y2 = JHat.Y * i
                };

                canvas.Children.Add(xline);
                canvas.Children.Add(yline);
            }
        }

        // VECTOR METHODS
        public void ShowAllVectors(Canvas canvas)
        {
            foreach(Line line in VectorLines)
            {
                if (!canvas.Children.Contains(line))
                    canvas.Children.Add(line);
            }
        }

        public void ShowBasisVectors(Canvas canvas)
        {

            DWVector ihat = VectorMath.GetVectorLocation(IHat.X, JHat.Y, 1, 0);
            DWVector jhat = VectorMath.GetVectorLocation(IHat.X, JHat.Y, 0, 1);

            VectorLines.Add(new Line
            {
                Stroke = Brushes.LightGreen,
                StrokeThickness = 2,
                X1 = CanvasXOrigin,
                Y1 = CanvasYOrigin,
                X2 = CanvasXOrigin + ihat.X,
                Y2 = CanvasYOrigin - ihat.Y
            });

            VectorLines.Add(new Line
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                X1 = CanvasXOrigin,
                Y1 = CanvasYOrigin,
                X2 = CanvasXOrigin + jhat.X,
                Y2 = CanvasYOrigin - jhat.Y
            });

        }

        public void AddVector(Canvas canvas, double x, double y)
        {

            DWVector vector = VectorMath.GetVectorLocation(IHat.X, JHat.Y, x, y);
            if (vector.X > CanvasWidth) throw new OutOfGraphBoundsException();
            if (vector.Y > CanvasHeight) throw new OutOfGraphBoundsException();
            
            Line vectorLine = new Line
            {
                Stroke = PickRandomColor(),
                X1 = CanvasXOrigin,
                Y1 = CanvasYOrigin,
                X2 = CanvasXOrigin + vector.X,
                Y2 = CanvasYOrigin - vector.Y
            };

            VectorLines.Add(vectorLine);
            ShownVectorLines.Add(vectorLine);
            NotifyPropertyChanged("ShownVectorLines");
        }

        public void Rotate90DegreesClockwise()
        {
            double ix, iy, jx, jy;
            ix = IHat.X == CanvasXOrigin ? 0 : IHat.X / IHat.X;
            iy = IHat.Y == CanvasYOrigin ? 0 : IHat.Y / IHat.Y;
            jx = JHat.X == CanvasXOrigin ? 0 : JHat.X / JHat.X;
            jy = JHat.Y == CanvasYOrigin ? 0 : JHat.Y / JHat.Y;

            ix = CanvasXOrigin + IHat.X < CanvasXOrigin ? -ix : ix;
            iy = CanvasYOrigin - IHat.Y > CanvasYOrigin ? -iy : iy;
            jx = CanvasXOrigin + JHat.X < CanvasXOrigin ? -jx : jx;
            jy = CanvasYOrigin - JHat.Y > CanvasYOrigin ? -jy : jy;

            DWMatrix newPlane = VectorMath.Rotate90Clockwise(new DWMatrix(ix, iy, jx, jy));
            IHat.X *= newPlane.IX;
            IHat.Y *= newPlane.IY;
            JHat.X *= newPlane.JX;
            JHat.Y *= newPlane.JY;
            UpdateVectorLines();
        }

        // HELPER METHODS
        private SolidColorBrush PickRandomColor()
        {
            List<SolidColorBrush> colors = new List<SolidColorBrush>() {
                Brushes.Red,
                Brushes.Blue,
                Brushes.Green,
                Brushes.Purple,
                Brushes.Orange,
                Brushes.DarkRed
            };

            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int choice = rand.Next(0, colors.Count);

            return colors[choice];
        }

        private double CheckForOrigin(double val, double multi)
        {
            if (val * multi == 0) return CanvasXOrigin;
            if (val * multi == 0) return CanvasYOrigin;
            return val * multi;
        }

        private void UpdateVectorLines()
        {
            foreach (Line line in VectorLines)
            {
                DWVector vector = VectorMath.GetVectorLocation(IHat.X, JHat.Y, line.X2, line.Y2);
                line.X2 = CanvasXOrigin + vector.X;
                line.Y2 = CanvasYOrigin - vector.Y;
            }
        }

    }
}
