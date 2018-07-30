using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        public SquareGrid Grid = new SquareGrid();

        public MainWindow()
        {
            InitializeComponent();

            Grid.SetGridSize(10);

            DataContext = Grid;

            // resize events
            Loaded += delegate { Grid.ResizeCanvasElement(Plane, new RoutedEventArgs()); };
            StateChanged += delegate { Grid.ResizeCanvasElement(Plane, new RoutedEventArgs()); };
            Plane.SizeChanged += Grid.ResizeCanvasElement;
        }

        // CLICK EVENTS
        
        // display
        private void ShowGridLines_ButtonClick(object sender, RoutedEventArgs e) => Grid.DrawGridLines(Plane);
        private void ShowBasisVectors_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                bool ischecked = tb.IsChecked == true;

                if (ischecked)
                { 
                    Grid.ShowBasisVectors(Plane);
                }
                else
                { 
                    foreach(DWLine dwline in Grid.BasisVectorLines)
                    {
                        Plane.Children.Remove(dwline.Line);
                    }
                    Grid.BasisVectorLines.Clear();
                }
            }
            
            Grid.ShowAllVectors(Plane);
        }

        // rotation
        private void Rotate90_ButtonClick(object sender, RoutedEventArgs e) => Grid.Rotate90DegreesClockwise();
        private void Rotate90Anti_ButtonClick(object sender, RoutedEventArgs e) => Grid.Rotate90DegreesAntiClockwise();
        private void RotateNAnti_ButtonClick(object sender, RoutedEventArgs e)
        {
            bool canParse = double.TryParse(RotateN_TextBox.Text, out double x);

            if (!canParse)
                MessageBox.Show("Please enter a number", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                Grid.RotateNDegreesAntiClockwise(x);
        }

        // vectors
        private void AddVector_ButtonClick(object sender, RoutedEventArgs e)
        {
            bool xParse = double.TryParse(XVector_TextBox.Text, out double x);
            bool yParse = double.TryParse(YVector_TextBox.Text, out double y);

            if (!xParse || !yParse || x > Grid.MaxSize || x < -Grid.MaxSize || y > Grid.MaxSize || y < -Grid.MaxSize)
                MessageBox.Show($"Please enter numbers between {-Grid.MaxSize} and {Grid.MaxSize}", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                Grid.AddVector(Plane, x, y);
                Grid.ShowAllVectors(Plane);
            }
        }

        private void AddRandomVector_ButtonClick(object sender, RoutedEventArgs e)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Grid.AddVector(Plane, rand.Next(-Grid.MaxSize, Grid.MaxSize + 1), rand.Next(-Grid.MaxSize, Grid.MaxSize + 1));
            Grid.ShowAllVectors(Plane);
        }

        // transformation
        private void LinearTransformation_ButtonClick(object sender, RoutedEventArgs e)
        {
            Grid.TransformPlane(Plane);
            Grid.ShowAllVectors(Plane);
        }

    }
}