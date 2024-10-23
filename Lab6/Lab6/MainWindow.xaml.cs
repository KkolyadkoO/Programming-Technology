using System.Windows;

namespace Lab6;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<SubjectIndex> _indexList = new List<SubjectIndex>();
    
    public MainWindow()
    {
        InitializeComponent();
    }
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        // Пример добавления данных с клавиатуры (тут можно сделать через ввод с TextBox)
        _indexList.Add(new SubjectIndex("Example", new List<int> { 1, 2, 3 }));
        UpdateListBox();
    }

    private void SortButton_Click(object sender, RoutedEventArgs e)
    {
        _indexList.Sort();
        UpdateListBox();
    }

    private void UpdateListBox()
    {
        IndexList.Items.Clear();
        foreach (var item in _indexList)
        {
            IndexList.Items.Add(item.ToString());
        }
    }
}