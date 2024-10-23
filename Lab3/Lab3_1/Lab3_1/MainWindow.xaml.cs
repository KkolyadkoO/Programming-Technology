using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Lab3_1
{
    public partial class MainWindow : Window
    {
        private double[] randomNumbers;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(InputN.Text, out int n) && n > 0)
            {
                Random random = new Random();
                randomNumbers = new double[n];
                for (int i = 0; i < n; i++)
                {
                    randomNumbers[i] = Math.Round(random.NextDouble() * 121 - 20, 3);
                }

                FillDataGrid(randomNumbers, DataGridView);
                UpdateValues();
            }
            else
            {
                MessageBox.Show("Введите корректное положительное целочисленное число.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FillDataGrid(double[] array, DataGrid dataGrid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Число", typeof(double));
            foreach (var num in array)
            {
                dt.Rows.Add(Math.Round(num, 3));
            }
            dataGrid.ItemsSource = dt.DefaultView;
        }

        private void DataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            DataView dv = (DataView)DataGridView.ItemsSource;
            randomNumbers = dv.Table.Rows.Cast<DataRow>().Select(row => Math.Round(Convert.ToDouble(row["Число"]), 3)).ToArray();

            UpdateValues();
        }

        private void UpdateValues()
        {
            double sumElement = GetSumAbs();
            SumOfElements.Text = Math.Round(sumElement, 3).ToString();


            double[] transformedArray = TransformArray();
            FillDataGrid(transformedArray, TransformedDataGridView);
        }

        private double GetSumAbs()
        {
            if(randomNumbers == null)
            {
                return 0;
            }
            double maxElement = randomNumbers.Max();
            int maxIndex = Array.IndexOf(randomNumbers, maxElement);
            double sum = randomNumbers
                    .Skip(maxIndex + 1)  
                    .Select(Math.Abs)
                    .Sum();
            return sum;
        }


        private double[] TransformArray()
        {
            return randomNumbers.OrderBy(x => Math.Abs(x)).ToArray();
        }

        private void FindCountOfElement_Click(object sender, RoutedEventArgs e)
        {
            if(!double.TryParse(A.Text, out double a))
            {
                MessageBox.Show("Ошибка: введите корректное число для A.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(!double.TryParse(B.Text, out double b))
            {
                MessageBox.Show("Ошибка: введите корректное число для A.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(a > b)
            {
                MessageBox.Show("Ошибка: А должно быть меньше чем B", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(randomNumbers == null)
            {
                MessageBox.Show("Ошибка: Массив пустой сначала сгенерируйте значения", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
           
            double count = randomNumbers.Count(x => x >= a && x <= b);
            CountOfElements.Text = count.ToString();
        }
    }
}
