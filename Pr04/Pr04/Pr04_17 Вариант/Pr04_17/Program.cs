using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Введите количество элементов массива: ");
            int n = int.Parse(Console.ReadLine());

            int[] array = new int[n];
            Console.WriteLine("Введите элементы массива:");
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Элемент {i + 1}: ");
                array[i] = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("\nИсходный массив:");
            PrintArray(array);
            int positiveCount = 0;
            foreach (int num in array)
            {
                if (num > 0)
                    positiveCount++;
            }
            Console.WriteLine($"\nКоличество положительных элементов: {positiveCount}");
            int lastZeroIndex = -1;
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (array[i] == 0)
                {
                    lastZeroIndex = i;
                    break;
                }
            }

            int sumAfterZero = 0;
            if (lastZeroIndex != -1 && lastZeroIndex < array.Length - 1)
            {
                for (int i = lastZeroIndex + 1; i < array.Length; i++)
                {
                    sumAfterZero += array[i];
                }
            }
            Console.WriteLine($"Сумма элементов после последнего нуля: {sumAfterZero}");
            List<int> lessOrEqualOne = new List<int>();
            List<int> greaterThanOne = new List<int>();

            foreach (int num in array)
            {
                if (num <= 1)
                    lessOrEqualOne.Add(num);
                else
                    greaterThanOne.Add(num);
            }
            List<int> transformed = new List<int>();
            transformed.AddRange(lessOrEqualOne);
            transformed.AddRange(greaterThanOne);

            Console.WriteLine("\nПреобразованный массив:");
            PrintArray(transformed.ToArray());
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: введено нецелое число.");
        }
        catch (OutOfMemoryException)
        {
            Console.WriteLine("Ошибка: недостаточно памяти.");
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("Ошибка: выход за границы массива.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
        }
    }

    static void PrintArray(int[] arr)
    {
        foreach (int num in arr)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
    }
}
