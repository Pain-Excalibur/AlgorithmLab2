using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot;
using System.Windows;
using OxyPlot.Axes;
using System.Printing;
using System.Windows.Media;
using System.Windows.Controls;
using System.Reflection.Emit;

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
            towers = new TowerOfHanoi(HanoiCanvas, StepsTextBox, Column1, Column2, Column3);

            TimeGraph.Model = TimePlot.GetPlotModel("Ханойские башни");
        }

        public async void RunButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => {
                Dispatcher.BeginInvoke((Action)(() =>
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

                                if (selection == MessageBoxResult.Yes)
                                {
                                    towers.SolveIteratively((int)n);
                                }
                                else
                                {
                                    towers.SolveRecursively((int)n);
                                }
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
                }));
            });
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

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}