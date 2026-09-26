using System;

namespace pracl
{
    class Program
    {
        public static int ConvertToNumber(string buffer)
        {
            int result = 0;
            int length = buffer.Length;

            for (int i = 0; i < length; i++)
            {
                int digit = buffer[i] - '0';
                int step = length - i - 1;
                result += digit * (int)Math.Pow(10, step);
            }

            return result;
        }

        public static void Main()
        {
            Console.Write("Введите число: ");
            int num = ConvertToNumber(Console.ReadLine());
            Console.WriteLine($"Результат: {num}");
        }
    }
}