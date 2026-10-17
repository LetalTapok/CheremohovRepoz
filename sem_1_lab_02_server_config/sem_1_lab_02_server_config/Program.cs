using System;

namespace sem_1_lab_02_server_config
{
    public class Program
    {
        
        public static string CheckConfiguration(int players, int ram, bool isPublic, bool hasPassword)
        {
            
            if (players <= 0)
            {
                return "Запуск невозможен: количество игроков должно быть больше нуля.";
            }

            if (ram < 2)
            {
                return "Запуск невозможен: серверу недостаточно оперативной памяти.";
            }

            
            if (isPublic && hasPassword)
            {
                return "Запуск возможен с предупреждением: публичный сервер защищён паролем.";
            }

            
            if (players > ram * 25)
            {
                return "Запуск возможен с предупреждением: для такого количества игроков рекомендуется больше оперативной памяти.";
            }

            
            return "Сервер готов к запуску.";
        }

        
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Проверка тестовых сценариев:");
            Console.WriteLine();

            Console.WriteLine("Тест 1 (50, 8, true, false):");
            Console.WriteLine(CheckConfiguration(50, 8, true, false));
            Console.WriteLine();

            Console.WriteLine("Тест 2 (0, 8, true, false):");
            Console.WriteLine(CheckConfiguration(0, 8, true, false));
            Console.WriteLine();

            Console.WriteLine("Тест 3 (50, 1, true, false):");
            Console.WriteLine(CheckConfiguration(50, 1, true, false));
            Console.WriteLine();

            Console.WriteLine("Тест 4 (50, 8, true, true):");
            Console.WriteLine(CheckConfiguration(50, 8, true, true));
            Console.WriteLine();

            Console.WriteLine("Тест 5 (150, 4, true, false):");
            Console.WriteLine(CheckConfiguration(150, 4, true, false));
            Console.WriteLine();

            Console.WriteLine("Тест 6 (0, 8, true, true):");
            Console.WriteLine(CheckConfiguration(0, 8, true, true));

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}