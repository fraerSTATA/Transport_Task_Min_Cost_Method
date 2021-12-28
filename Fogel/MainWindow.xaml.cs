using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            
             InitializeComponent();
            int j = 1;
            Grid DataGrid = CreateGridDynamic();
            Grid.SetRow(DataGrid, j);
            Grid.SetColumnSpan(DataGrid, 2);
            MainGrid.Children.Add(DataGrid);      
            var minich = new MatrixN();
            minich.TakeMatrixFromXML("C:\\Users\\bym20\\OneDrive\\Рабочий стол\\Fogel\\Matrix.xml");
      
            MinCost.PutMatrixToGrid(DataGrid, minich);


           MinCost min = new MinCost();
           List<Grid> a = min.SolveTask(minich,this);
           foreach(var item in a)
            {
                j++;
                MainGrid.RowDefinitions.Add(new RowDefinition());
                Grid.SetRow(item, j);
                Grid.SetColumnSpan(item, 2);
                MainGrid.Children.Add(item);
            }
            j++;
            MainGrid.RowDefinitions.Add(new RowDefinition());
            Label b = new Label();
            b.Content = "Итоговая сумма опорного плана = " + min.sum.ToString();
            b.FontSize = 18;
            b.Margin = new Thickness(50, 40, 0, 30);
            Grid.SetRow(b, j);
            Grid.SetColumnSpan(b, 2);
            MainGrid.Children.Add(b);

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
    }
    
}
