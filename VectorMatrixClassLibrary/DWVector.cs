using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace VectorMatrixClassLibrary
{
    public class DWVector: INotifyPropertyChanged
    {
        private double _x;
        private double _y;
        public double X { get { return _x; } set { NotifyPropertyChanged(); _x = value; } }
        public double Y { get { return _y; } set { NotifyPropertyChanged(); _y = value; } }

        public DWVector () { }
        public DWVector (double x, double y) { X = x; Y = y; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
