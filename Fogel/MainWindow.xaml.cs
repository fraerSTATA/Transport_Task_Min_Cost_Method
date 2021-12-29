using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Xml.Linq;

namespace Fogel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MatrixN minich;
        MinCost min;
        public MainWindow()
        {


            InitializeComponent();


        }


        public  Grid CreateGridDynamic()
        {
            Grid DataGrid = new Grid();          
            DataGrid.Background = Brushes.White;
            DataGrid.HorizontalAlignment = HorizontalAlignment.Left;
            DataGrid.VerticalAlignment = VerticalAlignment.Top;
           
            this.Title = "Grid Sample";
            DataGrid.Margin = new Thickness(50, 70, 20, 10);
           

            return DataGrid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
    
            Grid DataGrid = CreateGridDynamic();
            Grid.SetRow(DataGrid, 1);
            Grid.SetColumnSpan(DataGrid, 2);
            MainGrid.Children.Add(DataGrid);
             minich = new MatrixN();
            string filePath = "";
            var openFileDialog = new OpenFileDialog();
            
                
                openFileDialog.Filter = "XML Files (*.xml)|*.xml";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog().HasValue)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
            }
            minich.TakeMatrixFromXML(filePath);
            MinCost.PutMatrixToGrid(DataGrid, minich);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            min = new MinCost(minich, MainGrid);
            min.SolveTask();
            MainGrid.RowDefinitions.Add(new RowDefinition());
            Label b = new Label();
            b.Content = "Итоговая стоимость опорного плана = " + min.FinalSum.ToString();
            b.FontSize = 18;
            b.Margin = new Thickness(50, 40, 0, 30);
            Grid.SetRow(b, MainGrid.RowDefinitions.Count);
            Grid.SetColumnSpan(b, 2);
            MainGrid.Children.Add(b);
            if (!min.IsDegenerate)
            {
                MessageBox.Show("Невырожденный опорный план");
            }
            else
            {
                MessageBox.Show("Вырожденный опорный план");
            }
            MethodOfPotentials a = new MethodOfPotentials(min);
            a.OptimizePlan();
        }
    }
    
}
