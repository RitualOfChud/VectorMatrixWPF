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

using VectorMatrixWPF.Models;

namespace VectorMatrixWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public SquareGrid Graph = new SquareGrid(5);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = Graph;

            // resize events
            Loaded += delegate { Graph.ResizeCanvasElement(Chart_Canvas, new RoutedEventArgs()); };
            StateChanged += delegate { Graph.ResizeCanvasElement(Chart_Canvas, new RoutedEventArgs()); };
            Chart_Canvas.SizeChanged += Graph.ResizeCanvasElement;
        }

        private void AddVector_ButtonClick(object sender, RoutedEventArgs e)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Graph.AddVector(Chart_Canvas, rand.Next(-5, 6), rand.Next(-5, 6));
            Graph.ShowAllVectors(Chart_Canvas);
        }

        private void ShowGridLines_ButtonClick(object sender, RoutedEventArgs e) => Graph.DrawGridLines(Chart_Canvas);

        private void ShowBasisVectors_ButtonClick(object sender, RoutedEventArgs e)
        {
            Graph.ShowBasisVectors(Chart_Canvas);
            Graph.ShowAllVectors(Chart_Canvas);
        }

        private void Rotate90_ButtonClick(object sender, RoutedEventArgs e) => Graph.Rotate90DegreesClockwise();
    }
}