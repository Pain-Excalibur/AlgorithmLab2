using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AlgorithmLab2
{
    public class TowerOfHanoi
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

        public void SolveIteratively(int n)
        {
            canvas.Children.Clear();
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
            }

            textBox.Text = string.Join(Environment.NewLine, steps);
        }

        public void SolveRecursively(int n)
        {
            canvas.Children.Clear();
            steps.Clear();

            MoveDisks(n, 1, 3, 2);

            textBox.Text = string.Join(Environment.NewLine, steps);
        }

        private void MoveDisks(int n, int from, int to, int temp)
        {
            if (n == 1)
            {
                steps.Add($"{from} --> {to}");
                return;
            }

            MoveDisks(n - 1, from, temp, to);
            steps.Add($"{from} --> {to}");
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
