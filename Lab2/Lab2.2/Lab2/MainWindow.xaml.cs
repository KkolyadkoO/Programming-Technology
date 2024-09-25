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
        private int numberOfShots = 0, goodShoot = 0;
        public MainWindow()
        {
            InitializeComponent();
            ResultListBox.Items.Add("Таблица Значений функции");
        }


        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (numberOfShots == 10)
            {
                numberOfShots = 0;
                goodShoot = 0;
                ResultListBox.Items.Clear();
                ResultListBox.Items.Add("Таблица Значений функции");
            }

            string inputR = RTextBox.Text;
            string inputX = XTextBox.Text;
            string inputY = YTextBox.Text;

            if (!double.TryParse(inputR, out double r))
            {
                MessageBox.Show("Ошибка: введите корректное число для R.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(inputX, out double x))
            {
                MessageBox.Show("Ошибка: введите корректное число для X.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(inputY, out double y))
            {
                MessageBox.Show("Ошибка: введите корректное число для Y.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (r <= 0)
            {
                MessageBox.Show("Ошибка: Радиус должен быть больше 0.", "Ошибка логики", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsPointInGrayArea(x, y, r))
            {
                goodShoot++;
                ResultListBox.Items.Add((numberOfShots + 1).ToString() + " x=" + x.ToString() + " y=" + y.ToString() + " R=" + r.ToString() + " Попал");
            }
            else
            {
                ResultListBox.Items.Add((numberOfShots + 1).ToString() + " x=" + x.ToString() + " y=" + y.ToString() + " R=" + r.ToString() + " Мимо");
            }

            numberOfShots++;

            if (numberOfShots == 10)
            {
                MessageBox.Show("Количество попаданий " + goodShoot.ToString(), "Конец серии", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private bool IsPointInGrayArea(double x, double y, double R)
        {
            bool inFirstQuarter = (x + R) * (x + R) + (y - R) * (y - R) >= R * R && x <= 0 && y >= 0 && x >= -R && y <= R;

            bool inFourthQuarter = x * x + y * y <= R * R && x >= 0 && y <= 0;

            return inFirstQuarter || inFourthQuarter;
        }

    }
}