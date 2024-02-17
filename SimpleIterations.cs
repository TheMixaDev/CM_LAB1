using System;
using System.Linq;
using static Lab1.MatrixUtils;

namespace Lab1
{
    internal class SimpleIterations
    {
        public static void DiagonalPivot(double[][] A, double[] b)
        {
            for(int i = 0; i < A.Length; i++)
            {
                int max_row = i;
                for(int j = i + 1; j < A.Length; j++)
                {
                    if (Math.Abs(A[j][i]) > Math.Abs(A[max_row][i]))
                        max_row = j;
                }
                double[] tempA = A[i];
                A[i] = A[max_row];
                A[max_row] = tempA;

                double tempB = b[i];
                b[i] = b[max_row];
                b[max_row] = tempB;
            }
        }

        public static bool IsDiagonalPivot(double[][] A)
        {
            for (int i = 0; i < A.Length; i++)
            {
                double rowSum = A[i].Where((value, j) => j != i).Sum(Math.Abs);
                if (Math.Abs(A[i][i]) <= rowSum)
                    return false;
            }
            return true;
        }

        public static (double[], double[], int) SimpleIteration(double[][] A, double[] b, double tol, int max_iter)
        {
            double[][] C = MatrixCreate(A.Length, A.Length);
            for (int i = 0; i < C.Length; i++)
            {
                for (int j = 0; j < C.Length; j++)
                {
                    if (i == j) C[i][j] = 0;
                    else C[i][j] = -A[i][j] / A[i][i];
                }
            }
            Console.WriteLine("Матрица C: ");
            C.All(row =>
            {
                Console.WriteLine(string.Join(" ", row.Select(v => v.ToString("F2"))));
                return true; // C# hack
            });
            Console.WriteLine("");
            double[] d = new double[A.Length];
            for (int i = 0; i < d.Length; i++)
                d[i] = b[i] / A[i][i];
            Console.WriteLine("Вектор d: ");
            Console.WriteLine(string.Join(" ", d.Select(v => v.ToString("F2"))));
            Console.WriteLine("");
            double[] x = ZeroVector(A.Length);
            double lastMaxDiff = double.MaxValue;

            for (int itr = 0; itr < max_iter; itr++)
            {
                double[] xNew = ZeroVector(d.Length);
                for(int i = 0; i < d.Length; i++)
                {
                    for(int j = 0; j < d.Length; j++)
                        xNew[i] += C[i][j] * x[j];
                    xNew[i] += d[i];
                }
                double maxDiff = 0;
                for (int i = 0; i < x.Length; i++)
                    maxDiff = Math.Max(Math.Abs(xNew[i] - x[i]), maxDiff);
                bool overflow = xNew.Select(v => double.IsInfinity(v) || double.IsNaN(v)).ToList().Contains(true);
                Console.WriteLine($"Итерация {itr + 1}: {string.Join(", ", xNew)}");
                if (maxDiff < tol || overflow || itr == max_iter - 1 || maxDiff > lastMaxDiff)
                {
                    double[] errors = new double[x.Length];
                    for (int i = 0; i < x.Length; i++)
                        errors[i] = Math.Abs(xNew[i] - x[i]);
                    if (overflow || maxDiff > lastMaxDiff)
                    {
                        Console.WriteLine("Решение не сходится. Вычисления остановлены");
                        throw new InvalidOperationException();
                    }
                    if(itr == max_iter - 1)
                        Console.WriteLine("Достигнут лимит итераций.");
                    return (xNew, errors, itr + 1);
                }
                lastMaxDiff = maxDiff;
                x = xNew;
            }
            return (x, new double[A.Length], max_iter);
        }

        public static bool CheckInputValues(double[][] A, double[] b, out int maxIter)
        {
            maxIter = 1000000;
            try
            {
                if (MatrixDeterminant(A) == 0)
                {
                    Console.WriteLine("Метод неприменим к вырожденной матрице");
                    return false;
                }

                if (!IsDiagonalPivot(A))
                {
                    Console.WriteLine("Преобразование матрицы для соответствия диагональному преобладанию");
                    DiagonalPivot(A, b);
                    if (IsZero(A))
                    {
                        Console.WriteLine("После преобразования обнаружены нули на диагональных элементах");
                        Console.Write("Введите число итераций: ");
                        maxIter = int.Parse(Console.ReadLine());
                    }
                    foreach (var row in A)
                        Console.WriteLine(string.Join(" ", row));
                }
                Console.WriteLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Неверные входные данные");
                return false;
            }
            return true;
        }
    }
}
