using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot;
using System.Windows;
using OxyPlot.Axes;
using System.Printing;

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
            InitializeTimeGraph();
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
            if (RecursionSelector.SelectedItem != null)
            {
                if (uint.TryParse(InputBox.Text, out uint n))
                {
                    // Заглушка
                    MessageBox.Show("Всё окей дружище.", "Ура", MessageBoxButton.OK, MessageBoxImage.Information);
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
