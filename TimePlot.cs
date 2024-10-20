using OxyPlot;
using OxyPlot.Axes;

namespace AlgorithmLab2
{
    /// <summary>
    /// Класс для создания и настройки графиков времени.
    /// </summary>
    public class TimePlot
    {
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
