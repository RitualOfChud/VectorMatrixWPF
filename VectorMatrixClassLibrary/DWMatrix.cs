using System;
using System.Collections.Generic;
using System.Text;

namespace VectorMatrixClassLibrary
{
    public class DWMatrix
    {

        public double IX { get; set; }
        public double IY { get; set; }
        public double JX { get; set; }
        public double JY { get; set; }

        public DWMatrix(double iX, double iY, double jX, double jY)
        {
            IX = iX;
            IY = iY;
            JX = jX;
            JY = jY;
        }
    }
}
