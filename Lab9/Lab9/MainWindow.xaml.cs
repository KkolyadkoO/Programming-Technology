using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Lab9.Classes;

namespace Lab9;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Element currentElement;
    private Combinational combinationalElement;
    private Memory memoryElement;
    private Register registerElement;

    public MainWindow()
    {
        InitializeComponent();
        InitializeElements();
    }

    private void InitializeElements()
    {
        combinationalElement = new Combinational(5);
        memoryElement = new Memory();
        registerElement = new Register(10);
    }

    private void ElementSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        var selected = ((System.Windows.Controls.ComboBoxItem)ElementSelector.SelectedItem).Content.ToString();
        switch (selected)
        {
            case "MOD2 (Combinational)":
                currentElement = combinationalElement;
                ShiftButton.Visibility = Visibility.Collapsed;
                break;
            case "D-Trigger (Memory)":
                currentElement = memoryElement;
                ShiftButton.Visibility = Visibility.Collapsed;
                break;
            case "Register":
                currentElement = registerElement;
                ShiftButton.Visibility = Visibility.Visible;
                break;
        }

        DisplayElementProperties();
    }

    private void DisplayElementProperties()
    {
        ElementName.Text = $"Name: {currentElement.Name}";
        InputCount.Text = $"Input Count: {currentElement.InputCount}";
        OutputCount.Text = $"Output Count: {currentElement.OutputCount}";
    }

    private void SetInputsButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var inputs = InputValues.Text.Split(',')
                .Select(int.Parse)
                .ToArray();
            currentElement.SetInputs(inputs);
            OutputResult.Text = "Inputs set successfully.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error setting inputs: {ex.Message}");
        }
    }

    private void ComputeButton_Click(object sender, RoutedEventArgs e)
    {
        OutputResult.Text = $"Output: {currentElement.ComputeOutput()}";
    }

    private void InvertButton_Click(object sender, RoutedEventArgs e)
    {
        currentElement.Invert();
        OutputResult.Text = $"Output (Inverted): {currentElement.ComputeOutput()}";
    }

    private void ShiftButton_Click(object sender, RoutedEventArgs e)
    {
        if (currentElement is Register register)
        {
            register.Shift(1); // Сдвиг на 1 бит (можно добавить возможность выбора количества битов)
            OutputResult.Text = "Shifted Register";
        }
    }

    [Obsolete("Obsolete")]
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using (FileStream fs = new FileStream("elements.bin", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, new Element[] { combinationalElement, memoryElement, registerElement });
            }

            MessageBox.Show("State saved successfully.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving state: {ex.Message}");
        }
    }

    [Obsolete("Obsolete")]
    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using (FileStream fs = new FileStream("elements.bin", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var elements = (Element[])formatter.Deserialize(fs);

                combinationalElement = elements.OfType<Combinational>().FirstOrDefault();
                memoryElement = elements.OfType<Memory>().FirstOrDefault();
                registerElement = elements.OfType<Register>().FirstOrDefault();

                MessageBox.Show("State loaded successfully.");
            }

            DisplayElementProperties();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading state: {ex.Message}");
        }
    }
}