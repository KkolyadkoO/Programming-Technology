using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;


namespace Lab5String;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ReadAndProcessText_Click(object sender, RoutedEventArgs e)
    {
        // Открываем диалог выбора файла
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;
            MessageBox.Show(filePath);

            string text = File.ReadAllText(filePath);
            textBoxInput.Text = "Исходный текст:\n"+text;

            string[] words = text.Split(' ');
            string[] vowels = { "A", "E", "I", "O", "U", "a", "e", "i", "o", "u" };

            for (long i = 0; i < words.Length; i++)
            {
                try
                {
                    if (vowels.Contains(words[i][0].ToString()))
                    {
                        words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                    }
                }
                catch(Exception) {
                }
            }

            string result = string.Join(" ", words);

            textBoxOutput.Text = "Преобразованный текст:\n"+result;
        }
    }
}