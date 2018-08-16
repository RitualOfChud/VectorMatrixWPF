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
        /// <summary>
        /// The viewmodel to be used (holds the properties and functions of view)
        /// </summary>
        public SquareGrid Grid = new SquareGrid(10);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = Grid;

            // on load, create the gridlines and initialise canvas elements in view model
            ToggleButton tb = new ToggleButton() { IsChecked = false }; // set default togglestate to false
            Loaded += delegate
            {
                ToggleDynamicGridLines_Click(tb, new RoutedEventArgs());
                ToggleStaticGridLines_Click(tb, new RoutedEventArgs());
            };

            // on size change, reinitialise the canvas properties
            Plane.SizeChanged += delegate { Grid.InitialiseCanvasElement(Plane, new RoutedEventArgs()); };
        }

        // DISPLAY CLICK EVENTS

        /// <summary>
        /// Toggles the dynamic gridlines on and off, then draws any active vectors on currently on the plane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleDynamicGridLines_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                bool ischecked = tb.IsChecked == true;

                if (ischecked) Grid.ChangeGridLineState(true, false);
                else Grid.ChangeGridLineState(false, false);

                Grid.ShowActiveVectorLines(Plane);
            }
        }

        /// <summary>
        /// Toggles the static gridlines on and off, then draws any active vectors on currently on the plane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleStaticGridLines_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                bool ischecked = tb.IsChecked == true;

                if (ischecked) Grid.ChangeGridLineState(true, true);
                else Grid.ChangeGridLineState(false, true);

                Grid.ShowActiveVectorLines(Plane);
            }
        }

        /// <summary>
        /// Toggles the basis vectors on and off, then draws any active vectors on currently on the plane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        // ROTATION CLICK EVENTS

        /// <summary>
        /// Rotates the plane 90° clockwise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rotate90_ButtonClick(object sender, RoutedEventArgs e) => Grid.Rotate90DegreesClockwise();

        /// <summary>
        /// Rotates the plane 90° anticlockwise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rotate90Anti_ButtonClick(object sender, RoutedEventArgs e) => Grid.Rotate90DegreesAntiClockwise();

        /// <summary>
        /// Rotates the plane based on user-input
        /// Displays a messagebox error if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RotateNAnti_ButtonClick(object sender, RoutedEventArgs e)
        {
            bool canParse = double.TryParse(RotateN_TextBox.Text, out double x);

            if (!canParse)
                MessageBox.Show("Please enter a number", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                Grid.AnimateRotation(x);
        }

        // VECTOR CLICK EVENTS

        /// <summary>
        /// Adds a vector based on user-input
        /// Displays a messagebox error if input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Adds a random vector to anywhere on the plane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddRandomVector_ButtonClick(object sender, RoutedEventArgs e)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            Grid.AddVector(rand.Next(-Grid.MaxSize, Grid.MaxSize + 1), rand.Next(-Grid.MaxSize, Grid.MaxSize + 1));
            Grid.ShowActiveVectorLines(Plane);
        }

        // TRANSFORMATION CLICK EVENTS

        /// <summary>
        /// Shears the plane to the right
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shear_ButtonClick(object sender, RoutedEventArgs e) => Grid.ShearPlane();

        /// <summary>
        /// Performs a linear transformation based on a user-defined matrix
        /// Shows a messagebox error if the input is invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Reverts the plane to standard X/Y coords
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RevertToOriginal_Click(object sender, RoutedEventArgs e) => Grid.RevertToOriginal();

        private void ChangeAnimationFactor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string selected = ChangeSpeed_ComboBox.SelectedValue.ToString();
            Grid.AnimationFactor = Grid.Speeds[selected];
        }
        
        // KEYPRESS EVENTS

        /// <summary>
        /// On Enter keyup, will check which click-event to fire based on current keyboard focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FireButtonContextually_KeyUp(object sender, KeyEventArgs e)
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