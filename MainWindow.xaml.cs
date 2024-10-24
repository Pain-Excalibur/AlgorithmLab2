using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot;
using System.Windows;
using OxyPlot.Axes;
using System.Printing;
using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;

namespace AlgorithmLab2
{
    public partial class MainWindow : Window
    {
        private readonly HilbertCurve curve;
        private readonly TowerOfHanoi towers;

        public MainWindow()
        {
            InitializeComponent();

            curve = new HilbertCurve(HilbertCanvas, Brushes.Black, 2);

            // Передаем три колонки для башен
            towers = new TowerOfHanoi(HanoiCanvas, StepsTextBox, Column1, Column2, Column3);

            TimeGraph.Model = TimePlot.GetPlotModel("Ханойские башни");
        }

        private async void RunButton_Click(object sender, RoutedEventArgs e)
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

                        var points = new List<DataPoint>();
                        for (int i = 1; i <= n; i++)
                        {
                            double time;
                            if (selection == MessageBoxResult.Yes)
                            {
                                time = towers.SolveIterativelyWithTime(i);
                            }
                            else
                            {
                                time = towers.SolveRecursivelyWithTime(i);
                            }
                            points.Add(new DataPoint(i, time));
                        }

                        string methodTitle = selection == MessageBoxResult.Yes ? "Итеративный метод" : "Рекурсивный метод";
                        TimePlot.AddSeries(TimeGraph.Model, points, methodTitle);

                        List<Tuple<int, int>> solution;
                        if (selection == MessageBoxResult.Yes)
                        {
                            solution = towers.SolveIteratively((int)n);
                        }
                        else
                        {
                            solution = towers.SolveRecursively((int)n);
                        }

                        await towers.DrawRings((int)n, solution);
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
            ComboBoxItem selectedItem = (ComboBoxItem)RecursionSelector.SelectedItem;
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
