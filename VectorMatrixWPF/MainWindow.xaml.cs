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
            ToggleButton tb = new ToggleButton() { IsChecked = false };


            DataContext = Grid;

            // on load events
            Loaded += delegate
            {
                ToggleGridLines_Click(tb, new RoutedEventArgs());
                ToggleOriginalGridLines_Click(tb, new RoutedEventArgs());
                Grid.ResizeCanvasElement(Plane, new RoutedEventArgs());
            };
            Plane.SizeChanged += delegate { Grid.ResizeCanvasElement(Plane, new RoutedEventArgs()); };
        }

        // CLICK EVENTS

        // display
        private void ToggleGridLines_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                bool ischecked = tb.IsChecked == true;

                if (ischecked) Grid.ChangeGridLineState(true, false);
                else Grid.ChangeGridLineState(false, false);

                Grid.ShowActiveVectorLines(Plane);
            }
        }

        private void ToggleOriginalGridLines_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                bool ischecked = tb.IsChecked == true;

                if (ischecked) Grid.ChangeGridLineState(true, true);
                else Grid.ChangeGridLineState(false, true);

                Grid.ShowActiveVectorLines(Plane);
            }
        }

        private void ToggleBasisVectors_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                bool ischecked = tb.IsChecked == true;

                if (ischecked) Grid.ActivateBasisVectors();
                else
                {
                    foreach (DWLine dwline in Grid.VectorLines.Where(x => x.Type == LineType.BASE))
                    {
                        dwline.IsActive = false;
                    }
                }
            }

            Grid.ShowActiveVectorLines(Plane);
        }

        private void RevertToOriginal_Click(object sender, RoutedEventArgs e) => Grid.RevertToOriginal();

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
                MessageBox.Show($"Please enter a number between {-Grid.MaxSize} and {Grid.MaxSize}", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                Grid.AddVector(x, y);
                Grid.ShowActiveVectorLines(Plane);
                XVector_TextBox.Text = "";
                YVector_TextBox.Text = "";
            }
        }

        private void AddRandomVector_ButtonClick(object sender, RoutedEventArgs e)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Grid.AddVector(rand.Next(-Grid.MaxSize, Grid.MaxSize + 1), rand.Next(-Grid.MaxSize, Grid.MaxSize + 1));
            Grid.ShowActiveVectorLines(Plane);
        }

        // transformation
        private void Shear_ButtonClick(object sender, RoutedEventArgs e) => Grid.ShearPlane();

        private void LinearTransformation_ButtonClick(object sender, RoutedEventArgs e)
        {
            bool ixParsable = double.TryParse(IXMatrix_TextBox.Text, out double ix);
            bool iyParsable = double.TryParse(IYMatrix_TextBox.Text, out double iy);
            bool jxParsable = double.TryParse(JXMatrix_TextBox.Text, out double jx);
            bool jyParsable = double.TryParse(JYMatrix_TextBox.Text, out double jy);

            if (!ixParsable || !iyParsable || !jxParsable || !jyParsable)
                MessageBox.Show("Please enter numbers only", "Input Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                Grid.TransformPlane(new VectorMatrixClassLibrary.DWMatrix(ix, iy, jx, jy));
                IXMatrix_TextBox.Text = "";
                IYMatrix_TextBox.Text = "";
                JXMatrix_TextBox.Text = "";
                JYMatrix_TextBox.Text = "";
            }

        }

        // contextual enter key-check
        private void RunButtonContextually_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (XVector_TextBox.IsKeyboardFocused || YVector_TextBox.IsKeyboardFocused)
            {
                AddVector_ButtonClick(new object(), new RoutedEventArgs());
                Keyboard.Focus(NewVector_Button);
            }

            if (IXMatrix_TextBox.IsKeyboardFocused || IYMatrix_TextBox.IsKeyboardFocused || JXMatrix_TextBox.IsKeyboardFocused || JYMatrix_TextBox.IsKeyboardFocused)
            {
                LinearTransformation_ButtonClick(new object(), new RoutedEventArgs());
                Keyboard.Focus(NewTransformation_Button);
            }
            if (RotateN_TextBox.IsKeyboardFocused)
            {
                RotateNAnti_ButtonClick(new object(), new RoutedEventArgs());
                Keyboard.Focus(DegreeNAnticlockwise_Button);
            }

        }
    }
}