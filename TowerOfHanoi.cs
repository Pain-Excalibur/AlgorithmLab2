using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AlgorithmLab2
{
    public class TowerOfHanoi
    {
        private readonly Canvas canvas; // Для будущей визуализации
        private readonly TextBox textBox;
        private readonly List<string> steps;
        // параметры колец
        private readonly int MaxWidth = 270;
        private readonly int MinWidth = 60;
        private readonly int MaxHeight = 20;
        // три палки для колец
        private readonly Canvas column1;
        private readonly Canvas column2;
        private readonly Canvas column3;

        public TowerOfHanoi(Canvas canvas, TextBox textBox, Canvas column1, Canvas column2, Canvas column3)
        {
            this.canvas = canvas;
            this.textBox = textBox;
            steps = new List<string>();
            this.column1 = column1;
            this.column2 = column2;
            this.column3 = column3;
        }

        public double SolveIterativelyWithTime(int n)
        {
            var stopwatch = Stopwatch.StartNew();
            SolveIteratively(n);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalSeconds;
        }

        public double SolveRecursivelyWithTime(int n)
        {
            var stopwatch = Stopwatch.StartNew();
            SolveRecursively(n);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalSeconds;
        }

        public List<Tuple<int, int>> SolveIteratively(int n)
        {
            List<Tuple<int, int>> solution = new List<Tuple<int, int>>();

            steps.Clear();

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

                steps.Add($"{GetRodName(from)} --> {GetRodName(to)}");
                solution.Add(new Tuple<int, int>(from, to)); // Визуально перемещаем кольцо
            }

            textBox.Text = string.Join(Environment.NewLine, steps);
            return solution;
        }

        public List<Tuple<int, int>> SolveRecursively(int n)
        {
            List<Tuple<int, int>> solution = new List<Tuple<int, int>>();

            steps.Clear();

            MoveDisks(n, 1, 3, 2, solution);

            textBox.Text = string.Join(Environment.NewLine, steps);
            return solution;
        }

        private void MoveDisks(int n, int from, int to, int temp, List<Tuple<int, int>> solution)
        {
            if (n == 1)
            {
                steps.Add($"{from} --> {to}");
                return;
            }

            MoveDisks(n - 1, from, temp, to, solution);
            steps.Add($"{from} --> {to}");
            solution.Add(new Tuple<int, int>(from - 1, to - 1)); // Визуально перемещаем кольцо
            MoveDisks(n - 1, temp, to, from, solution);
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

        public async Task DrawRings(int n, List<Tuple<int, int>> solution)
        {
            CreateArea(n);
            foreach (Tuple<int, int> t in solution)
            {
                await MoveRings(t.Item1, t.Item2);
            }
        }

        private void CreateArea(int n)//нарисовать поле
        {
            column1.Children.Clear();
            column2.Children.Clear();
            column3.Children.Clear();
            int difference;
            if (n > 1)
                difference = (MaxWidth - MinWidth) / (n - 1);
            else
                difference = 0;

            for (int ringNumber = 0; ringNumber < n; ringNumber++)
            {
                Rectangle ring = new Rectangle
                {
                    Width = MaxWidth - ringNumber * (difference),
                    Height = MaxHeight,
                    Fill = Brusher.RingsBrusher(ringNumber, n)
                };
                Canvas.SetLeft(ring, (284 - ring.Width) / 2);
                Canvas.SetBottom(ring, ring.Height * ringNumber - 20);
                column1.Children.Add(ring);
            }
        }

        private async Task MoveRings(int fromNumber, int toNumber)
        {
            double animtimeS = 0.15;
            Canvas fromColumn = GetColumn(fromNumber);
            Canvas toColumn = GetColumn(toNumber);
            if (fromColumn.Children.Count == 0) return;

            Rectangle ring = (Rectangle)fromColumn.Children[fromColumn.Children.Count - 1];

            ring.BeginAnimation(Canvas.BottomProperty, new DoubleAnimation()
            {
                To = fromColumn.Height + 100,
                Duration = TimeSpan.FromSeconds(animtimeS)
            });
            await Task.Delay((int)(animtimeS * 1000 + 100));

            if (fromNumber < toNumber)
                ring.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation()
                {
                    To = (int)(Canvas.GetLeft(toColumn) - Canvas.GetLeft(fromColumn) + (284 - ring.Width) / 2),
                    Duration = TimeSpan.FromSeconds(animtimeS)
                });
            else
                ring.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation()
                {
                    To = (int)((Canvas.GetLeft(toColumn) - Canvas.GetLeft(fromColumn)) + ((284 - ring.Width) / 2)),
                    Duration = TimeSpan.FromSeconds(animtimeS)
                });
            await Task.Delay((int)(animtimeS * 1000 + 100));

            ring.BeginAnimation(Canvas.BottomProperty, new DoubleAnimation()
            {
                To = toColumn.Children.Count * MaxHeight - 20,
                Duration = TimeSpan.FromSeconds(animtimeS)
            });
            await Task.Delay((int)(animtimeS * 1000 + 100));

            await ChangeColumn(ring, fromColumn, toColumn);
        }

        private async Task ChangeColumn(Rectangle ring, Canvas fromColumn, Canvas toColumn)
        {
            fromColumn.Children.Remove(ring);

            Rectangle ring2 = new Rectangle
            {
                Width = ring.Width,
                Height = ring.Height,
                Fill = ring.Fill
            };

            Canvas.SetBottom(ring2, (toColumn.Children.Count * MaxHeight) - 20);
            Canvas.SetLeft(ring2, (284 - ring.Width) / 2);
            toColumn.Children.Add(ring2);
            await Task.Delay(200);
        }

        private Canvas GetColumn(int index)
        {
            return index switch
            {
                0 => column1,
                1 => column2,
                2 => column3,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public class Brusher
        {
            public static readonly List<string> ColorsOfPaint = new()
            {
                "#FF0000",
                "#8B0000",
                "#FFA500",
                "#8B4513",
                "#FFFF00",
                "#808000",
                "#008000",
                "#006400",
                "#0000FF",
                "#000080",
                "#4B0082",
                "#2E0854",
                "#FF69B4",
                "#FF1493"
            };
            public static SolidColorBrush RingsBrusher(int colorIndex, int n)
            {
                return (SolidColorBrush)new BrushConverter().ConvertFrom(ColorsOfPaint[(int)((int)(13 / n) * colorIndex)])!;
            }
        }
    }
}