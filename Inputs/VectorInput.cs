using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Lab1.Inputs
{
    internal class VectorInput
    {
        public static double[] GetVector(bool tryFile, string[] args, int size)
        {
            if (tryFile)
            {
                try
                {
                    int index = Array.IndexOf(args, "--vector") + 1;
                    if (index < args.Length) // Read from file in args
                    {
                        (bool success, double[] vector) = GetVectorFile(args[index], size);
                        if (success)
                            return vector;
                        return GetVector(false, args, size);
                    }
                }
                catch (Exception)
                {
                    return GetVector(false, args, size);
                }
            }
            Console.Write("Загрузить вектор из файла? (y/n): ");
            string answ = Console.ReadLine().ToLower();
            if (answ == "y")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выберите файл с вектором";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    (bool success, double[] vector) = GetVectorFile(openFileDialog.FileName, size);
                    if (success)
                        return vector;
                }
                return GetVector(tryFile, args, size);
            }
            else if (answ == "n")
            {
                return GetVectorKeyboard(size);
            }
            return GetVector(tryFile, args, size);
        }
        static (bool, double[]) GetVectorFile(string fileName, int size)
        {
            try
            {
                string vector = File.ReadAllText(fileName);
                double[] transformed = vector.Replace(".", ",").Split().Select(double.Parse).ToArray();
                if(transformed.Length != size)
                {
                    Console.WriteLine("Размерность вектора не соответствует размерности матрицы.");
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
        static double[] GetVectorKeyboard(int size)
        {
            double[] b = new double[0];
            try
            {
                Console.Write("Введите вектор: ");
                b = Console.ReadLine().Replace(".", ",").Split().Select(double.Parse).ToArray();
                if (b.Length != size)
                {
                    Console.WriteLine("Размерность вектора не соответствует размерности матрицы.");
                    return GetVectorKeyboard(size);
                }
                return b;
            }
            catch (Exception)
            {
                Console.WriteLine("Вектор введен неверно.");
            }
            return GetVectorKeyboard(size);
        }
    }
}
