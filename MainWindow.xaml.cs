using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot;
using System.Windows;
using OxyPlot.Axes;
using System.Printing;
using System.Windows.Media;
using System.Windows.Controls;

namespace AlgorithmLab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HilbertCurve curve;
        private readonly TowerOfHanoi towers;

        public MainWindow()
        {
            InitializeComponent();

            curve = new HilbertCurve(HilbertCanvas, Brushes.Black, 2);
            towers = new TowerOfHanoi(HanoiCanvas, StepsTextBox);

            TimeGraph.Model = TimePlot.GetPlotModel("Ханойские башни", towers);
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecursionSelector.SelectedItem is ComboBoxItem selectedItem)
            {
                if (uint.TryParse(InputBox.Text, out uint n))
                {
                    if (selectedItem.Content.Equals("Кривая Гильберта"))
                    {
                        curve.DrawHilbertCurve(n, 500, 215, 50);
                    }
                    else if (selectedItem.Content.Equals("Ханойские башни"))
                    {
                        MessageBoxResult selection = MessageBox.Show(
                            "Использовать итеративный метод, вместо рекурсивного?",
                            "Выбор метода", MessageBoxButton.YesNo, MessageBoxImage.Question
                        );

                        towers.Solve((int)n, selection == MessageBoxResult.Yes); //второй аргумент отвечает за способ решения true - итеративный, false - рекурсивный
                    }
                }
                else
                {
                    MessageBox.Show("Введены некорректные параметры, повторите ввод.", "ОШИБКА: НЕКОРРЕКТНЫЕ ПАРАМЕТРЫ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Перед началом необходимо выбрать тип рекурсии.", "ОШИБКА: НЕ ВЫБРАНА РЕКУРСИЯ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RecursionSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem) RecursionSelector.SelectedItem;
            string? itemName = selectedItem.Content.ToString();

            if (itemName.Equals("Кривая Гильберта"))
            {
                Tabs.SelectedIndex = 0;
            }
            else if (itemName.Equals("Ханойские башни"))
            {
                Tabs.SelectedIndex = 1;
            }
        }
    }
}