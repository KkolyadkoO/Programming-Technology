using System.Windows;

namespace Lab10
{
    public partial class GraphSettingsWindow : Window
    {
        public string GraphText { get; set; }
        public double ParameterA { get; set; }
        public int FontStyleIndex { get; set; }

        public GraphSettingsWindow(string graphText, double parameterA, int fontStyleIndex)
        {
            InitializeComponent();

            // Инициализация свойств
            GraphText = graphText;
            ParameterA = parameterA;
            FontStyleIndex = fontStyleIndex;
            GraphTextBox.Text = GraphText;
            ParameterATextBox.Text = ParameterA.ToString();
            FontStyleComboBox.SelectedIndex = FontStyleIndex;


        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ParameterATextBox.Text, out double a))
            {
                GraphText = GraphTextBox.Text; // Получаем текст графика
                ParameterA = a; // Получаем параметр `a`
                FontStyleIndex = FontStyleComboBox.SelectedIndex; // Получаем выбранный стиль шрифта

                DialogResult = true; // Закрываем окно с результатом "успех"
                Close();
            }
            else
            {
                MessageBox.Show("Введите корректное значение параметра a.");
            }
        }
    }
}
