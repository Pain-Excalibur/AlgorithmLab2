using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AlgorithmLab2
{
    public class TowerOfHanoi : IStepable
    {
        private readonly Canvas canvas; // Для будущей визуализации
        private readonly TextBox textBox;
        private readonly List<string> steps;

        public TowerOfHanoi(Canvas canvas, TextBox textBox)
        {
            this.canvas = canvas;
            this.textBox = textBox;
            steps = [];
        }
        /// <summary>
        /// Внимание! Может вернуть 0 если не было запущено вычисление.
        /// </summary>
        /// <returns>
        /// Количество совершённых шагов
        /// </returns>
        public double GetStepsCount()
        {
            return steps.Count;
        }

        private void Clear()
        {
            steps.Clear();
            canvas.Children.Clear();
        }

        private void PrintSteps()
        {
            textBox.Text = string.Join(Environment.NewLine, steps);
        }

        public void Solve(int n, bool solveByIteration)
        {
            Clear();
            if (solveByIteration)
            {
                SolveIteratively(n);
            }
            else
            {
                SolveRecursively(n);
            }
            PrintSteps();
        }

        public void SolveIteratively(int n)
        {

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
            }

        }

        public void SolveRecursively(int n)
        {
            MoveDisks(n, 1, 3, 2);
        }

        private void MoveDisks(int n, int from, int to, int temp)
        {
            if (n == 1)
            {
                steps.Add($"{from} --> {to}");
                return;
            }

            MoveDisks(n - 1, from, temp, to);
            steps.Add($"{from} --> {to}"); //мб тут тоже GetRodName нужно использовать?
            MoveDisks(n - 1, temp, to, from);
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
    }
}
