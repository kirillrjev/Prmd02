using System;

class Program
{
    static void Main()
    {
        try
        {
            
            Console.WriteLine("Введите количество элементов массива:");
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
            SelectionSort(array);
            Console.WriteLine("\nОтсортированный массив (по возрастанию):");
            PrintArray(array);
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: введено некорректное число.");
        }
        catch (OutOfMemoryException)
        {
            Console.WriteLine("Ошибка: недостаточно памяти для создания массива.");
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
    static void SelectionSort(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }

            int temp = arr[minIndex];
            arr[minIndex] = arr[i];
            arr[i] = temp;
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
