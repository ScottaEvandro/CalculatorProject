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
    private bool isEnteringSecondOperand;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void NumberButton_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;

        string number = button.Content.ToString()!;

        if (isEnteringSecondOperand)
        {
            Display.Text = number;
            isEnteringSecondOperand = false;
        }
        else
        {
            Display.Text = Display.Text == "0" ? number : Display.Text + number;
        }
    }

    private void OperatorButton_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        var operatorText = button.Content.ToString();

        firstOperand = double.Parse(Display.Text);
        currentOperator = operatorText![0];
        isEnteringSecondOperand = true;
    }

    private void EqualsButton_Click(object sender, RoutedEventArgs e)
    {
        var secondOperand = double.Parse(Display.Text);

        if (currentOperator == '+')
        {
            Display.Text = (firstOperand + secondOperand).ToString();
        }
    }
}