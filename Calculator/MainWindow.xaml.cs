using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator;

public partial class MainWindow : Window
{
    private readonly List<double> operands = [];
    private readonly List<char> operators = [];
    private bool startsNewNumber;
    private bool isShowingResult;

    public MainWindow() => InitializeComponent();

    private void NumberButton_Click(object sender, RoutedEventArgs e)
    {
        string number = ((Button)sender).Content.ToString()!;
        EnterNumber(number);
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.None)
        {
            return;
        }

        string? number = e.Key switch
        {
            Key.D0 or Key.NumPad0 => "0",
            Key.D1 or Key.NumPad1 => "1",
            Key.D2 or Key.NumPad2 => "2",
            Key.D3 or Key.NumPad3 => "3",
            Key.D4 or Key.NumPad4 => "4",
            Key.D5 or Key.NumPad5 => "5",
            Key.D6 or Key.NumPad6 => "6",
            Key.D7 or Key.NumPad7 => "7",
            Key.D8 or Key.NumPad8 => "8",
            Key.D9 or Key.NumPad9 => "9",
            _ => null
        };

        if (number is null)
        {
            return;
        }

        EnterNumber(number);
        e.Handled = true;
    }

    private void EnterNumber(string number)
    {
        if (isShowingResult || startsNewNumber || Display.Text == "Error")
        {
            if (isShowingResult) ResetExpression();
            Display.Text = number == "." ? "0." : number;
            isShowingResult = startsNewNumber = false;
        }
        else if (number != "." || !Display.Text.Contains('.'))
        {
            Display.Text = Display.Text == "0" ? number : Display.Text + number;
        }
    }

    private void OperatorButton_Click(object sender, RoutedEventArgs e)
    {
        char selectedOperator = ((Button)sender).Content.ToString()![0];
        if (!TryGetDisplayValue(out double value)) { ShowError(); return; }
        if (startsNewNumber && operators.Count > 0)
        {
            operators[^1] = selectedOperator;
            return;
        }

        operands.Add(value);
        operators.Add(selectedOperator);
        startsNewNumber = true;
        isShowingResult = false;
    }

    private void EqualsButton_Click(object sender, RoutedEventArgs e)
    {
        if (operators.Count == 0) return;
        if (!startsNewNumber)
        {
            if (!TryGetDisplayValue(out double value)) { ShowError(); return; }
            operands.Add(value);
        }

        if (!TryEvaluate(out double result)) { ShowError(); return; }
        Display.Text = FormatNumber(result);
        ResetExpression();
        isShowingResult = true;
        startsNewNumber = false;
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        Display.Text = "0";
        ResetExpression();
        isShowingResult = startsNewNumber = false;
    }

    private void BackspaceButton_Click(object sender, RoutedEventArgs e)
    {
        if (isShowingResult || startsNewNumber || Display.Text == "Error")
        {
            Display.Text = "0";
            isShowingResult = startsNewNumber = false;
            return;
        }
        Display.Text = Display.Text.Length > 1 ? Display.Text[..^1] : "0";
        if (Display.Text is "-" or "") Display.Text = "0";
    }

    private void ToggleSignButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetDisplayValue(out double value)) { ShowError(); return; }
        Display.Text = FormatNumber(-value);
        isShowingResult = false;
    }

    private void PercentButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetDisplayValue(out double value)) { ShowError(); return; }
        Display.Text = FormatNumber(value / 100);
        isShowingResult = false;
    }

    private bool TryEvaluate(out double result)
    {
        result = 0;
        if (operands.Count != operators.Count + 1) return false;

        var reducedOperands = new List<double> { operands[0] };
        var reducedOperators = new List<char>();
        for (int index = 0; index < operators.Count; index++)
        {
            double next = operands[index + 1];
            char operation = operators[index];
            if (operation is '*' or '/')
            {
                double left = reducedOperands[^1];
                if (operation == '/' && next == 0) return false;
                reducedOperands[^1] = operation == '*' ? left * next : left / next;
            }
            else
            {
                reducedOperators.Add(operation);
                reducedOperands.Add(next);
            }
        }

        result = reducedOperands[0];
        for (int index = 0; index < reducedOperators.Count; index++)
            result = reducedOperators[index] == '+' ? result + reducedOperands[index + 1] : result - reducedOperands[index + 1];
        return !double.IsNaN(result) && !double.IsInfinity(result);
    }

    private bool TryGetDisplayValue(out double value) =>
        double.TryParse(Display.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);

    private static string FormatNumber(double value) => value.ToString("G15", CultureInfo.InvariantCulture);

    private void ShowError()
    {
        Display.Text = "Error";
        ResetExpression();
        isShowingResult = true;
        startsNewNumber = false;
    }

    private void ResetExpression()
    {
        operands.Clear();
        operators.Clear();
    }
}
