using System;
using System.IO;
using System.Windows.Forms;

namespace Lab1.Inputs
{
    internal class ToleranceInput
    {
        public static double GetTolerance(bool tryFile, string[] args)
        {
            if (tryFile)
            {
                try
                {
                    int index = Array.IndexOf(args, "--tolerance") + 1;
                    if (index < args.Length) // Read from file in args
                    {
                        (bool success, double tolerance) = GetToleranceFile(args[index]);
                        if (success)
                            return tolerance;
                        return GetTolerance(false, args);
                    }
                }
                catch (Exception)
                {
                    return GetTolerance(false, args);
                }
            }
            Console.Write("Загрузить точность из файла? (y/n): ");
            string answ = Console.ReadLine().ToLower();
            if (answ == "y")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выберите файл с точностью";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    (bool success, double tolerance) = GetToleranceFile(openFileDialog.FileName);
                    if (success)
                        return tolerance;
                }
                return GetTolerance(tryFile, args);
            }
            else if (answ == "n")
            {
                return GetToleranceKeyboard();
            }
            return GetTolerance(tryFile, args);
        }
        static (bool, double) GetToleranceFile(string fileName)
        {
            try
            {
                string tolerance = File.ReadAllText(fileName).Replace(".",",");
                return (true, double.Parse(tolerance));
            }
            catch (Exception)
            {
                Console.WriteLine("Файл не найден, либо в нем указаны неверные данные.");
            }
            return (false, 0f);
        }
        static double GetToleranceKeyboard()
        {
            double tol;
            try
            {
                Console.Write("Введите точность: ");
                tol = double.Parse(Console.ReadLine().Replace(".", ","));
            }
            catch (Exception)
            {
                Console.WriteLine("Точность введена неверно.");
                return GetToleranceKeyboard();
            }
            return tol;
        }
    }
}
