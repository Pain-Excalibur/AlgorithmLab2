using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AlgorithmLab2
{
    public class TowerOfHanoi
    {
        private readonly Canvas canvas; // Для будущей визуализации
        private readonly TextBox textBox;
        private readonly List<string> steps;
        //параметры колец
        private readonly int MaxWidth = 270;
        private readonly int MinWidth = 60;
        private readonly int MaxHeight = 20; 
        //три палки для колец
        private readonly Canvas column1;
        private readonly Canvas column2;
        private readonly Canvas column3;

        public TowerOfHanoi(Canvas canvas, TextBox textBox, Canvas column1, Canvas column2, Canvas column3)
        {
            this.canvas = canvas;
            this.textBox = textBox;
            steps = [];
            this.column1 = column1;
            this.column2 = column2;
            this.column3 = column3;
        }

        public void SolveIteratively(int n)// итеративно
        {
            List<Tuple<int, int>> solution = new List<Tuple<int, int>>();

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
            DrawRings(n, solution);
        }

        public void SolveRecursively(int n) // рекурсивно
        {
            List<Tuple<int, int>> solution = new List<Tuple<int, int>>();

            MoveDisks(n, 1, 3, 2, solution);

            textBox.Text = string.Join(Environment.NewLine, steps);
            DrawRings(n, solution);
        }

        private void MoveDisks(int n, int from, int to, int temp, List<Tuple<int,int>> solution)
        {
            if (n == 1)
            {
                steps.Add($"{from} --> {to}");
                return;
            }

            MoveDisks(n - 1, from, temp, to, solution);
            steps.Add($"{from} --> {to}");
            solution.Add(new Tuple<int, int>(from - 1, to - 1)); ; // Визуально перемещаем кольцо
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

        private async void DrawRings(int n, List<Tuple<int,int>> solution)
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
            double animtimeS = 0.2;
            Canvas fromColumn = GetColumn(fromNumber);
            Canvas toColumn = GetColumn(toNumber);
            if (fromColumn.Children.Count == 0) return;

            // Извлекаем верхнее кольцо с исходной колонны
            Rectangle ring = (Rectangle)fromColumn.Children[fromColumn.Children.Count - 1];
            //anime

            ring.BeginAnimation(Canvas.TopProperty, new DoubleAnimation()
            {
                From = (int)(fromColumn.Height - (fromColumn.Children.Count * MaxHeight)),
                To = -100,
                Duration = TimeSpan.FromSeconds(animtimeS)
            });
            await Task.Delay((int)(animtimeS * 1000));

            ring.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation()
            {
                From = (int)(Canvas.GetLeft(fromColumn) + (284 - ring.Width) / 2),
                To = (int)(Canvas.GetLeft(toColumn)) + (284 - ring.Width) / 2,
                Duration = TimeSpan.FromSeconds(animtimeS)
            });
            await Task.Delay((int)(animtimeS * 1000));

            ring.BeginAnimation(Canvas.TopProperty, new DoubleAnimation()
            {
                From = -100,
                To = (int)(toColumn.Height - (toColumn.Children.Count * MaxHeight)),
                Duration = TimeSpan.FromSeconds(animtimeS)
            });
            Task.Delay((int)(animtimeS * 1000));

            await ChangeColumn(ring, fromColumn, toColumn);


        }

        private async Task ChangeColumn(Rectangle ring, Canvas fromColumn, Canvas toColumn)
        {
            fromColumn.Children.Remove(ring);
            await Task.Delay((10));

            toColumn.Children.Add(ring);
            await Task.Delay((10));
            // Обновляем позицию кольца в целевой колонне
            Canvas.SetTop(ring, toColumn.Height - (toColumn.Children.Count * MaxHeight) + 20);
            await Task.Delay((10));
            //Canvas.SetLeft(ring, (int)(Canvas.GetLeft(toColumn)) + (284 - ring.Width) / 2);
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
                return (SolidColorBrush)new BrushConverter().ConvertFrom(ColorsOfPaint[(int)((int)(13/n)*colorIndex)])!;
            }
        }
    }
}
