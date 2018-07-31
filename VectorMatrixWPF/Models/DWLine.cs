using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace VectorMatrixWPF
{
    public class DWLine: INotifyPropertyChanged
    {
        public double X { get; set; }
        public double Y { get; set; }
        public LineType Type { get; set; }
        public Line Line { get; set; } = new Line();

        private bool _isActive = true;
        public bool IsActive { get { return _isActive; } set { NotifyPropertyChanged(); _isActive = value; } }

        public DWLine(double x, double y, LineType type, Line line, bool active = true)
        {
            X = x;
            Y = y;
            Type = type;
            Line = line;
            IsActive = active;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
