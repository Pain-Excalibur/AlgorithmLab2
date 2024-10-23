using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics;
using MathNet.Numerics;

namespace AlgorithmLab2
{
    /// <summary>
    /// Класс для создания и настройки графиков времени.
    /// </summary>
    public class TimePlot
    {
        private static double GetTime(int x)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            TowerOfHanoi towerOfHanoi = new TowerOfHanoi();
            towerOfHanoi.SolveRecursively(5);
            stopwatch.Restart();
            towerOfHanoi.SolveRecursively(x);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        public static FunctionSeries GetTimeGraph(int n)
        {
            FunctionSeries serie = new FunctionSeries();
            for (int x = 1; x < n; x++)
            {
              
                    //adding the points based x,y
                 DataPoint data = new DataPoint(x, GetTime(x));

                    //adding the point to the serie
                serie.Points.Add(data);
                
            }
            

            return serie;
        }

        public static LineSeries GetApproximationGraph(double[] data)
        {
            int length = data.Length;
            double[] elements = new double[length];

            for (int i = 0; i < length; i++)
            {
                elements[i] = i+1;
            }

            // TODO: наверное нужна более подробная аппроксимация, потому не везде подходит вторая степень.
            var coefficients = Fit.Polynomial(elements, data, 2);
            var f = Fit.PolynomialFunc(elements, data, 5);

            LineSeries lineSeries = new()
            {
                Title = "Аппроксимация",
                Color = OxyColors.Red
            };

            for (int i = 0; i < data.Length; i++)
            {
                double value = coefficients[0] + coefficients[1] * (i+1) + coefficients[2] * Math.Pow(i + 1, 2);
                lineSeries.Points.Add(new DataPoint(i, f(i)));
            }

            return lineSeries;
        }

        /// <summary>
        /// Создает модель графика с заданным заголовком.
        /// </summary>
        /// <param name="title">Заголовок графика. Если значение null, будет присвоена пустая строка.</param>
        /// <returns>Модель графика с осью X и осью Y.</returns>
        public static PlotModel GetPlotModel(string title)
        {
            PlotModel model = new()
            {
                Title = title ?? string.Empty
            };

            model.Axes.Add(CreateAxis(AxisPosition.Bottom, "Размерность"));
            model.Axes.Add(CreateAxis(AxisPosition.Left, "Секунды"));

            
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
