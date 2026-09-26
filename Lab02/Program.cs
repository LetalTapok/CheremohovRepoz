using System;
using System.Collections.Generic;

namespace lab02

{

public class Program
{
    
    public static string CheckConfiguration(int players, int memory, bool isPublic, bool hasPassword)
    {
        

        if (players <= 0)
            return "Запуск невозможен: количество игроков должно быть больше нуля.";

        if (memory < 2)
            return "Запуск невозможен: серверу недостаточно оперативной памяти.";

       

        if (isPublic && hasPassword)
            return "Запуск возможен с предупреждением: публичный сервер защищён паролем.";

        
        if (memory * 25 < players)
            return "Запуск возможен с предупреждением: для такого количества игроков рекомендуется больше оперативной памяти.";

       
        return "Сервер готов к запуску.";
    }

    

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=========================================");
        Console.WriteLine("    ПРОВЕРКА КОНФИГУРАЦИИ СЕРВЕРА");
        Console.WriteLine("=========================================");
        Console.WriteLine();

        Console.Write("Количество игроков: ");
        int players = int.Parse(Console.ReadLine());

        Console.Write("Оперативной памяти (ГБ): ");
        int memory = int.Parse(Console.ReadLine());

        Console.Write("Сервер публичный? (да/нет): ");
        bool isPublic = Console.ReadLine().ToLower() == "да";

        Console.Write("Пароль установлен? (да/нет): ");
        bool hasPassword = Console.ReadLine().ToLower() == "да";

        Console.WriteLine();

        string result = CheckConfiguration(players, memory, isPublic, hasPassword);
        Console.WriteLine(result);
    }
}

}
