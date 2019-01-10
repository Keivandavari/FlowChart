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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Flowchart fc = new Flowchart();
        double topaddOn, topaddOnEnd;
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
            int numberofStacks = i > 11 ? i : 11;
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

            Rectangle rect = new Rectangle();
            Ellipse ellipse = new Ellipse();
            if (fc.chart[j].Count <= 5)
            {
                topaddOn = (5 / 2) * (canva.ActualHeight / 5) - ((fc.chart[j].Count) / 2 - 1 / 4) * (canva.ActualHeight / 5);
            }
            else
            {
                topaddOn = 0;
            }
            

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
                            if (fc.chart[y].Count <= 5)
                            {
                                topaddOnEnd = (5 / 2) * (canva.ActualHeight / 5) - ((fc.chart[y].Count) / 2 - 1 / 4) * (canva.ActualHeight / 5);
                            }
                            else
                            {
                                topaddOnEnd = 0;
                            }
                            break;
                        }
                    }
                    if (isFound) break;

                }

                Drawlines(j, k, y, z, stacks);
                //var line = new Line()
                //{
                //    X1 = (canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 2,
                //    Y1 = (canva.ActualHeight / slices(j)) * (k) + topaddOn + (canva.ActualHeight / slices(j)) / 4,
                //    X2 = (canva.ActualWidth / stacks) * ((y)),
                //    Y2 = (canva.ActualHeight / slices(y)) * (z) + topaddOnEnd + (canva.ActualHeight / slices(y)) / 4,
                //    Stroke = Brushes.Black,
                //    StrokeThickness = 1
                //};
                //canva.Children.Add(line);

            }
        }

        private void Drawlines(int j, int k, int y, int z, int stacks)
        {
            Random rand = new Random();
            Color color = Color.FromArgb(Convert.ToByte(rand.Next(256)), Convert.ToByte(rand.Next(256)),
                            Convert.ToByte(rand.Next(256)), Convert.ToByte(rand.Next(256)));
            int distance = y - j;
            double nextHeight = 0;
            List<Point> points = new List<Point>();
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 2,
                     (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j)) / 4));
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks)/ 2,
         (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j)) / 4));
            if (distance > 0)
            {

                for (int i = j; i < y; i++)
                {
                    if (i == y - 1)
                    {
                        nextHeight = (canva.ActualHeight / slices(y)) * (z) + topaddOnEnd + (canva.ActualHeight / slices(y)) / 4;
                        points.Add(new Point(points[1].X + canva.ActualHeight / slices(y) / (slices(y) + 1) * (z+1),
                        points[1].Y));
                        points.Add(new Point(points[2].X,
                        nextHeight));
                        points.Add(new Point((canva.ActualWidth / stacks) * ((y)),
                        nextHeight));
                    }
                    else
                    {
                        bool isfound = false;
                        for (int r = 0; r < fc.chart[i + 1].Count; r++)
                        {
                            if (points.Last().Y < r * canva.ActualHeight / slices(i + 1) + topAddOn(i + 1))
                            {
                                nextHeight = r * canva.ActualHeight / slices(i + 1) -
                                           canva.ActualHeight / slices(i + 1) / 5 + topAddOn(i + 1);
                                isfound = true;
                                break;
                            }

                        }
                        if (!isfound) nextHeight = (canva.ActualHeight / slices(i + 1)) * fc.chart[i+1].Count + topAddOn(i+1);
                        points.Add(new Point(points[1].X + (canva.ActualWidth / stacks) * 1 / 4,
                        points[1].Y));
                        points.Add(new Point(points[2].X,
                        nextHeight));
                        points.Add(new Point(points[3].X + (canva.ActualWidth / stacks) * 1 / 2,
                        nextHeight));
                    }
                    for (int v = 0; v < points.Count - 1; v++)
                    {
                        var line = new Line()
                        {
                            X1 = points[v].X,
                            Y1 = points[v].Y,
                            X2 = points[v + 1].X,
                            Y2 = points[v + 1].Y,
                            Stroke = Brushes.Black,
                            StrokeThickness = 1
                        };
                        canva.Children.Add(line);
                    }
                    var temp = points.Last();
                    var sec = new Point(points.Last().X + (canva.ActualWidth / stacks) * 1 / 4, points.Last().Y);
                    points.Clear();
                    points.Add(temp);
                    points.Add(sec);
                }

               
            }
        }

        private void In_Focused(object sender, RoutedEventArgs e)
        {
            inputsT.SelectAll();
        }
    }

}
