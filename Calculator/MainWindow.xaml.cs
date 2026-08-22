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
    private bool isShowingResult;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void NumberButton_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;

        string number = button.Content.ToString()!;

        if (isShowingResult)
        {
            Display.Text = number;
            isShowingResult = false;
        }
        else if (isEnteringSecondOperand)
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

        switch (currentOperator)
        {
            case '+':
                Display.Text = (firstOperand + secondOperand).ToString();
                break;
            case '-':
                Display.Text = (firstOperand - secondOperand).ToString();
                break;
            case '*':
                Display.Text = (firstOperand * secondOperand).ToString();
                break;
            case '/':
                if (secondOperand != 0)
                {
                    Display.Text = (firstOperand / secondOperand).ToString();
                }
                else
                {
                    Display.Text = "Error";
                }
                break;
        }

        isShowingResult = true;
    }
}