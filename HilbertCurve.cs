using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlgorithmLab2
{
    /// <summary>
    /// Класс для построения и отрисовки кривой Гильберта на холсте.
    /// </summary>
    public class HilbertCurve : IStepable
    {
        private double x, y; // Координаты отрисовки линии.
        private readonly Canvas canvas; // Холст для отрисовки.
        private readonly Brush strokeColor; // Цвет линии.
        private readonly double strokeThickness; // Толщина линии.
        private int steps; // Количество отдельных шагов для отрисовки фрактала

        /// <summary>
        /// Конструктор класса HilbertCurve.
        /// </summary>
        /// <param name="canvas">Холст, на котором будет отрисовываться кривая Гильберта.</param>
        public HilbertCurve(Canvas canvas, Brush strokeColor, double strokeThickness)
        {
            this.canvas = canvas;
            this.strokeColor = strokeColor;
            this.strokeThickness = strokeThickness;
        }

        /// <summary>
        /// Внимание! Может вернуть 0 если не было запущено вычисление.
        /// </summary>
        /// <returns>
        /// Количество совершённых шагов
        /// </returns>
        public double GetStepsCount()
        {
            return steps;
        }

        /// <summary>
        /// Начинает отрисовку кривой Гильберта на холсте с указанным уровнем и размером.
        /// </summary>
        /// <param name="level">Уровень рекурсии, определяющий сложность кривой.</param>
        /// <param name="size">Размер области, в которой будет рисоваться кривая.</param>
        /// <param name="x">Начальная координата по оси X.</param>
        /// <param name="y">Начальная координата по оси Y.</param>
        public void DrawHilbertCurve(uint level, double size, double x, double y)
        {
            this.x = x;
            this.y = y;

            canvas.Children.Clear();

            double step = size / (Math.Pow(2, level) - 1); // Шаг на основе размера и уровня.
            Hilbert(level, step, 0, 0, step);
        }

        /// <summary>
        /// Рекурсивная функция для построения кривой Гильберта.
        /// </summary>
        /// <param name="level">Текущий уровень рекурсии.</param>
        /// <param name="dx">Смещение по оси X.</param>
        /// <param name="dy">Смещение по оси Y.</param>
        /// <param name="nextDx">Следующее смещение по оси X.</param>
        /// <param name="nextDy">Следующее смещение по оси Y.</param>
        private void Hilbert(uint level, double dx, double dy, double nextDx, double nextDy)
        {
            if (level == 0) return; // Выход из рекурсии.

            this.steps++;

            Hilbert(level - 1, dy, dx, nextDy, nextDx);
            DrawLine(x, y, x + dx, y + dy);
            x += dx;
            y += dy;

            Hilbert(level - 1, dx, dy, nextDx, nextDy);
            DrawLine(x, y, x + nextDx, y + nextDy);
            x += nextDx;
            y += nextDy;

            Hilbert(level - 1, dx, dy, nextDx, nextDy);
            DrawLine(x, y, x - dx, y - dy);
            x -= dx;
            y -= dy;

            Hilbert(level - 1, -dy, -dx, -nextDy, -nextDx);
        }

        /// <summary>
        /// Рисует линию между двумя точками (x1, y1) и (x2, y2) на холсте.
        /// </summary>
        /// <param name="x1">Начальная координата X линии.</param>
        /// <param name="y1">Начальная координата Y линии.</param>
        /// <param name="x2">Конечная координата X линии.</param>
        /// <param name="y2">Конечная координата Y линии.</param>
        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            Line line = new()
            {
                X1 = x1, Y1 = y1,
                X2 = x2, Y2 = y2,
                Stroke = strokeColor,
                StrokeThickness = strokeThickness
            };

            canvas.Children.Add(line);
        }
    }
}
