using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Lab1.Inputs
{
    internal class MatrixInput
    {
        public static double[][] GetMatrix(bool tryFile, string[] args)
        {
            if (tryFile)
            {
                try
                {
                    int index = Array.IndexOf(args, "--matrix") + 1;
                    if (index < args.Length) // Read from file in args
                    {
                        (bool success, double[][] matrix) = GetMatrixFile(args[index]);
                        if (success)
                            return matrix;
                        return GetMatrix(false, args);
                    }
                }
                catch (Exception)
                {
                    return GetMatrix(false, args);
                }
            }
            Console.Write("Загрузить матрицу из файла? (y/n): ");
            string answ = Console.ReadLine().ToLower();
            if (answ == "y")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выберите файл с матрицей";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    (bool success, double[][] matrix) = GetMatrixFile(openFileDialog.FileName);
                    if (success)
                        return matrix;
                }
                return GetMatrix(tryFile, args);
            }
            else if (answ == "n")
            {
                return GetMatrixKeyboard();
            }
            else if (answ == "r")
            {
                return GetRandomMatrix();
            }
            return GetMatrix(tryFile, args);
        }
        static (bool, double[][]) GetMatrixFile(string fileName)
        {
            try
            {
                string[] matrix = File.ReadAllLines(fileName);
                double[][] transformed = matrix.Select(line => line.Replace(".",",").Split().Select(double.Parse).ToArray()).ToArray();
                if(transformed.Length > 20)
                {
                    Console.WriteLine("Размер матрицы не должен превышать 20");
                    return (false, null);
                } else if(transformed.Any(row => row.Length != transformed.Length))
                {
                    Console.WriteLine("Матрица должна быть квадратной");
                    return (false, null);
                }
                    return (true, transformed);
            }
            catch (Exception)
            {
                Console.WriteLine("Файл не найден, либо в нем указаны неверные данные.");
            }
            return (false, null);
        }
        static double[][] GetMatrixKeyboard()
        {
            double[][] A = new double[0][];
            try
            {
                Console.Write("Введите размерность матрицы: ");
                int n = int.Parse(Console.ReadLine());
                if(n > 20)
                {
                    Console.WriteLine("Размер матрицы не должен превышать 20");
                    return GetMatrixKeyboard();
                }
                A = new double[n][];
                for (int i = 0; i < n; i++)
                {
                    Console.Write($"Введите строку {i + 1}: ");
                    A[i] = Console.ReadLine().Replace(".", ",").Split().Select(double.Parse).ToArray();
                    if (A[i].Length != n) throw new Exception();
                }
                return A;
            }
            catch (Exception)
            {
                Console.WriteLine("Матрица введена неверно.");
            }
            return GetMatrixKeyboard();
        }
        static double[][] GetRandomMatrix()
        {
            Console.WriteLine("Генерация случайной матрицы.");
            double[][] A = new double[0][];
            try
            {
                Console.Write("Введите размерность матрицы: ");
                int n = int.Parse(Console.ReadLine());
                if(n > 20)
                {
                    Console.WriteLine("Размер матрицы не должен превышать 20");
                    return GetRandomMatrix();
                }
                Console.Write("Введите верхнюю границу значения матрицы (целочисленное): ");
                int top = int.Parse(Console.ReadLine());
                A = new double[n][];
                Random random = new Random();
                for (int i = 0; i < n; i++)
                {
                    A[i] = new double[n];
                    for (int j = 0; j < n; j++)
                        A[i][j] = random.Next(top * 2) - top;
                }
                Console.WriteLine("Сгенерированная матрица:");
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine(string.Join(" ", A[i]));
                }
                return A;
            }
            catch (Exception)
            {
                Console.WriteLine("Введены неверные числовые значения.");
            }
            return GetRandomMatrix();
        }
    }
}
