using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VectorMatrixClassLibrary;

namespace VectorMatrixWPF.Models
{
    public class Animation: INotifyPropertyChanged
    {

        public bool AnimationEnabled { get; set; } = true;

        public ObservableCollection<Transformation> TransformationsList
            = new ObservableCollection<Transformation>();

        public Dictionary<string, double> Speeds { get; set; }
            = new Dictionary<string, double>()
            {
                { "Very Slow", 5 },
                { "Slow", 3 },
                { "Normal", 1 },
                { "Fast", .75 },
                { "Very Fast", .5 }
            };

        private const int ANIMATIONBASESPEED = 180;
        private double _animationFactor = 1;
        public double AnimationFactor
        {
            get { return _animationFactor; }
            set { _animationFactor = value; NotifyPropertyChanged(); }
        }

        public Animation(bool enabled = true)
        {
            AnimationEnabled = enabled;
        }

        public Animation(int speedFactor, bool enabled = true) : this(enabled)
        {
            AnimationFactor = speedFactor;
        }

        // PROPERTY CHANGED IMPLEMENTATION
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        public double GetFactoredSpeed() => ANIMATIONBASESPEED * AnimationFactor;

        public void UpdateLines(
            ref ObservableCollection<DWLine> vectorlines,
            ref ObservableCollection<DWLine> basislines,
            ref ObservableCollection<DWGridLine> dynamicLines,
            DWVector iHat, DWVector jHat,
            double xOrig, double yOrig, double unitLength)
        {
            foreach (DWLine dwline in vectorlines)
            {
                DWVector vector = VectorMath.GetNewVectorLocation(iHat, jHat, dwline.X, dwline.Y);
                dwline.Line.X2 = xOrig + (vector.X * unitLength);
                dwline.Line.Y2 = yOrig - (vector.Y * unitLength);
            }

            foreach (DWLine dwline in basislines)
            {
                DWVector vector = VectorMath.GetNewVectorLocation(iHat, jHat, dwline.X, dwline.Y);
                dwline.Line.X2 = xOrig + (vector.X * unitLength);
                dwline.Line.Y2 = yOrig - (vector.Y * unitLength);
            }

            foreach (DWGridLine gridline in dynamicLines)
            {
                DWVector vector1 = VectorMath.GetNewVectorLocation(iHat, jHat, gridline.Vector1.X, gridline.Vector1.Y);
                DWVector vector2 = VectorMath.GetNewVectorLocation(iHat, jHat, gridline.Vector2.X, gridline.Vector2.Y);

                gridline.DWLine.Line.X1 = xOrig + (vector1.X * unitLength);
                gridline.DWLine.Line.Y1 = yOrig - (vector1.Y * unitLength);

                gridline.DWLine.Line.X2 = xOrig + (vector2.X * unitLength);
                gridline.DWLine.Line.Y2 = yOrig - (vector2.Y * unitLength);
            }

        }

    }
}
