using System;
using static Lab1.SimpleIterations;
using Lab1.Inputs;

namespace Lab1
{
    internal class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            double[][] matrix = MatrixInput.GetMatrix(true, args);
            double[] vector = VectorInput.GetVector(true, args, matrix.Length);
            double tolerance = ToleranceInput.GetTolerance(true, args);
            int maxIter;
            CheckInputValues(matrix, vector, out maxIter);
            try
            {
                (double[] solution, double[] errors, int iterations) = SimpleIteration(matrix, vector, tolerance, maxIter);
                Console.WriteLine($"\nРешение найдено за {iterations} итераций");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Решение: {string.Join(", ", solution)}");
                Console.WriteLine($"Вектор погрешностей: {string.Join(", ", errors)}");
            }
            catch (InvalidOperationException) { }
            Console.Read();
        }
    }
}
