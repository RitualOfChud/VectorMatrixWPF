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

        public SquareGrid() { }
        public SquareGrid(int maxGridSize) { MaxSize = maxGridSize; }

        //////////////////////////
        // MEMBERS & PROPERTIES //
        //////////////////////////

        // MATHMATICAL PROPERTIES
        public int MaxSize { get; set; } = 10;

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

        public ObservableCollection<DWLine> BasisVectorLines { get; } = new ObservableCollection<DWLine>();
        public ObservableCollection<DWLine> VectorLines { get; } = new ObservableCollection<DWLine>();
        public ObservableCollection<DWLine> ShownVectorLines { get; } = new ObservableCollection<DWLine>();

        // CANVAS PROPERTIES
        private double _unitLength;
        public double UnitLength
        {
            get { return _unitLength; }
            set { NotifyPropertyChanged(); _unitLength = value; }
        }

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

        // INIT METHODS
        public void SetGridSize (int size) => MaxSize = size;

        // CANVAS METHODS
        public void ResizeCanvasElement(object element, RoutedEventArgs e)
        {
            if (element is Canvas canvas)
            {
                CanvasHeight = canvas.ActualHeight;
                CanvasWidth = canvas.ActualWidth;
                CanvasXOrigin = CanvasWidth / 2.0;
                CanvasYOrigin = CanvasHeight / 2.0;
                UnitLength = CanvasWidth / (MaxSize * 2.0);
                IHat.X = 1;
                IHat.Y = 0;
                JHat.X = 0;
                JHat.Y = 1;
            }
        }

        public void DrawGridLines(Canvas canvas)
        {
            for (int i = 0; i <= MaxSize * 2; i++)
            {
                Line xline = new Line
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.5,
                    X1 = UnitLength * i,
                    Y1 = 0,
                    X2 = UnitLength * i,
                    Y2 = CanvasHeight
                };

                Line yline = new Line
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.5,
                    X1 = 0,
                    Y1 = UnitLength * i,
                    X2 = CanvasWidth,
                    Y2 = UnitLength * i
                };

                canvas.Children.Add(xline);
                canvas.Children.Add(yline);
            }
        }

        // VECTOR METHODS
        public void ShowAllVectors(Canvas canvas)
        {
            foreach (DWLine dwline in BasisVectorLines)
            {
                if (!canvas.Children.Contains(dwline.Line))
                    canvas.Children.Add(dwline.Line);
            }

            foreach (DWLine dwline in VectorLines)
            {
                if (!canvas.Children.Contains(dwline.Line))
                    canvas.Children.Add(dwline.Line);
            }
        }

        public void ShowBasisVectors(Canvas canvas)
        {
            if (BasisVectorLines.Count == 0) { 
                BasisVectorLines.Add(new DWLine
                (
                    IHat.X, IHat.Y,
                    new Line
                    {
                        Stroke = Brushes.LightGreen,
                        StrokeThickness = 2,
                        X1 = CanvasXOrigin,
                        Y1 = CanvasYOrigin,
                        X2 = CanvasXOrigin + (IHat.X * UnitLength),
                        Y2 = CanvasYOrigin - (IHat.Y * UnitLength)
                    }
                ));

                BasisVectorLines.Add(new DWLine
                (
                    JHat.X, JHat.Y,
                    new Line { 
                        Stroke = Brushes.Red,
                        StrokeThickness = 2,
                        X1 = CanvasXOrigin,
                        Y1 = CanvasYOrigin,
                        X2 = CanvasXOrigin + (JHat.X * UnitLength),
                        Y2 = CanvasYOrigin - (JHat.Y * UnitLength)
                    }
                ));
            }
        }

        public void AddVector(Canvas canvas, double x, double y)
        {
            DWVector vector = VectorMath.GetVectorLocation(IHat, JHat, x, y);
            
            DWLine vectorLine = new DWLine
            (
                x, y,
                new Line { 
                    Stroke = PickRandomColor(),
                    X1 = CanvasXOrigin,
                    Y1 = CanvasYOrigin,
                    X2 = CanvasXOrigin + (vector.X * UnitLength),
                    Y2 = CanvasYOrigin - (vector.Y * UnitLength)
                }
            );

            VectorLines.Add(vectorLine);
            ShownVectorLines.Add(vectorLine);
            NotifyPropertyChanged("ShownVectorLines");
        }

        public void RotateNDegreesAntiClockwise(double n)
        {
            DWMatrix newPlane = VectorMath.RotateNDegreesAntiClockwise(new DWMatrix(IHat, JHat), ((n * Math.PI) / 180));

            IHat.X = newPlane.IX;
            IHat.Y = newPlane.IY;
            JHat.X = newPlane.JX;
            JHat.Y = newPlane.JY;
            UpdateVectorLines();
        }

        public void Rotate90DegreesClockwise()
        {
            DWMatrix newPlane = VectorMath.Rotate90Clockwise(new DWMatrix(IHat, JHat));

            IHat.X = newPlane.IX;
            IHat.Y = newPlane.IY;
            JHat.X = newPlane.JX;
            JHat.Y = newPlane.JY;
            UpdateVectorLines();
        }

        public void Rotate90DegreesAntiClockwise()
        {
            DWMatrix newPlane = VectorMath.Rotate90AntiClockwise(new DWMatrix(IHat, JHat));

            IHat.X = newPlane.IX;
            IHat.Y = newPlane.IY;
            JHat.X = newPlane.JX;
            JHat.Y = newPlane.JY;
            UpdateVectorLines();
        }

        // transformation methods
        public void TransformPlane(Canvas canvas)
        {
            DWMatrix newPlane = VectorMath.LinearTransformation(new DWMatrix(1, 0, 1, 1), new DWMatrix(IHat, JHat));

            IHat.X = newPlane.IX;
            IHat.Y = newPlane.IY;
            JHat.X = newPlane.JX;
            JHat.Y = newPlane.JY;
            UpdateVectorLines();
        }


        // HELPER METHODS
        private DWVector ConvertPlaneVectorToMathmaticalVector(DWVector vector)
        {
            return new DWVector(vector.X / UnitLength, vector.Y / UnitLength);
        }

        private double TranslatePoint(double point)
        {
            return (point * 2) / UnitLength;
        }

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

            foreach (DWLine dwline in BasisVectorLines)
            {
                DWVector vector = VectorMath.GetVectorLocation(IHat, JHat, dwline.X, dwline.Y);
                dwline.Line.X2 = CanvasXOrigin + (vector.X * UnitLength);
                dwline.Line.Y2 = CanvasYOrigin - (vector.Y * UnitLength);
            }

            foreach (DWLine dwline in VectorLines)
            {
                DWVector vector = VectorMath.GetVectorLocation(IHat, JHat, dwline.X, dwline.Y);
                dwline.Line.X2 = CanvasXOrigin + (vector.X * UnitLength);
                dwline.Line.Y2 = CanvasYOrigin - (vector.Y * UnitLength);
            }
        }

    }
}
