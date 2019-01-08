using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Flowchart fc = new Flowchart();
        List<Tuple<List<string>, Point, Point>> portAxis = new List<Tuple<List<string>, Point, Point>>();
        double topaddOn, topaddOnEnd;
        protected internal double MaxAvailableHeight { get; private set; }
        protected internal double MaxAvailableWidth { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            //MaxAvailableHeight = canva.ActualHeight;
            //MaxAvailableWidth = canva.ActualWidth;
            Grid stackGrid = new Grid()
            {
                //Name = "stackGrid" + j.ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                //Width = (MaxAvailableWidth / stacks),
                //Margin = new Thickness((MaxAvailableWidth / stacks) * (j), 0, 0, 0)
            };

        }

        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            MaxAvailableHeight = canva.ActualHeight;
            MaxAvailableWidth = canva.ActualWidth;
            fc.Add(inputsT.Text.Split(' ').ToList(), outputT.Text, fT.Text);
            Draw();
            //diagram.InvalidateVisual();
            //((MainWindow)System.Windows.Application.Current.MainWindow).UpdateLayout();
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
                    DrawElement(j, k, numberofSlices, numberofStacks);
                    //DrawLines(i,j,k, numberofStacks);

                }
            }
        }

        private void DrawLines(int i, int j, int k, int stacks)
        {
            int slicesend = fc.chart[j + 1].Count > 5 ? fc.chart[j + 1].Count : 5;
            int slicesstart = fc.chart[j].Count > 5 ? fc.chart[j].Count : 5;
            int y = 0;
            for (int x = 1; x <= fc.chart[j][k].Count - 1; x++)
            {
                for(y=0; y<= fc.chart[j+1].Count -1;y++)
                {
                    if (fc.chart[j + 1][y][0].Equals(fc.chart[j][k][0])) break;
                }
                var line = new Line()
                {
                    X1 = (canva.ActualWidth / stacks) * (j + 1 / 4) + (canva.ActualWidth / stacks) / 2,
                    Y1 = (canva.ActualHeight / slicesstart) * (k) + topaddOn + (canva.ActualHeight / slicesstart) / 4,
                    X2 = (canva.ActualWidth / stacks) * ((j + 1) + 1 / 4),
                    Y2 = (canva.ActualHeight / slicesend) * (y) + topaddOn + (canva.ActualHeight / slicesend) / 4,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                canva.Children.Add(line);

            }
        }

        private void DrawElement(int j, int k, int slices, int stacks)
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
            if (fc.chart[j+1].Count <= 5)
            {
                topaddOnEnd = (5 / 2) * (canva.ActualHeight / 5) - ((fc.chart[j+1].Count) / 2 - 1 / 4) * (canva.ActualHeight / 5);
            }
            else
            {
                topaddOnEnd = 0;
            }

            if (j % 2 == 0)
            {
                rect = new Rectangle()
                {
                    Name = fc.chart[j][k][0].ToString(),
                    Width = (canva.ActualWidth / stacks) / 2,
                    Height = (canva.ActualHeight / slices) / 2,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                };
                canva.Children.Add(rect);
                rect.Margin = new Thickness((canva.ActualWidth / stacks) * (j + 1 / 4), (canva.ActualHeight / slices) * (k) + topaddOn, 0, 0);
            }
            else
            {
                ellipse = new Ellipse()
                {
                    Name = fc.chart[j][k][fc.chart[j][k].Count - 2].ToString(),
                    Width = (canva.ActualWidth / stacks) / 2,
                    Height = (canva.ActualHeight / slices) / 2,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                };
                canva.Children.Add(ellipse);
                ellipse.Margin = new Thickness((canva.ActualWidth / stacks) * (j + 1 / 4), (canva.ActualHeight / slices) * (k) + topaddOn, 0, 0);
                //portAxis.Add(new Tuple<List<string>, Point, Point>(fc.chart[j][k],
                //    new Point((canva.ActualWidth / stacks) * (j + 1 / 4),
                //   (canva.ActualHeight / slices) * (k) + topaddOn + (canva.ActualHeight / slices) / 4),
                //    new Point((canva.ActualWidth / stacks) * (j + 1 / 4) + (canva.ActualHeight / slices) / 2,
                //    (canva.ActualHeight / slices) * (k) + topaddOn + (canva.ActualHeight / slices) / 4)));
            }
            var textBlock = new TextBlock()
            {
                Name = j % 2 == 1 ? fc.chart[j][k][fc.chart[j][k].Count - 2].ToString() : fc.chart[j][k][0].ToString() + "tb",
                Text = j % 2 == 1 ? fc.chart[j][k][fc.chart[j][k].Count - 2].ToString() : fc.chart[j][k][0].ToString(),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            canva.Children.Add(textBlock);
            textBlock.Margin = new Thickness((canva.ActualWidth / stacks) * (j + 1 / 4) + (canva.ActualWidth / stacks) / 5 - textBlock.ActualWidth, (canva.ActualHeight / slices) * (k) + (canva.ActualHeight / slices) / 6 + topaddOn, 0, 0);
            int slicesend = fc.chart[j + 1].Count > 5 ? fc.chart[j + 1].Count : 5;
            int slicesstart = fc.chart[j].Count > 5 ? fc.chart[j].Count : 5;
            int y = 0;
            for (int x = 1; x <= fc.chart[j][k].Count - 1; x++)
            {
                for (y = 0; y <= fc.chart[j + 1].Count - 1; y++)
                {
                    if (fc.chart[j + 1][y][0].Equals(fc.chart[j][k][x])) break;
                }
                y = y > 0 ? y : 0;
                var line = new Line()
                {
                    X1 = (canva.ActualWidth / stacks) * (j + 1 / 4) + (canva.ActualWidth / stacks) / 2,
                    Y1 = (canva.ActualHeight / slicesstart) * (k) + topaddOn + (canva.ActualHeight / slicesstart) / 4,
                    X2 = (canva.ActualWidth / stacks) * ((j + 1) + 1 / 4),
                    Y2 = (canva.ActualHeight / slicesend) * (y) + topaddOnEnd + (canva.ActualHeight / slicesend) / 4,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                canva.Children.Add(line);

            }
        }
        private void In_Focused(object sender, RoutedEventArgs e)
        {
            inputsT.SelectAll();
        }
    }

}
