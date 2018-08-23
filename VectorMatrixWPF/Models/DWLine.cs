using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VectorMatrixWPF
{
    public class DWLine
    {
        public double X { get; set; }
        public double Y { get; set; }
        public LineType Type { get; set; }
        public Line Line { get; set; } = new Line();
        public bool IsActive { get; set; } = true;
        public SolidColorBrush Color { get; set; }

        public DWLine(double x, double y, LineType type, Line line, SolidColorBrush color, bool active = true)
        {
            X = x;
            Y = y;
            Type = type;
            Line = line;
            IsActive = active;
            Color = color;
        }
    }
}
