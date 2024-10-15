using System;
using System.Collections.Generic;
using System.Data;
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

namespace Lab3_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateMatrix_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rows = int.Parse(((TextBox)this.FindName("matrixRows")).Text);
                int columns = int.Parse(((TextBox)this.FindName("matrixColumns")).Text);

                DataTable table = new DataTable();

                for (int i = 0; i < columns; i++)
                {
                    table.Columns.Add("Col " + (i + 1));
                }

                Random random = new Random();

                for (int i = 0; i < rows; i++)
                {
                    DataRow newRow = table.NewRow();
                    for (int j = 0; j < columns; j++)
                    {
                        newRow[j] = random.Next(-10, 11);
                    }
                    table.Rows.Add(newRow);
                }

                matrixGrid.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            DataView dataView = matrixGrid.ItemsSource as DataView;

            if (dataView != null)
            {
                int rowCount = dataView.Count;
                int columnCount = dataView.Table.Columns.Count;

                int[,] matrix = new int[rowCount, columnCount];

                for (int i = 0; i < rowCount; i++)
                {
                    DataRowView rowView = dataView[i];
                    for (int j = 0; j < columnCount; j++)
                    {
                        matrix[i, j] = Convert.ToInt32(rowView[j]);
                    }
                }
                if (!int.TryParse(countForShift.Text, out int shift))
                {
                    MessageBox.Show("Ошибка: введите корректное число для сдвига.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                var selectedItem = comboBoxDirection.SelectedItem as ComboBoxItem;
                string selectedDirection = selectedItem.Content.ToString();

                if (selectedDirection == "Вправо")
                {
                    
                    ShiftRight(matrix, shift);

                }
                else if(selectedDirection == "Вниз")
                {
                    ShiftDown(matrix, shift);
                }


                DataTable sortedTable = new DataTable();
                for (int i = 0; i < columnCount; i++)
                {
                    sortedTable.Columns.Add("Col " + (i + 1));
                }

                for (int i = 0; i < rowCount; i++)
                {
                    DataRow newRow = sortedTable.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        newRow[j] = matrix[i, j];
                    }
                    sortedTable.Rows.Add(newRow);
                }

                matrixGrid.ItemsSource = sortedTable.DefaultView;
            }
            else
            {
                MessageBox.Show("Матрица пуста или не существует.", "Ошибка:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        static void ShiftRight(int[,] matrix, int shiftAmount)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                int[] row = new int[cols];

                // Для каждого элемента строки вычисляем новое положение
                for (int j = 0; j < cols; j++)
                {
                    row[(j + shiftAmount) % cols] = matrix[i, j];
                }

                // Заменяем исходную строку на сдвинутую
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = row[j];
                }
            }
        }

        static void ShiftDown(int[,] matrix, int shiftAmount)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int j = 0; j < cols; j++)
            {
                int[] column = new int[rows];

                // Для каждого элемента столбца вычисляем новое положение
                for (int i = 0; i < rows; i++)
                {
                    column[(i + shiftAmount) % rows] = matrix[i, j];
                }

                // Заменяем исходный столбец на сдвинутый
                for (int i = 0; i < rows; i++)
                {
                    matrix[i, j] = column[i];
                }
            }
        }



        private void matrixRows_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Логика, связанная с изменением текста, если потребуется
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (matrixGrid.CurrentCell != null)
            {
                var currentCell = matrixGrid.CurrentCell;
                var dataRowView = currentCell.Item as DataRowView;

                if (dataRowView != null)
                {
                    matrixGrid.BeginEdit();
                }
            }
        }

        private void matrixGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editingElement = e.EditingElement as TextBox;
                if (editingElement != null)
                {
                    if (!int.TryParse(editingElement.Text, out int value))
                    {
                        MessageBox.Show("Пожалуйста, введите корректное целое число.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        editingElement.Text = "0"; // Устанавливаем значение ячейки в 0
                        e.Cancel = true; // Отменяем изменение ячейки
                        return;
                    }
                    // Если число корректное, значение будет применено к DataGrid
                }
            }
        }
    }
}
