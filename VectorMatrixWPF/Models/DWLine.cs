using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace VectorMatrixWPF
{
    public class DWLine
    {

        public double X { get; set; }
        public double Y { get; set; }
        public Line Line { get; set; } = new Line();

        public DWLine(double x, double y, Line line)
        {
            X = x;
            Y = y;
            Line = line;
        }

    }
}
