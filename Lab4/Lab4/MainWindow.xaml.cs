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

namespace Lab4;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void OnCalculateClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var firstBinary = new BinaryNumber(FirstBinaryInput.Text);
            var secondBinary = new BinaryNumber(SecondBinaryInput.Text);
            BinaryNumber result = null;

            switch (OperationComboBox.SelectedIndex)
            {
                case 0: 
                    result = firstBinary + secondBinary;
                    break;
                case 1: 
                    result = firstBinary - secondBinary;
                    break;
                case 2: 
                    result = firstBinary * secondBinary;
                    break;
                case 3:
                    result = firstBinary / secondBinary;
                    break;
                default:
                    MessageBox.Show("Выберите операцию.");
                    return;
            }


            ResultTextBlock.Text = $"Результат: {result}";
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
        catch (DivideByZeroException)
        {
            MessageBox.Show("Ошибка: Деление на ноль.");
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}
