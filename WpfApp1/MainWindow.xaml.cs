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
        protected internal double MaxAvailableHeight { get; private set; }
        protected internal double MaxAvailableWidth { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            MaxAvailableHeight = canva.ActualHeight;
            MaxAvailableWidth = canva.ActualWidth;
            
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
            int numberofStacks = i > 13 ? i : 13;
            for (int j = 0; j < i; j++)
            {
                int numberofSlices = fc.chart[j].Count > 7 ? fc.chart[j].Count : 7;
                for (int k = 0; k < fc.chart[j].Count; k++)
                {
                    DrawElement(j, k, numberofSlices, numberofStacks);
                }
            }
        }


        private void DrawElement(int j, int k, int slices, int stacks)
        {
            if (j % 2 == 0)
            {
                Rectangle rect = new Rectangle()
                {
                    Name = fc.chart[j][k][0].ToString(),
                    Width = (MaxAvailableWidth / stacks) / 2,
                    Height = (MaxAvailableHeight / slices) / 2,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Margin = new Thickness((MaxAvailableWidth / stacks) * (j + 1 / 4), (MaxAvailableHeight / slices) * (k + 1 / 4), 0, 0)
                };
                canva.Children.Add(rect);
            }
            else
            {
                Ellipse ellipse = new Ellipse()
                {
                    Name = fc.chart[j][k][fc.chart[j][k].Count - 2].ToString(),
                    Width = (MaxAvailableWidth / stacks) / 2,
                    Height = (MaxAvailableHeight / slices) / 2,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Margin = new Thickness((MaxAvailableWidth / stacks) * (j + 1 / 4), (MaxAvailableHeight / slices) * (k + 1 / 4), 0, 0)
                };
                canva.Children.Add(ellipse);
            }
        }


        private void In_Focused(object sender, RoutedEventArgs e)
        {
            inputsT.SelectAll();
        }
    }

}
