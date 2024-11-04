using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Lab6;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<SubjectIndex> subjectIndices = new List<SubjectIndex>();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        string word = wordTextBox.Text;
        string pagesInput = pagesTextBox.Text;

        if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(pagesInput))
        {
            MessageBox.Show("Введите слово и номер страницы");
            return;
        }

        try
        {
            List<int> pages = pagesInput.Split(',').Select(int.Parse).ToList();
            if (pages.Count > 10)
            {
                MessageBox.Show("Вы не можете ввести больше 10 страниц");
                return;
            }

            subjectIndices.Add(new SubjectIndex(word, pages));
            UpdateListBox();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Проблема в вводе, некоректный формат.\nПодробности:\n" + ex.Message);

        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (subjectIndexListBox.SelectedItem is SubjectIndex selected)
        {
            subjectIndices.Remove(selected);
            UpdateListBox();
        }
        else
        {
            MessageBox.Show("Пожалуйста выберите элемент для удаления");
        }
    }

    private void LoadFromFileButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                LoadFromFile(openFileDialog.FileName);
                UpdateListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка во время загрузки с файла:\n" + ex.Message);
            }
        }
    }

    private void LoadFromFile(string filePath)
    {
        subjectIndices.Clear();  
        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            string[] parts = line.Split(':');
            if (parts.Length != 2)
            {
                throw new FormatException($"Неправильный формат строки:\n{line}");
            }

            string word = parts[0].Trim();
            List<int> pages = parts[1].Split(',')
                                     .Select(p => int.Parse(p.Trim()))
                                     .ToList();

            subjectIndices.Add(new SubjectIndex(word, pages));
        }
    }


    private void SortButton_Click(object sender, RoutedEventArgs e)
    {
        subjectIndices.Sort();
        UpdateListBox();
    }

    private void SortByPageButton_Click(object sender, RoutedEventArgs e)
    {
        PageNumberComparer comparer = new PageNumberComparer();
        subjectIndices.Sort(comparer);
        UpdateListBox();
    }

    private void UpdateListBox()
    {
        subjectIndexListBox.ItemsSource = null;
        subjectIndexListBox.ItemsSource = subjectIndices;
    }
}