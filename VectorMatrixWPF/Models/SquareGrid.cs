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
        public void SetGridSize(int size) => MaxSize = size;

        // CANVAS METHODS
        public void ResizeCanvasElement(object element, RoutedEventArgs e) => ResizeCanvasElement(element);

        public void ResizeCanvasElement(object element)
        {
            if (element is Canvas plane)
            {
                CanvasHeight = plane.ActualHeight;
                CanvasWidth = plane.ActualWidth;
                CanvasXOrigin = CanvasWidth / 2.0;
                CanvasYOrigin = CanvasHeight / 2.0;
                UnitLength = CanvasWidth / (MaxSize * 2.0);
                IHat.X = 1;
                IHat.Y = 0;
                JHat.X = 0;
                JHat.Y = 1;
            }
        }

        public void ChangeGridLineState(bool activate, bool isOriginal)
        {
            if (isOriginal)
            {
                if (VectorLines.Any(x => x.Type == LineType.GRIDORIG))
                {
                    foreach (DWLine gridline in VectorLines.Where(x => x.Type == LineType.GRIDORIG).ToList())
                    {
                        if (activate) gridline.IsActive = true;
                        else gridline.IsActive = false;
                    }
                }
                else
                    AddOrigGridLines(activate);
            }
            else
            {
                if (VectorLines.Any(x => x.Type == LineType.GRID))
                {
                    foreach (DWLine gridline in VectorLines.Where(x => x.Type == LineType.GRID).ToList())
                    {
                        if (activate) gridline.IsActive = true;
                        else gridline.IsActive = false;
                    }
                }
                else
                    AddGridLines(activate);
            }
        }

        public void AddGridLines(bool isactive = true)
        {
            SolidColorBrush color = Brushes.Blue;

            for (int i = 0; i <= MaxSize * 2; i++)
            {
                DWLine xline = new DWLine
                (
                    i, 0, LineType.GRID,
                    new Line
                    {
                        Stroke = color,
                        StrokeThickness = 0.5,
                        X1 = UnitLength * i,
                        Y1 = 0,
                        X2 = UnitLength * i,
                        Y2 = CanvasHeight
                    },
                    isactive
                );

                DWLine yline = new DWLine
                (
                    i, 0, LineType.GRID,
                    new Line
                    {
                        Stroke = color,
                        StrokeThickness = 0.5,
                        X1 = 0,
                        Y1 = UnitLength * i,
                        X2 = CanvasWidth,
                        Y2 = UnitLength * i
                    },
                    isactive
                );

                VectorLines.Add(xline);
                VectorLines.Add(yline);
            }
        }

        public void AddOrigGridLines(bool isactive = true)
        {
            SolidColorBrush color = Brushes.Gray;

            for (int i = 0; i <= MaxSize * 2; i++)
            {
                DWLine xline = new DWLine
                (
                    i, 0, LineType.GRIDORIG,
                    new Line
                    {
                        Stroke = color,
                        StrokeThickness = 0.3,
                        X1 = UnitLength * i,
                        Y1 = 0,
                        X2 = UnitLength * i,
                        Y2 = CanvasHeight
                    },
                    isactive
                );

                DWLine yline = new DWLine
                (
                    i, 0, LineType.GRIDORIG,
                    new Line
                    {
                        Stroke = color,
                        StrokeThickness = 0.3,
                        X1 = 0,
                        Y1 = UnitLength * i,
                        X2 = CanvasWidth,
                        Y2 = UnitLength * i
                    },
                    isactive
                );

                VectorLines.Add(xline);
                VectorLines.Add(yline);
            }
        }

        // VECTOR METHODS
        public void ShowActiveVectorLines(Canvas plane)
        {
            foreach (DWLine dwline in VectorLines)
            {
                if (dwline.IsActive)
                {
                    if (!plane.Children.Contains(dwline.Line))
                        plane.Children.Add(dwline.Line);
                }
                else
                    plane.Children.Remove(dwline.Line);
            }
        }

        public void AddBasisVectors()
        {
            VectorLines.Add(new DWLine
            (
                IHat.X, IHat.Y,
                LineType.BASE,
                new Line
                {
                    Stroke = Brushes.LightGreen,
                    StrokeThickness = 3,
                    X1 = CanvasXOrigin,
                    Y1 = CanvasYOrigin,
                    X2 = CanvasXOrigin + (IHat.X * UnitLength),
                    Y2 = CanvasYOrigin - (IHat.Y * UnitLength)
                }
            ));

            VectorLines.Add(new DWLine
            (
                JHat.X, JHat.Y,
                LineType.BASE,
                new Line
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 3,
                    X1 = CanvasXOrigin,
                    Y1 = CanvasYOrigin,
                    X2 = CanvasXOrigin + (JHat.X * UnitLength),
                    Y2 = CanvasYOrigin - (JHat.Y * UnitLength)
                }
            ));
        }

        public void ActivateBasisVectors()
        {
            if (VectorLines.Count(x => x.Type == LineType.BASE) == 0)
                AddBasisVectors();
            else
            {
                foreach (DWLine dwline in VectorLines)
                {
                    if (dwline.Type == LineType.BASE)
                        dwline.IsActive = true;
                }
            }
        }

        public void AddVector(double x, double y)
        {
            DWVector vector = VectorMath.GetVectorLocation(IHat, JHat, x, y);

            DWLine vectorLine = new DWLine
            (
                x, y, LineType.VECTOR,
                new Line
                {
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
            RotateNDegreesAntiClockwise(-90);
        }

        public void Rotate90DegreesAntiClockwise()
        {
            RotateNDegreesAntiClockwise(90);
        }

        // transformation methods
        public void TransformPlane(DWMatrix matrix)
        {
            DWMatrix newPlane = VectorMath.LinearTransformation(matrix, new DWMatrix(IHat, JHat));

            IHat.X = newPlane.IX;
            IHat.Y = newPlane.IY;
            JHat.X = newPlane.JX;
            JHat.Y = newPlane.JY;
            UpdateVectorLines();
        }

        public void ShearPlane()
        {
            TransformPlane(new DWMatrix(1, 0, 1, 1));
        }


        public void RevertToOriginal()
        {
            IHat.X = 1;
            IHat.Y = 0;
            JHat.X = 0;
            JHat.Y = 1;
            UpdateVectorLines();
        }

        // HELPER METHODS
        private SolidColorBrush PickRandomColor()
        {
            List<SolidColorBrush> colors = new List<SolidColorBrush>() {
                Brushes.Red,
                Brushes.Green,
                Brushes.Purple,
                Brushes.Orange,
                Brushes.DarkRed
            };

            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int choice = rand.Next(0, colors.Count);

            return colors[choice];
        }

        private void UpdateVectorLines()
        {

            List<DWLine> gridLines = VectorLines.Where(x => x.Type == LineType.GRID && x.Line.Y1 == 0).ToList();

            for (int i = 0, len = gridLines.Count; i < len; i++)
            {
                gridLines[i].Line.X1 = i * UnitLength + (UnitLength * IHat.X);
                gridLines[i].Line.X2 = i * UnitLength - (UnitLength * IHat.Y);
            }

            foreach (DWLine dwline in VectorLines.Where(x => x.Type == LineType.VECTOR || x.Type == LineType.BASE))
            {
                DWVector vector = VectorMath.GetVectorLocation(IHat, JHat, dwline.X, dwline.Y);
                dwline.Line.X2 = CanvasXOrigin + (vector.X * UnitLength);
                dwline.Line.Y2 = CanvasYOrigin - (vector.Y * UnitLength);
            }
        }

    }
}
