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
        const int MAXITER = 500;
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
            string inputE = ETextBox.Text;

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

            if (!double.TryParse(inputE, out double eps))
            {
                MessageBox.Show("Ошибка: введите корректное число для ε.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (eps <= 0)
            {
                MessageBox.Show("Ошибка: ε должно быть больше 0.", "Ошибка логики", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (double x = xmin; x <= xmax; x += dx)
            {
                double sum = 0;
                bool done = true;
                double term, prev=1;
                int n;


                for (n = 1; n < MAXITER; n++)
                {
                    term = Math.Abs(prev) * Math.Pow(-1, n) * (Math.Pow(x,2)/n);

                   
                    sum += term;
                    prev = term;

                    if (Math.Abs(term) < eps)
                    {
                        done = false;
                        if (term < 0) continue;
                        break;
                    }
                }
                sum += 1;

                double f = Math.Pow(Math.E,-Math.Pow(x,2));


                if (done)
                {
                    ResultListBox.Items.Add($"x = {x:F4}, Ряд не сошёлся после {MAXITER} итераций");
                }
                else
                {
                    ResultListBox.Items.Add($"x = {x:F4}, Сумма ряда = {sum:F6}, Точное значение = {f:F6}, Членов ряда = {n + 1}");
                }
            }
        }

        

    }
}