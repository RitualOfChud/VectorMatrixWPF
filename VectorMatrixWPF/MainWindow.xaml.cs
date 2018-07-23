using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VectorMatrixWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public double _canvasHeight;
        public double CanvasHeight
        {
            get { return _canvasHeight; }
            set { NotifyPropertyChanged(); _canvasHeight = value; }
        }

        public double _canvasWidth;
        public double CanvasWidth
        {
            get { return _canvasWidth; }
            set { NotifyPropertyChanged(); _canvasWidth = value; }
        }

        public double _canvasHalfWidth;
        public double CanvasHalfWidth
        {
            get { return _canvasHalfWidth; }
            set { NotifyPropertyChanged(); _canvasHalfWidth = value; }
        }

        public double _canvasHalfHeight;
        public double CanvasHalfHeight
        {
            get { return _canvasHalfHeight; }
            set { NotifyPropertyChanged(); _canvasHalfHeight = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // resize events
            Loaded += delegate { ResizeCanvas(new object(), new RoutedEventArgs()); };
            StateChanged += delegate { ResizeCanvas(new object(), new RoutedEventArgs()); };
            Chart_Canvas.SizeChanged += ResizeCanvas;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ResizeCanvas(object sender, RoutedEventArgs e)
        {
            CanvasHeight = Chart_Canvas.ActualHeight;
            CanvasWidth = Chart_Canvas.ActualWidth;
            CanvasHalfWidth = CanvasWidth / 2;
            CanvasHalfHeight = CanvasHeight / 2;
            Console.WriteLine("");
        }

    }
}