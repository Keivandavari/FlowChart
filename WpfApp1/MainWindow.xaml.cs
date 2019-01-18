using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Point = System.Windows.Point;
using Brushes = System.Windows.Media.Brushes;
using Rectangle = System.Windows.Shapes.Rectangle;
using System.Drawing;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Flowchart fc = new Flowchart();
        // count for color to get different colors for connections.
        int colorCounter = 0;
        // StrokeThickness of the connection lines.
        int strokeThickness=1;
        protected internal double MaxAvailableHeight { get; private set; }
        protected internal double MaxAvailableWidth { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = fc;
        }

        /// <summary>
        /// This is when user click on the ADD button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            // This is the Max Width and Height of the Chart UI. canva is the Canvas control that we Draw the flowchart on it.
            MaxAvailableHeight = canva.ActualHeight;
            MaxAvailableWidth = canva.ActualWidth;
            // Send the User's Inputs To our FlowChart class.
            fc.Add(inputsT.Text.Split(',').ToList().Select(t => t.Trim()).ToList(), outputT.Text, fT.Text);
            Draw();
        }

        private void Draw()
        {
            colorCounter = 0;
            canva.Children.Clear();
            int i = 0;
            //Find the Exact number of our columns or stacks in the our chart.
            for (i = 0; i < fc.chart.Count; i++)
            {
                if (fc.chart[i].Count == 0) break;
            }
            // If number of stacks bigger than 7 then we devide the page to i otherwise we devide it to 7.
            int numberofStacks = i > 7 ? i : 7;
            for (int j = 0; j < i; j++)
            {
                // We also devide each column to slices.
                int numberofSlices = fc.chart[j].Count > 5 ? fc.chart[j].Count : 5;
                for (int k = 0; k < fc.chart[j].Count; k++)
                {
                    DrawElements(j, k, numberofSlices, numberofStacks,i);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="j"></param>
        /// The column number of our current(First) object.
        /// <param name="k"></param>
        /// The slice number of our current(First) object.
        /// <param name="numberofSlices"></param>
        /// <param name="numberofStacks"></param>
        /// <param name="numofcolumns"></param>
        private void DrawElements(int j, int k, int numberofSlices, int numberofStacks, int numofcolumns)
        {
            string name = j % 2 == 1 ? fc.chart[j][k][fc.chart[j][k].Count - 2].Split('-')[0].ToString() : fc.chart[j][k][0].Split('-')[0].ToString();
            //Here is the textblock for showing objects name 
            var textBlock = new TextBlock()
            {
                Text = name,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Foreground = j % 2 == 0 ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#3E50B4")) :
                                       (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3F80")),
                FontFamily = new System.Windows.Media.FontFamily("SegoeUI"),
                FontWeight = FontWeights.Light,
                FontSize = 24,
            };
            canva.Children.Add(textBlock);
            textBlock.Margin = new Thickness((canva.ActualWidth / numberofStacks) * (j), (canva.ActualHeight / numberofSlices) * (k) + topAddOn(j), 0, 0);
            /// Here we go through chart objects to see where objects should be connected to.
            /// when we find the right object we call drawConnection with correspandant indexes.
            int z = 0, y = 0, t;
            for (int x = 1; x <= fc.chart[j][k].Count - 1; x++)
            {
                for (y = 0; y <= numofcolumns; y++)
                {
                    for (z = 0; z <= fc.chart[y].Count - 1; z++)
                    {
                        if (fc.chart[y][z][0].Equals(fc.chart[j][k][x]))
                        {
                            DrawConnection(j, k, x, y, z, numberofStacks);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// We Draw the connection here. We start from the first object and we try to 
        /// go between all the items between the first and second object and connect them.
        /// </summary>
        /// <param name="j"></param>
        /// it is column number of the first item.
        /// <param name="k"></param>
        /// it is slice number of the first item.
        /// <param name="x"></param>
        /// it is the index of the first object in the last list.
        /// <param name="y"></param>
        /// it is column number of the last item.
        /// <param name="z"></param>
        /// it is slice number of the last item.
        /// <param name="stacks"></param>
        private void DrawConnection(int j, int k, int x, int y, int z, int stacks)
        {
            strokeThickness = 1;
            Random rand = new Random();
            /// it is distance between two columns that has objects should be connected
            int distance = y - j;
            int numOfInputs=0,originalNumOfInputs=0,temp1;
            double nextHeight = 0;
            List<Point> points = new List<Point>();
            //if distance is negative we make it positive and change the strokethickness of the line to 5 to make it difference than others.
            // it happens when the output of the function is in columns before the inputs are.
            if (distance < 0)
            {
                temp1 = y;
                y = j;
                j = temp1;
                temp1 = z;
                z = k;
                k = temp1;
                strokeThickness = 4;
            }
            /// Here is the first points of the connetion
            /// we add them based on the position of the object which we draw connection for.
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks)*2/5,
                     (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j))/4));
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 2,
                     (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j)) / 2 * x / (fc.chart[j][k].Count)));
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 2,
                     (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j)) / 2 * x / fc.chart[j][k].Count));
                for (int i = j; i < y; i++)
                {
                    if (i > j) colorCounter--;

                /// when we start to draw and we get to the column befor the last column.
                /// we act different here. all previous columns needed to pass between objects but this one needs to get connected to the destination object of the connection.
                if (i == y - 1)
                {

                    if (y % 2 == 0)
                    {
                        List<string> L = new List<string>();
                        foreach (var item in fc.allFuncs)
                        {
                            if (item[item.Count - 1].Equals(fc.chart[y][z][0]))
                            {
                                L.Add(item[item.Count - 3]);
                            }
                            L.Add(fc.chart[y][z][0]);
                        }
                        // these calculations are for separating the end point of each line.
                        // we devide each object height by number of connections that supposed to be connected to that object.
                        // Then for each entry we make different height.
                        //**************************************************************
                        originalNumOfInputs = L.Count;
                        numOfInputs = L.Count;
                        int cIndex = L.IndexOf(fc.chart[j][k][0]);
                        if (numOfInputs == 1) nextHeight = (canva.ActualHeight / slices(y)) * (z) + topAddOn(y) + (canva.ActualHeight / slices(y)) / 2;
                        else nextHeight = (canva.ActualHeight / slices(y)) * (z) + topAddOn(y) + (canva.ActualHeight / slices(y)) * cIndex / (numOfInputs - 1) / 2;
                    }
                    else
                    {
                        originalNumOfInputs = fc.allFuncs.FirstOrDefault(s => s.Contains(fc.chart[y][z][0])).Count;
                        numOfInputs = fc.allFuncs.FirstOrDefault(s => s.Contains(fc.chart[y][z][0])).Count - 2;
                        int cIndex = fc.allFuncs.FirstOrDefault(s => s.Contains(fc.chart[y][z][0])).IndexOf(fc.chart[j][k][0]);
                        if (numOfInputs == 1) nextHeight = (canva.ActualHeight / slices(y)) * (z) + topAddOn(y) + (canva.ActualHeight / slices(y)) / 2;
                        else nextHeight = (canva.ActualHeight / slices(y)) * (z) + topAddOn(y) + (canva.ActualHeight / slices(y)) * cIndex / (numOfInputs - 1) / 2;

                    }
                    //*****************************************************************

                    // these are all points we need to draw the curve in the last step of drawing the curve.

                    points.Add(new Point(points[1].X + canva.ActualHeight / slices(y) / 2 - canva.ActualHeight / slices(y) / 2 / (slices(y) + 1) * (z),
                    points[1].Y));
                    points.Add(new Point(points[2].X,
                    nextHeight));
                    points.Add(new Point((canva.ActualWidth / stacks) * ((y)) - (canva.ActualWidth / stacks) / 10,
                    nextHeight));
                    if (originalNumOfInputs != 1)
                    {
                        points.Add(new Point((canva.ActualWidth / stacks) * ((y)) - (canva.ActualWidth / stacks) / 40,
                        (canva.ActualHeight / slices(y)) * (z) + topAddOn(y) + (canva.ActualHeight / slices(y)) / 4));
                        points.Add(new Point((canva.ActualWidth / stacks) * ((y)),
                        (canva.ActualHeight / slices(y)) * (z) + topAddOn(y) + (canva.ActualHeight / slices(y)) / 4));
                    }
                    points.Add(new Point((canva.ActualWidth / stacks) * (y) + (canva.ActualWidth / stacks) / 8,
                        (canva.ActualHeight / slices(y)) * (z) + topAddOn(y) + (canva.ActualHeight / slices(y)) / 4));
                }
                // Here is when we are passing through columns and we have not got to the last column.
                // The nextHeight parameter is the height for next step of drawing to pass between the ovjects.
                else
                {
                    bool isfound = false;
                    for (int r = 0; r < 2 * fc.chart[i + 1].Count; r++)
                    {
                        if (points.Last().Y < (r + 1) * (canva.ActualHeight / slices(i + 1)) / 2 + topAddOn(i + 1))
                        {
                            if (r % 2 == 0) nextHeight = (r + 1) * canva.ActualHeight / slices(i + 1) / 2 + topAddOn(i + 1) - canva.ActualHeight / slices(i + 1) / 2 < points.Last().Y ? r * canva.ActualHeight / slices(i + 1) / 2 +
                                            canva.ActualHeight / slices(i + 1) + topAddOn(i + 1) : (r + 1) * canva.ActualHeight / slices(i + 1) / 2 + topAddOn(i + 1) / 2;
                            else nextHeight = points.Last().Y;
                            isfound = true;
                            break;
                        }
                    }
                    if (!isfound) nextHeight = points.Last().Y;
                    points.Add(new Point(points[1].X + (canva.ActualWidth / stacks) * 1 / 4,
                    points[1].Y));
                    points.Add(new Point(points[2].X,
                    nextHeight));
                    points.Add(new Point(points[3].X + (canva.ActualWidth / stacks) * 1 / 2,
                    nextHeight));
                }
                // We have made a lists of points we want to pass through 
                // now it is time to call the MakeCubicCurve with the points.
                MakeCubicCurve(points.ToArray());
                //Last point of each step should be first one of the next one so here at the end
                // we insert the last one to the new list and do the process again for the next column
                // also we know what is the second point would be so add it here.
                var first = points.Last();
                var second = new Point(points.Last().X + (canva.ActualWidth / stacks) * 1 / 4, points.Last().Y);
                    points.Clear();
                    points.Add(first);
                    points.Add(second);
                }
            
        }

        /// <summary>
            /// Making the curve.
            /// </summary>
            /// <param name="points"></param>
            /// It is a list that contains points for connecting  two objects in the flowchart.
        private void MakeCubicCurve(Point[] points)
        {
            if (colorCounter > Helpers.indexcolors.Count() - 1) colorCounter = 0;
            var b = Helpers.GetBezierApproximation(points, 256);
            PathFigure pf = new PathFigure(b.Points[0], new[] { b }, false);
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pf);
            var pge = new PathGeometry();
            pge.Figures = pfc;
            Path p = new Path();
            p.Data = pge;
            p.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom(Helpers.indexcolors[++colorCounter]));
            p.StrokeThickness = strokeThickness;
            canva.Children.Add(p);
        }


        private void In_Focused(object sender, RoutedEventArgs e)
        {
            inputsT.SelectAll();
        }


        // for stacks that have less than 5 objects for better visualization we add some height to their objects.
        // we try to keep objects at the center.
        private double topAddOn(int i)
        {
            if (fc.chart[i].Count <= 5)
            {
                return (5 / 2) * (canva.ActualHeight / 5) - ((fc.chart[i].Count) / 2 - 1 / 4) * (canva.ActualHeight / 5);
            }
            else
            {
                return 0;
            }
        }
        // deviding columns to some slices to find coordinate of each object easier.
        private double slices(int i)
        {
            return fc.chart[i].Count > 5 ? fc.chart[i].Count : 5;
        }

        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }
    }

}
