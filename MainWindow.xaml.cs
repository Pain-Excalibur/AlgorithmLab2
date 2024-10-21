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

namespace AlgorithmLab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeRodNames();
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Clear();
            StepsCountLabel.Content = "";

            if (int.TryParse(InputTextBox.Text, out int numberOfDisks) && numberOfDisks > 0)
            {
                List<string> steps = new List<string>();
                SolveHanoiIteratively(numberOfDisks, steps);
                OutputTextBox.Text = string.Join(Environment.NewLine, steps);
                StepsCountLabel.Content = $"Количество шагов: {steps.Count}";
            }
            else
            {
                OutputTextBox.Text = "Пожалуйста, введите корректное число колец.";
            }
        }

        private void SolveHanoiIteratively(int n, List<string> steps)
        {
            char source = 'A';
            char target = 'C';
            char auxiliary = 'B';

            if (n % 2 == 0)
            {
                char temp = target;
                target = auxiliary;
                auxiliary = temp;
            }

            int totalMoves = (1 << n) - 1;

            for (int i = 1; i <= totalMoves; i++)
            {
                int from = (i & i - 1) % 3;
                int to = ((i | i - 1) + 1) % 3;

                steps.Add($"Переместить кольцо со стержня {GetRodName(from)} на стержень {GetRodName(to)}");
            }
        }

        private void RecursiveButton_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Clear();
            StepsCountLabel.Content = "";

            if (int.TryParse(InputTextBox.Text, out int numberOfDisks) && numberOfDisks > 0)
            {
                listBox1.Items.Clear();
                MoveDisks(numberOfDisks, 1, 3, 2);
                StepsCountLabel.Content = $"Количество шагов: {listBox1.Items.Count}";
            }
            else
            {
                OutputTextBox.Text = "Пожалуйста, введите корректное число колец.";
            }
        }

        private void MoveDisks(int n, int from, int to, int temp)
        {
            if (n == 1)
            {
                listBox1.Items.Add($"Переместить диск 1 со стержня {from} на стержень {to}");
                return;
            }

            MoveDisks(n - 1, from, temp, to);
            listBox1.Items.Add($"Переместить диск {n} со стержня {from} на стержень {to}");
            MoveDisks(n - 1, temp, to, from);
        }

        private char GetRodName(int index)
        {
            return index switch
            {
                0 => 'A',
                1 => 'B',
                2 => 'C',
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void InitializeRodNames()
        {
            listBox1.Items.Add("Стержень 1:");
            listBox1.Items.Add("Стержень 2:");
            listBox1.Items.Add("Стержень 3:");
        }
    }
}
}