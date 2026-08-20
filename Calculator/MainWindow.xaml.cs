using System.Windows;
using System.Windows.Controls;

#nullable disable

namespace Calculator;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private double? firstOperand;
    private char? currentOperator;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void NumberButton_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;

        string number = button.Content.ToString()!;

        Display.Text = Display.Text == "0" ? number : Display.Text + number;
    }

    private void OperatorButton_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        var operatorText = button.Content.ToString();
        firstOperand = double.Parse(Display.Text);
        Display.Text = "0";
        currentOperator = operatorText![0];
    }
}