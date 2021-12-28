using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using System.Windows.Shapes;

namespace Fogel
{
    class MinCost : ILeastCostMethodSolve
    {
        Grid MainGrid;
        public int sum = 0;
        MatrixN changedMatrix;
        MatrixN matrix;
        private int iteration = 1;
        private Dictionary<KeyValuePair<int, int>, int> products = new Dictionary<KeyValuePair<int, int>, int>();
        Dictionary<int, int> rows; //текущие строки
        Dictionary<int, int> columns; //текущие столбцы
        Dictionary<KeyValuePair<int, int>, int> minValues; //минимальые значения найденные в матрице
        public MinCost(MatrixN matrix, Grid MainGrid) { this.matrix = matrix; this.MainGrid = MainGrid; }
        public static void PutMatrixToGrid(Grid current, MatrixN matrix)
        {

            if (matrix != null)
            {
                var def = new RowDefinition();
                var col = new ColumnDefinition();
                for (int i = 0; i < matrix.getMatrix().GetLength(0); i++)
                {
                    def = new RowDefinition();
                    def.Height = new GridLength(5, GridUnitType.Star);
                    current.RowDefinitions.Add(def);

                    for (int j = 0; j < matrix.getMatrix().GetLength(1); j++)
                    {
                        if (i == 0)
                        {
                            col = new ColumnDefinition();
                            col.Width = new GridLength(5, GridUnitType.Star);
                            current.ColumnDefinitions.Add(col);
                        }

                        var smth = new TextBlockContent
                        {
                            Coef = matrix.getMatrix()[i, j],                           
                        };

                        Border txt3 = createStackPanel(smth);
                        Grid.SetColumn(txt3, j);
                        Grid.SetRow(txt3, i);
                        current.Children.Add(txt3);
                    }
                }

                Border c = CreatePost();
                current.Children.RemoveAt(0);
                Grid.SetColumn(c, 0);
                Grid.SetRow(c, 0);
                current.Children.Insert(0, c);

            }
            else
            {
                throw new Exception("Matrix dont initialisate");
            }
        }

        private static Border CreatePost()
        {
            Border c = new Border();
            c.BorderBrush = Brushes.Black;
            c.BorderThickness = new Thickness(0.5);
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(15, 15, 0, 10);
            TextBlock a = new TextBlock();
            a.Text = "Потребители /";
            a.Margin = new Thickness(0, 0, 10, 0);
            TextBlock b = new TextBlock();
            b.Text = "Поставщики";
            b.FontSize = 14;
            a.FontSize = 14;
            stackPanel.Children.Add(a);
            stackPanel.Children.Add(b);
            c.Child = stackPanel;
            return c;
        }


        public KeyValuePair<int, int> GetMinElement(int current)
        {
            KeyValuePair<int, int> minEl = new KeyValuePair<int, int>();
            int min = int.MaxValue; // батя ваерус

           foreach(var row in rows)
            {
                foreach(var column in columns)
                {
                    if (min > changedMatrix.getMatrix()[row.Key, column.Key] && changedMatrix.getMatrix()[row.Key, column.Key] != 0
                        || min > changedMatrix.getMatrix()[row.Key, column.Key] && changedMatrix.getMatrix()[row.Key, column.Key] == 0 && (columns.Count == 1 || rows.Count == 1))
                    {
                        min = changedMatrix.getMatrix()[row.Key, column.Key];
                        minEl = new KeyValuePair<int, int>(row.Key, column.Key);
                    }
                    
                }
            }
                
            return minEl;
        }
      
        public void SolveTask()
        {
     
           // List<Grid> grids = new List<Grid>(); //таблицы для вывода
            rows = new Dictionary<int, int>(); //хранение не исключенных из итераций строк
            columns = new Dictionary<int, int>(); //хранение не исключенных из итераций колонок
            KeyValuePair<int, int> currentMin = GetMin(matrix, rows, columns); //получение минимального блока для первой итерации;
            minValues = new Dictionary<KeyValuePair<int, int>, int>();            
            changedMatrix = new MatrixN();
            changedMatrix.CopyMatrix(matrix); //матрица которая будет хранить измененные значения поставщиков и потребителей

            while (rows.Count != 0 && columns.Count != 0) //пока все строки и колонки не удоволетворят потребности
            {
               
                int minBlock = GetMinProduct(currentMin); //получение значения продукта для распределения
                minValues.Add(currentMin, minBlock);                                      
                currentMin = GetMinElement(changedMatrix.getMatrix()[currentMin.Key, currentMin.Value]);
                AddMinValuesToGrid();
                iteration++;
            }
           
                
      }

       protected void AddMinValuesToGrid()
        {
            Grid a = CreateGridDynamic();         
            MinCost.PutMatrixToGrid(a, matrix);
            int columnsCount = matrix.getMatrix().GetLength(1);
            foreach (var item in minValues)
            {
                var smth = new TextBlockContent
                {
                    Coef = matrix.getMatrix()[item.Key.Key, item.Key.Value],
                    Product = item.Value
                };
                int position = ((item.Key.Key + 1) * columnsCount) - (columnsCount - item.Key.Value);//позиция в grid текущей минимальной ячейки
                Border txt3 = createStackPanel2(smth);
                a.Children.RemoveAt(position);
                Grid.SetColumn(txt3, item.Key.Value);
                Grid.SetRow(txt3, item.Key.Key);
                a.Children.Insert(position, txt3);
            }
            foreach (var item in minValues)
            {
                sum += item.Value * matrix.getMatrix()[item.Key.Key, item.Key.Value];
            }
            TextBlock textBlock = new TextBlock();
            textBlock.Text = $"Шаг {iteration} Текущая сумма опорного плана = {sum}";
            textBlock.FontSize = 20;
            textBlock.Margin = new Thickness(50, 20, 20, 0);
            sum = 0;
          

            MainGrid.RowDefinitions.Add(new RowDefinition());
            Grid.SetRow(a, iteration+1);
            Grid.SetColumnSpan(a, 2);

            MainGrid.RowDefinitions.Add(new RowDefinition());
            Grid.SetRow(textBlock, iteration+1);
            Grid.SetColumnSpan(textBlock, 2);

            MainGrid.Children.Add(textBlock);
            MainGrid.Children.Add(a);
            
            //  grids.Add(a);

        }
        protected KeyValuePair<int, int> GetMin(MatrixN matrix, Dictionary<int, int> rows, Dictionary<int, int> columns)
        {
            int min = matrix.getMatrix()[1, 1];
            KeyValuePair<int, int> minimum = new KeyValuePair<int, int>();
            for (int i = 1; i < matrix.getMatrix().GetLength(0); i++)
            {
                rows.Add(i,0);
                for (int j = 1; j < matrix.getMatrix().GetLength(1); j++)
                {
                    if (i == 1)
                    {
                        columns.Add(j, 0);
                    }
                    if (min > matrix.getMatrix()[i, j] && matrix.getMatrix()[i, j] != 0)
                    {
                        min = matrix.getMatrix()[i, j];
                        minimum = new KeyValuePair<int, int>(i, j);
                    }
                }
            }
            return minimum;
        }


       
        protected int GetMinProduct(KeyValuePair<int, int> currentMin)
        {
            if (changedMatrix.getMatrix()[0, currentMin.Value] > changedMatrix.getMatrix()[currentMin.Key, 0])
            {
                int minValue = matrix.getMatrix()[currentMin.Key, 0];
                changedMatrix.getMatrix()[0, currentMin.Value] = changedMatrix.getMatrix()[0, currentMin.Value] - minValue;
                if(changedMatrix.getMatrix()[0, currentMin.Value] == 0)
                {
                   for(int i = 0; i < columns.Count; i++)
                    {

                    }
                    columns.Remove(currentMin.Value);
                }
                changedMatrix.getMatrix()[currentMin.Key, 0] = changedMatrix.getMatrix()[currentMin.Key, 0] - minValue;
                if (changedMatrix.getMatrix()[currentMin.Key, 0] == 0)
                {
                  
                    rows.Remove(currentMin.Key);
                }
                return minValue;
            }
            else
            {
                int minValue = changedMatrix.getMatrix()[0, currentMin.Value];
                changedMatrix.getMatrix()[0, currentMin.Value] = changedMatrix.getMatrix()[0, currentMin.Value] - minValue;
                if (changedMatrix.getMatrix()[0, currentMin.Value] == 0)
                {
                
                    columns.Remove(currentMin.Value);
                }
                changedMatrix.getMatrix()[currentMin.Key, 0] = changedMatrix.getMatrix()[currentMin.Key, 0] - minValue;
                if (changedMatrix.getMatrix()[currentMin.Key, 0] == 0)
                {                
                    rows.Remove(currentMin.Key);
                }
                return minValue;
            }
        }

        public static Grid CreateGridDynamic()
        {
            Grid DataGrid = new Grid();        
            DataGrid.Background = Brushes.White;
            DataGrid.HorizontalAlignment = HorizontalAlignment.Left;
            DataGrid.VerticalAlignment = VerticalAlignment.Top;
            DataGrid.Margin = new Thickness(50, 70, 20, 10);
            return DataGrid;
        }

        public static Border createStackPanel2(TextBlockContent content)
        {
            Border c = new Border();
            c.BorderBrush = Brushes.Black;
            c.BorderThickness = new Thickness(1);
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(45, 10, 20, 0);
            var a = new TextBlock();
            a.FontSize = 18;
            a.Margin = new Thickness(10, 0, 0, 0);
            var b = new TextBlock();
            b.FontSize = 18;
            b.Margin = new Thickness(-40, 2, 5, 0);
            stackPanel.Children.Add(a);
            stackPanel.Children.Add(b);
            var binding = new Binding("Coef");
            binding.Mode = BindingMode.OneWay;
            binding.Source = content;
            var binding2 = new Binding("Product");
            binding2.Mode = BindingMode.OneWay;
            binding2.Source = content;
            BindingOperations.SetBinding(stackPanel.Children[0], TextBlock.TextProperty, binding);
            BindingOperations.SetBinding(stackPanel.Children[1], TextBlock.TextProperty, binding2);
            c.Child = stackPanel;
            return c;
        }

        public static Border createStackPanel(TextBlockContent content)
        {
            Border c = new Border();
            c.BorderBrush = Brushes.Black;
            c.BorderThickness = new Thickness(0.5);
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(53, 15, 20, 20);
            var a = new TextBlock();
            a.FontSize = 18;
            stackPanel.Children.Add(a);
            var binding = new Binding("Coef");
            binding.Mode = BindingMode.OneWay;
            binding.Source = content;
            BindingOperations.SetBinding(stackPanel.Children[0], TextBlock.TextProperty, binding);
            c.Child = stackPanel;
            return c;
        }
    }
}
