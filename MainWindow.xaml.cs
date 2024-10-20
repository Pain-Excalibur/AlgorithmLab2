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

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimeGraph();

            curve = new HilbertCurve(HilbertCanvas, Brushes.Black, 2);
        }

        private void InitializeTimeGraph()
        {
            PlotModel timeModel = new();

            timeModel.Axes.Add(CreateAxis(AxisPosition.Bottom, "Размерность"));
            timeModel.Axes.Add(CreateAxis(AxisPosition.Left, "Секунды"));

            TimeGraph.Model = timeModel;
        }


        private static LinearAxis CreateAxis(AxisPosition position, string title)
        {
            return new LinearAxis
            {
                Position = position,
                AbsoluteMinimum = 0,
                Title = title
            };
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
                    else
                    {
                        MessageBox.Show("Жду башенки", "Где?", MessageBoxButton.OK, MessageBoxImage.Error);
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

    }
}
