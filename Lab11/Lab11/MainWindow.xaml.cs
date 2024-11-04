using System.IO;
using System.Text;
using System.Windows;
using Path = System.IO.Path;

namespace Lab11;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CancellationTokenSource _cancellationTokenSource;

    public MainWindow()
    {
        InitializeComponent();
        _cancellationTokenSource = new CancellationTokenSource();
        
    }

    private async void ProcessTextFile_Click(object sender, RoutedEventArgs e)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "text_file.txt");
        if (!File.Exists(filePath))
        {
            MessageBox.Show("Файл не найден.");
            return;
        }
        StartBackgroundTask();
        textBoxInput.Text = "Чтение и обработка текста...";
        string text = await File.ReadAllTextAsync(filePath);

        string[] words = text.Split(' ');
        string[] vowels = { "A", "E", "I", "O", "U", "a", "e", "i", "o", "u" };
        StringBuilder result = new StringBuilder();

        await Task.Run( async () =>
        {
            for (int i = 0; i < words.Length; i++)
            {
                if (vowels.Contains(words[i][0].ToString()))
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }

                if (i % 100 == 0) 
                {
                    Dispatcher.Invoke(() => { textBoxInput.Text = $"Преобразованный текст:\n{result.ToString()}"; });
                }
                await Task.Delay(10); 
                result.Append(words[i] + " ");
            }
        });

        textBoxInput.Text = "Преобразованный текст:\n" + result.ToString();
    }

    private async void StartBackgroundTask()
    {
        await Task.Run(() => GenerateFibonacciSequence(_cancellationTokenSource.Token));
    }

    private void GenerateFibonacciSequence(CancellationToken token)
    {
        long first = 0, second = 1;
        while (!token.IsCancellationRequested)
        {
            long next = first + second;
            first = second;
            second = next;

            Dispatcher.Invoke(() => { textBoxSecondaryOutput.Text += $"Число Фибоначчи: {next}\n"; });

            Thread.Sleep(500); 
        }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _cancellationTokenSource.Cancel();
    }
}