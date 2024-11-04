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
                ShiftPanel.Visibility = Visibility.Collapsed;
                RegisterState.Visibility = Visibility.Collapsed;
                break;
            case "D-Trigger (Memory)":
                currentElement = memoryElement;
                ShiftPanel.Visibility = Visibility.Collapsed;
                RegisterState.Visibility = Visibility.Collapsed;
                break;
            case "Register":
                currentElement = registerElement;
                ShiftPanel.Visibility = Visibility.Visible;
                RegisterState.Visibility = Visibility.Visible;
                break;
        }
        UpdateTriggersInfo();
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
            if(currentElement == null)
            {
                MessageBox.Show($"Выберите элемент");
                return;
            }
            currentElement.SetInputs(inputs);
            UpdateTriggersInfo();
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
        UpdateTriggersInfo();
    }

    private void ShiftButton_Click(object sender, RoutedEventArgs e)
    {
        if(!int.TryParse(ShiftValue.Text, out var shift) || shift<0)
        {
            OutputResult.Text = $"Output (ERROR): Enter a correct shift value";
            return;
        }
        if (currentElement is Register register)
        {
            register.Shift(shift);
            OutputResult.Text = "Shifted Register";
        }
       
        UpdateTriggersInfo();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        using (var writer = new StreamWriter("save.txt"))
        {
            writer.WriteLine(memoryElement.ToBinaryString());
            writer.WriteLine(registerElement.ToBinaryString());
            writer.WriteLine(combinationalElement.ToBinaryString());
        } 
    }

    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        if (!File.Exists("save.txt"))
        {
            MessageBox.Show("Error", "Savefile not found");
            return;
        }
        using(var reader = new StreamReader("save.txt"))
        {
            string line = reader.ReadLine();
            memoryElement.FromBinaryString(line);
            line = reader.ReadLine();
            registerElement.FromBinaryString(line);
            line = reader.ReadLine();
            combinationalElement.FromBinaryString(line);
        }
        
        UpdateTriggersInfo();
    }
    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        registerElement.SetSetState(1);
        UpdateTriggersInfo();
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        registerElement.SetSetState(0);
        UpdateTriggersInfo();
    }
    private void UpdateTriggersInfo()
    {
        try
        {
            if (currentElement is Register)
            {
                OutputAllTriggersInRegister.Text = "Triggers: " + currentElement.ToString();
                RegisterState.IsChecked = registerElement.getCurrentState() == 0 ? false : true;

            }
            else
            {
                OutputAllTriggersInRegister.Text = "Inputs: " + currentElement.ToString();
            }
        }
        catch (NullReferenceException)
        {

        }
        
    }
}