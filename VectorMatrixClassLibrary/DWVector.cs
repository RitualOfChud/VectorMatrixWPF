using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace VectorMatrixClassLibrary
{
    public class DWVector
    {
        public double X { get; set; }
        public double Y { get; set; }

        public DWVector () { }
        public DWVector (double x, double y) { X = x; Y = y; }
    }
}
