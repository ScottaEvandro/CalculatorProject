using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private double? firstOperand;
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
        firstOperand = double.Parse(Display.Text);
        Display.Text = "0";
    }
}