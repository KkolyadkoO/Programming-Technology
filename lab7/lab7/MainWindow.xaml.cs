using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab7;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<NOTE> notes = new List<NOTE>();

    public MainWindow()
    {
        InitializeComponent();
        Resources["BooleanToVisibilityConverter"] = new BooleanToVisibilityConverter();
    }

    private void AddNote_Click(object sender, RoutedEventArgs e)
    {
        // Чтение данных из текстовых полей
        string fullName = FullNameTextBox.Text.Trim();
        string phoneNumber = PhoneTextBox.Text.Trim();

        if (!IsValidFullName(fullName))
        {
            MessageBox.Show("Фамилия и инициалы должны быть в формате: Фамилия И. О.");
            return;
        }

        if (!IsValidPhoneNumber(phoneNumber))
        {
            MessageBox.Show("Номер телефона должен быть в формате: +375XXXXXXXXX (12 цифр после +375).");
            return;
        }

        if (!int.TryParse(DayTextBox.Text, out int day) ||
            !int.TryParse(MonthTextBox.Text, out int month) ||
            !int.TryParse(YearTextBox.Text, out int year) ||
            !IsValidDate(day, month, year))
        {
            MessageBox.Show("Введите корректную дату рождения.");
            return;
        }

        // Создание новой записи
        NOTE newNote = new NOTE
        {
            FullName = fullName,
            PhoneNumber = phoneNumber,
            BirthDate = new int[] { day, month, year }
        };

        // Добавление записи и сортировка по фамилии
        notes.Add(newNote);
        notes = notes.OrderBy(n => n.FullName).ToList();
        UpdateNotesList();

        // Очистка текстовых полей после добавления
        FullNameTextBox.Clear();
        PhoneTextBox.Clear();
        DayTextBox.Clear();
        MonthTextBox.Clear();
        YearTextBox.Clear();
    }
    private void UpdateNotesList()
    {
        NotesListBox.Items.Clear();
        foreach (var note in notes)
        {
            NotesListBox.Items.Add($"ФИО: {note.FullName}, Телефон: {note.PhoneNumber}, " +
                                   $"Дата рождения: {note.BirthDate[0]:D2}.{note.BirthDate[1]:D2}.{note.BirthDate[2]}");
        }
    }

    // Проверка ФИО на формат "Фамилия И. О."
    private bool IsValidFullName(string fullName)
    {
        return Regex.IsMatch(fullName, @"^[А-ЯЁ][а-яё]+\s[А-ЯЁ]\.\s[А-ЯЁ]\.$");
    }

    // Проверка номера телефона на формат "+375XXXXXXXXX"
    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, @"^\+375\d{9}$");
    }

    // Проверка на существующую дату
    private bool IsValidDate(int day, int month, int year)
    {
        try
        {
            DateTime date = new DateTime(year, month, day);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Обработчик кнопки для поиска по месяцу
    private void SearchByMonth_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(MonthSearchTextBox.Text, out int searchMonth) || searchMonth < 1 || searchMonth > 12)
        {
            MessageBox.Show("Введите корректный номер месяца (1-12).");
            return;
        }

        // Поиск всех записей с указанным месяцем
        var results = notes.Where(note => note.BirthDate[1] == searchMonth).ToList();

        // Очистка и обновление результатов
        ResultsTextBox.Clear();
        if (results.Count > 0)
        {
            foreach (var note in results)
            {
                ResultsTextBox.AppendText($"ФИО: {note.FullName}, Телефон: {note.PhoneNumber}, " +
                                          $"Дата рождения: {note.BirthDate[0]:D2}.{note.BirthDate[1]:D2}.{note.BirthDate[2]}\n");
            }
        }
        else
        {
            ResultsTextBox.AppendText("Записи с таким месяцем рождения не найдены.\n");
        }
    }
}