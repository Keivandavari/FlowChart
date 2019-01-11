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
            int n = 0;
            MaxAvailableHeight = canva.ActualHeight;
            MaxAvailableWidth = canva.ActualWidth;
            fc.Add(inputsT.Text.Split(',').ToList().Select(t => t.Trim()).ToList(), outputT.Text, fT.Text);
            Draw();
        }

        private void Draw()
        {
            colorCounter = 0;
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
                //List<Point> points = new List<Point>();
                //points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 4,
                //                     (canva.ActualHeight / slices1) * (k) + topAddOn(j)));
                //points.Add(new Point((canva.ActualWidth / stacks) * (j),
                //                     (canva.ActualHeight / slices1) * (k) + topAddOn(j) + (canva.ActualHeight / slices1) / 4));
                //points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 4,
                //                     (canva.ActualHeight / slices1) * (k) + topAddOn(j) + (canva.ActualHeight / slices1) / 2));
                //points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 2,
                //     (canva.ActualHeight / slices1) * (k) + topAddOn(j) + (canva.ActualHeight / slices1) / 4));
                //MakeCubicCurve(points.ToArray());
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
                //canva.Children.Add(rect);
                //rect.Margin = new Thickness((canva.ActualWidth / stacks) * (j), (canva.ActualHeight / slices1) * (k) + topAddOn(j), 0, 0);
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
                //canva.Children.Add(ellipse);
                //ellipse.Margin = new Thickness((canva.ActualWidth / stacks) * (j), (canva.ActualHeight / slices1) * (k) + topAddOn(j), 0, 0);
            }

            var textBlock = new TextBlock()
            {
                Name = j % 2 == 1 ? fc.chart[j][k][fc.chart[j][k].Count - 2].ToString() : fc.chart[j][k][0].ToString() + "tb",
                Text = j % 2 == 1 ? fc.chart[j][k][fc.chart[j][k].Count - 2].ToString() : fc.chart[j][k][0].ToString(),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Foreground = j%2==0 ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#3E50B4")):
                                       (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3F80")),
                FontFamily = new System.Windows.Media.FontFamily("SegoeUI"),
                FontWeight = FontWeights.Light,
                FontSize = 24,
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
            //colorCounter++;
            //List<KnownColor> colors = Enum.GetValues(typeof(KnownColor))
                                      //.Cast<KnownColor>()
                                      //.ToList();
            //Color color = new Color();
            //color = Color.FromName(colors[colorCounter].ToString());
            Random rand = new Random();
            int distance = y - j,numOfInputs=0,originalNumOfInputs=0;
            double nextHeight = 0;
            List<Point> points = new List<Point>();
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 8,
            (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) +(canva.ActualWidth / stacks)/4+ (canva.ActualHeight / slices(j)) / 2 * x / (fc.chart[j][k].Count)));
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks)/4,
                    (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j))/4 + (canva.ActualHeight / slices(j)) / 2 * x / (fc.chart[j][k].Count)));
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 2,
                     (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j)) / 2 * x / (fc.chart[j][k].Count)));
            points.Add(new Point((canva.ActualWidth / stacks) * (j) + (canva.ActualWidth / stacks) / 2,
         (canva.ActualHeight / slices(j)) * (k) + topAddOn(j) + (canva.ActualHeight / slices(j)) / 2 * x / fc.chart[j][k].Count));
            if (distance > 0)
            {

                for (int i = j; i < y; i++)
                {
                    if (i > j) colorCounter--;
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
                        points.Add(new Point((canva.ActualWidth / stacks) * (y) + (canva.ActualWidth / stacks)/4,
                            (canva.ActualHeight / slices(y)) * (z) + topAddOn(y)));
                        //points.Add(new Point((canva.ActualWidth / stacks) * (y) + (canva.ActualWidth / stacks) / 2,
                        //    (canva.ActualHeight / slices(y)) * (z) + topAddOn(y) + (canva.ActualHeight / slices(y))/4));

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
                    //System.Windows.Media.Color color1 = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    MakeCubicCurve(points.ToArray());
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

        private void MakeCubicCurve(Point[] points)
        {
            if (colorCounter > indexcolors.Count() - 1) colorCounter = 0;
            var b = GetBezierApproximation(points, 256);
            PathFigure pf = new PathFigure(b.Points[0], new[] { b }, false);
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pf);
            var pge = new PathGeometry();
            pge.Figures = pfc;
            Path p = new Path();
            p.Data = pge;
            p.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom(indexcolors[++colorCounter]));
            canva.Children.Add(p);
        }



        // helpers
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

        private static String[] indexcolors = new String[]{

        "#000000", "#FFFF00", "#1CE6FF", "#FF34FF", "#FF4A46", "#008941", "#006FA6", "#A30059",

        "#FFDBE5", "#7A4900", "#0000A6", "#63FFAC", "#B79762", "#004D43", "#8FB0FF", "#997D87",
        "#5A0007", "#809693", "#FEFFE6", "#1B4400", "#4FC601", "#3B5DFF", "#4A3B53", "#FF2F80",
        "#61615A", "#BA0900", "#6B7900", "#00C2A0", "#FFAA92", "#FF90C9", "#B903AA", "#D16100",
        "#DDEFFF", "#000035", "#7B4F4B", "#A1C299", "#300018", "#0AA6D8", "#013349", "#00846F",
        "#372101", "#FFB500", "#C2FFED", "#A079BF", "#CC0744", "#C0B9B2", "#C2FF99", "#001E09",
        "#00489C", "#6F0062", "#0CBD66", "#EEC3FF", "#456D75", "#B77B68", "#7A87A1", "#788D66",
        "#885578", "#FAD09F", "#FF8A9A", "#D157A0", "#BEC459", "#456648", "#0086ED", "#886F4C",
        
        "#34362D", "#B4A8BD", "#00A6AA", "#452C2C", "#636375", "#A3C8C9", "#FF913F", "#938A81",
        "#575329", "#00FECF", "#B05B6F", "#8CD0FF", "#3B9700", "#04F757", "#C8A1A1", "#1E6E00",
        "#7900D7", "#A77500", "#6367A9", "#A05837", "#6B002C", "#772600", "#D790FF", "#9B9700",
        "#549E79", "#FFF69F", "#201625", "#72418F", "#BC23FF", "#99ADC0", "#3A2465", "#922329",
        "#5B4534", "#FDE8DC", "#404E55", "#0089A3", "#CB7E98", "#A4E804", "#324E72", "#6A3A4C",
        "#83AB58", "#001C1E", "#D1F7CE", "#004B28", "#C8D0F6", "#A3A489", "#806C66", "#222800",
        "#BF5650", "#E83000", "#66796D", "#DA007C", "#FF1A59", "#8ADBB4", "#1E0200", "#5B4E51",
        "#C895C5", "#320033", "#FF6832", "#66E1D3", "#CFCDAC", "#D0AC94", "#7ED379", "#012C58",
        
        "#7A7BFF", "#D68E01", "#353339", "#78AFA1", "#FEB2C6", "#75797C", "#837393", "#943A4D",
        "#B5F4FF", "#D2DCD5", "#9556BD", "#6A714A", "#001325", "#02525F", "#0AA3F7", "#E98176",
        "#DBD5DD", "#5EBCD1", "#3D4F44", "#7E6405", "#02684E", "#962B75", "#8D8546", "#9695C5",
        "#E773CE", "#D86A78", "#3E89BE", "#CA834E", "#518A87", "#5B113C", "#55813B", "#E704C4",
        "#00005F", "#A97399", "#4B8160", "#59738A", "#FF5DA7", "#F7C9BF", "#643127", "#513A01",
        "#6B94AA", "#51A058", "#A45B02", "#1D1702", "#E20027", "#E7AB63", "#4C6001", "#9C6966",
        "#64547B", "#97979E", "#006A66", "#391406", "#F4D749", "#0045D2", "#006C31", "#DDB6D0",
        "#7C6571", "#9FB2A4", "#00D891", "#15A08A", "#BC65E9", "#FFFFFE", "#C6DC99", "#203B3C",

        "#671190", "#6B3A64", "#F5E1FF", "#FFA0F2", "#CCAA35", "#374527", "#8BB400", "#797868",
        "#C6005A", "#3B000A", "#C86240", "#29607C", "#402334", "#7D5A44", "#CCB87C", "#B88183",
        "#AA5199", "#B5D6C3", "#A38469", "#9F94F0", "#A74571", "#B894A6", "#71BB8C", "#00B433",
        "#789EC9", "#6D80BA", "#953F00", "#5EFF03", "#E4FFFC", "#1BE177", "#BCB1E5", "#76912F",
        "#003109", "#0060CD", "#D20096", "#895563", "#29201D", "#5B3213", "#A76F42", "#89412E",
        "#1A3A2A", "#494B5A", "#A88C85", "#F4ABAA", "#A3F3AB", "#00C6C8", "#EA8B66", "#958A9F",
        "#BDC9D2", "#9FA064", "#BE4700", "#658188", "#83A485", "#453C23", "#47675D", "#3A3F00",
        "#061203", "#DFFB71", "#868E7E", "#98D058", "#6C8F7D", "#D7BFC2", "#3C3E6E", "#D83D66",
        
        "#2F5D9B", "#6C5E46", "#D25B88", "#5B656C", "#00B57F", "#545C46", "#866097", "#365D25",
        "#252F99", "#00CCFF", "#674E60", "#FC009C", "#92896B"
    };

        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }
    }

}
