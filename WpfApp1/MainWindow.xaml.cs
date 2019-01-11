﻿using System;
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
        int colorCounter = 0;
        protected internal double MaxAvailableHeight { get; private set; }
        protected internal double MaxAvailableWidth { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = fc;
        }

        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            MaxAvailableHeight = canva.ActualHeight;
            MaxAvailableWidth = canva.ActualWidth;
            fc.Add(inputsT.Text.Split(',').ToList().Select(t => t.Trim()).ToList(), outputT.Text, fT.Text);
            Draw();
        }

        private void Draw()
        {
            canva.Children.Clear();
            int i = 0;
            for (i = 0; i < fc.chart.Count; i++)
            {
                if (fc.chart[i].Count == 0) break;
            }
            int numberofStacks = i > 7 ? i : 7;
            for (int j = 0; j < i; j++)
            {
                int numberofSlices = fc.chart[j].Count > 5 ? fc.chart[j].Count : 5;
                for (int k = 0; k < fc.chart[j].Count; k++)
                {
                    DrawElements(j, k, numberofSlices, numberofStacks,i);
                }
            }
        }
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
        private double slices(int i)
        {
            return fc.chart[i].Count > 5 ? fc.chart[i].Count : 5;
        }
        private void DrawElements(int j, int k, int slices1, int stacks, int numofcolumns)
        {

            Rectangle rect = new System.Windows.Shapes.Rectangle();
            Ellipse ellipse = new Ellipse();

            if (j % 2 == 0)
            {
                rect = new Rectangle()
                {
                    Name = fc.chart[j][k][0].ToString(),
                    Width = (canva.ActualWidth / stacks) / 2,
                    Height = (canva.ActualHeight / slices1) / 2,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                };
                canva.Children.Add(rect);
                rect.Margin = new Thickness((canva.ActualWidth / stacks) * (j), (canva.ActualHeight / slices1) * (k) + topAddOn(j), 0, 0);
            }
            else
            {
                ellipse = new Ellipse()
                {
                    Name = fc.chart[j][k][fc.chart[j][k].Count - 2].ToString(),
                    Width = (canva.ActualWidth / stacks) / 2,
                    Height = (canva.ActualHeight / slices1) / 2,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                };
                canva.Children.Add(ellipse);
                ellipse.Margin = new Thickness((canva.ActualWidth / stacks) * (j), (canva.ActualHeight / slices1) * (k) + topAddOn(j), 0, 0);
            }

            var textBlock = new TextBlock()
            {
                Name = j % 2 == 1 ? fc.chart[j][k][fc.chart[j][k].Count - 2].ToString() : fc.chart[j][k][0].ToString() + "tb",
                Text = j % 2 == 1 ? fc.chart[j][k][fc.chart[j][k].Count - 2].ToString() : fc.chart[j][k][0].ToString(),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            canva.Children.Add(textBlock);
            textBlock.Margin = new Thickness((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 5 - textBlock.ActualWidth, (canva.ActualHeight / slices1) * (k) + (canva.ActualHeight / slices1) / 6 + topAddOn(j), 0, 0);
            
            ///lines
            int z=0,y = 0;
            for (int x = 1; x <= fc.chart[j][k].Count - 1; x++)
            {
                for (y = 0; y <= numofcolumns; y++)
                {
                    bool isFound = false;
                    for (z = 0; z <= fc.chart[y].Count - 1; z++)
                    {
                        if (fc.chart[y][z][0].Equals(fc.chart[j][k][x]))
                        {
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound) break;

                }
                Drawlines(j, k,x, y, z, stacks);

            }
        }

        private void Drawlines(int j, int k, int x, int y, int z, int stacks)
        {
            colorCounter++;
            List<KnownColor> colors = Enum.GetValues(typeof(KnownColor))
                                      .Cast<KnownColor>()
                                      .ToList();
            Color color = new Color();
            color = Color.FromName(colors[colorCounter].ToString());
            Random rand = new Random();
            int distance = y - j,numOfInputs=0,originalNumOfInputs=0;
            double nextHeight = 0;
            List<Point> points = new List<Point>();
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 2,
                     (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j)) / 2 * x / (fc.chart[j][k].Count)));
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 2,
         (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j)) / 2 * x / fc.chart[j][k].Count));
            if (distance > 0)
            {

                for (int i = j; i < y; i++)
                {
                    if (i == y - 1)
                    {
                        if (y % 2 == 0)
                        {
                            List<string> L = new List<string>();
                            foreach (var item in fc.allFuncs)
                            {
                                if (item.Last().Equals(fc.chart[y][z][0]))
                                {
                                    L.Add(item[item.Count - 2]);
                                }
                                L.Add(fc.chart[y][z][0]);
                            }
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
                        points.Add(new Point(points[1].X + canva.ActualHeight / slices(y)/2 - canva.ActualHeight / slices(y) / 2 / (slices(y) + 1) * (z),
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
                    }
                    else
                    {
                        bool isfound = false;
                        for (int r = 0; r < fc.chart[i + 1].Count; r++)
                        {
                            if (points.Last().Y < (r + 1) * canva.ActualHeight / slices(i + 1) + topAddOn(i + 1))
                            {
                                nextHeight = r * canva.ActualHeight / slices(i + 1) -
                                           canva.ActualHeight / slices(i + 1) / 5 + topAddOn(i + 1);
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
                    System.Windows.Media.Color color1 = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    ////var b = GetBezierApproximation(points.ToArray(), 256);
                    PathFigure pf = new PathFigure(b.Points[0], new[] { b }, false);
                    PathFigureCollection pfc = new PathFigureCollection();
                    pfc.Add(pf);
                    var pge = new PathGeometry();
                    pge.Figures = pfc;
                    Path p = new Path();
                    p.Data = pge;
                    p.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                    canva.Children.Add(p);
                    //for (int v = 0; v < points.Count - 1; v++)
                    //{

                    //    var line = new Line()
                    //    {
                    //        X1 = points[v].X,
                    //        Y1 = points[v].Y,
                    //        X2 = points[v + 1].X,
                    //        Y2 = points[v + 1].Y,
                    //        Stroke = Brushes.Black,
                    //        StrokeThickness = 1
                    //    };
                    //    canva.Children.Add(line);
                    //}
                    var temp = points.Last();
                    var sec = new Point(points.Last().X + (canva.ActualWidth / stacks) * 1 / 4, points.Last().Y);
                    points.Clear();
                    points.Add(temp);
                    points.Add(sec);
                }
            }
        }


        PolyLineSegment GetBezierApproximation(Point[] controlPoints, int outputSegmentCount)
        {
            Point[] points = new Point[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
            }
            return new PolyLineSegment(points, true);
        }

        Point GetBezierPoint(double t, Point[] controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Point((1 - t) * P0.X + t * P1.X, (1 - t) * P0.Y + t * P1.Y);
        }




        private void In_Focused(object sender, RoutedEventArgs e)
        {
            inputsT.SelectAll();
        }
    }

}
