using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2
{
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResultListBox.Items.Add("Таблица Значений функции");
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            ResultListBox.Items.Clear();
            ResultListBox.Items.Add("Таблица Значений функции");
            string inputXmin = XminTextBox.Text;
            string inputXmax = XmaxTextBox.Text;
            string inputDx = dXTextBox.Text;
            double y = 0;

            if (!double.TryParse(inputXmin, out double xmin))
            {
                MessageBox.Show("Ошибка: введите корректное число для Xmin.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(inputXmax, out double xmax))
            {
                MessageBox.Show("Ошибка: введите корректное число для Xmax.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(inputDx, out double dx))
            {
                MessageBox.Show("Ошибка: введите корректное число для Dx.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (xmin >= xmax)
            {
                MessageBox.Show("Ошибка: Xmax должно быть больше Xmin.", "Ошибка логики", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dx <= 0)
            {
                MessageBox.Show("Ошибка: Dx должно быть больше 0.", "Ошибка логики", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (double x = xmin; x <= xmax; x += dx)
            {
                if(x < -7 || x > 3) {
                    ResultListBox.Items.Add("x=" + x.ToString() + " вне диапазона");
                    continue;
                    }
                if (x >= -7 && x <= -6) y = 1;
                if (x > -6 && x < -4) y = (-0.5) * x - 2;
                if (x >= -4 && x < 0) y = Math.Sqrt(-Math.Pow(x, 2) - 4 * x);
                if (x >= 0 && x < 2) y = -Math.Sqrt(-Math.Pow(x, 2) + 2 * x);
                if (x >= 2 && x <= 3) y = 2 - x;
                if (y == -0) y = 0;
                ResultListBox.Items.Add("x=" + x.ToString() + " " + y.ToString());
            }
        }

    }
}