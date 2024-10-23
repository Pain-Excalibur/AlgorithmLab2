using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics;

namespace AlgorithmLab2
{
    /// <summary>
    /// Класс для создания и настройки графиков времени.
    /// </summary>
    public class TimePlot
    {
        //your function based on x,y
        public static double getValue(int x, int y)
        {
            //TowerOfHanoi tower = new TowerOfHanoi();
            Stopwatch stopwatch = new Stopwatch();

            for (int i = 0; i < 5; i++)
            {

            }

            return 1;
        }

        //setting the values to the function
        public static FunctionSeries GetFunction(TowerOfHanoi tower)
        {
            int n = 5;
            FunctionSeries serie = new FunctionSeries();
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    //adding the points based x,y
                    DataPoint data = new DataPoint(x, getValue(x, y));

                    //adding the point to the serie
                    serie.Points.Add(data);
                }
            }
            //returning the serie
            return serie;
        }

        /// <summary>
        /// Создает модель графика с заданным заголовком.
        /// </summary>
        /// <param name="title">Заголовок графика. Если значение null, будет присвоена пустая строка.</param>
        /// <returns>Модель графика с осью X и осью Y.</returns>
        public static PlotModel GetPlotModel(string title, TowerOfHanoi tower)
        {
            PlotModel model = new()
            {
                Title = title ?? string.Empty
            };

            model.Axes.Add(CreateAxis(AxisPosition.Bottom, "Размерность"));
            model.Axes.Add(CreateAxis(AxisPosition.Left, "Секунды"));

            model.Series.Add(GetFunction(tower));

            return model;
        }

        /// <summary>
        /// Создает ось графика с заданной позицией и заголовком.
        /// </summary>
        /// <param name="position">Позиция оси.</param>
        /// <param name="title">Заголовок оси.</param>
        /// <returns>Объект LinearAxis, представляющий ось графика.</returns>
        private static LinearAxis CreateAxis(AxisPosition position, string title)
        {
            return new LinearAxis
            {
                Position = position,
                AbsoluteMinimum = 0,
                Title = title
            };
        }

    }
}
