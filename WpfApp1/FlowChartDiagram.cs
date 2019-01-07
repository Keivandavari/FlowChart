using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;
using Point = System.Drawing.Point;

namespace WpfApp1
{
    [TemplatePart(Name = "PART_Content", Type = typeof(Grid))]
    public class FlowChartDiagram : UserControl
    {
        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register("Chart", typeof(ObservableCollection<ObservableCollection<ObservableCollection<string>>>), typeof(FlowChartDiagram), new PropertyMetadata(EventsPropertyChanged));

        private static void EventsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FlowChartDiagram)d).EventsPropertyChanged(e);
        }

        private void EventsPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            var newCollection = args.NewValue as INotifyCollectionChanged;
            if (newCollection != null)
            { }
        }

        public IEnumerable Events
        {
            get { return (IEnumerable)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }
        public ObservableCollection<ObservableCollection<ObservableCollection<string>>> Chart
        {
            get { return (ObservableCollection<ObservableCollection<ObservableCollection<string>>>)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }
        Grid ContentGrid { get; set; }
        protected internal double MaxAvailableHeight { get; private set; }
        protected internal double MaxAvailableWidth { get; private set; }
        public FlowChartDiagram()
        {
            DefaultStyleKey = typeof(FlowChartDiagram);
        }
        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            bool allowUpdateData = MaxAvailableHeight == 0;
            MaxAvailableHeight = availableSize.Height;
            MaxAvailableWidth = availableSize.Width;
            if (allowUpdateData)
                UpdateData();
            return base.MeasureOverride(availableSize);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ContentGrid = (Grid)GetTemplateChild("PART_Content");
            if (ContentGrid != null)
                UpdateData();
        }
        private void OnSizesChanged()
        {
            UpdateData();
        }
        private void UpdateData()
        {
            if (ContentGrid == null)
                return;
            int i = 0;
            for(i = 0; i < Chart.Count;i++)
            {
                if (Chart[i].Count == 0) break;
            }
            int numberofStacks = i > 13 ? i : 13;
            for (int j = 0; j < i; j++)
            {
                int numberofSlices = Chart[j].Count > 7 ? Chart[j].Count : 7;
                for (int k = 0; k < Chart[j].Count; k++)
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
                    Name = Chart[j][k][0].ToString(),
                    Width = (MaxAvailableWidth / stacks) / 2,
                    Height = (MaxAvailableHeight / slices) / 2,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Margin = new Thickness((MaxAvailableWidth / stacks) * (j + 1 / 4), (MaxAvailableHeight / slices) * (k + 1 / 4), 0, 0)
                };
                ContentGrid.Children.Add(rect);
            }
            else
            {
                Ellipse ellipse = new Ellipse()
                {
                    Name = Chart[j][k][Chart[j][k].Count - 2].ToString(),
                    Width = (MaxAvailableWidth / stacks) / 2,
                    Height = (MaxAvailableHeight / slices) / 2,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Margin = new Thickness((MaxAvailableWidth / stacks) * (j + 1 / 4), (MaxAvailableHeight / slices) * (k + 1 / 4), 0, 0)
                };
                ContentGrid.Children.Add(ellipse);
            }
        }


    }
}
