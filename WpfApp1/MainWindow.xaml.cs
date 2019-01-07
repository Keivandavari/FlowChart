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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            fc.Add(inputsT.Text.Split(' ').ToList(), outputT.Text, fT.Text);
            diagram.InvalidateVisual();
            ((MainWindow)System.Windows.Application.Current.MainWindow).UpdateLayout();
        }

        private void In_Focused(object sender, RoutedEventArgs e)
        {
            inputsT.SelectAll();
        }
    }

}
