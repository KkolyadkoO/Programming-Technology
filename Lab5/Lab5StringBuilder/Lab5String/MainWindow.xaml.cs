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
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;

            string text = File.ReadAllText(filePath);
            textBoxInput.Text = text;

            StringBuilder sb = new StringBuilder();
            string[] words = text.Split(' ');
            string[] vowels = { "A", "E", "I", "O", "U", "a", "e", "i", "o", "u" };

            foreach (var word in words)
            {
                if (vowels.Contains(word[0].ToString()))
                {
                    sb.Append(char.ToUpper(word[0]) + word.Substring(1) + " ");
                }
                else
                {
                    sb.Append(word + " ");
                }
            }

            textBoxOutput.Text = sb.ToString().Trim();
        }
    }
}