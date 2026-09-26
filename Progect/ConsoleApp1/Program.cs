using System;

namespace GameProfile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("========================================");
            Console.WriteLine("       СОЗДАНИЕ ИГРОВОГО ПРОФИЛЯ        ");
            Console.WriteLine("========================================");
            Console.WriteLine();

            
            Console.Write("Введите ваш никнейм: ");
            string nickname = Console.ReadLine();
            Console.Write("Введите ваше реальное имя: ");
            string realName = Console.ReadLine();
            Console.Write("Введите вашу любимую игру: ");
            string favoriteGame = Console.ReadLine();
            Console.Write("Введите название вашего любимого класса: ");
            string playerClass = Console.ReadLine();

            
            Console.Write("Введите ваш возраст: ");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите ваш игровой стаж (в годах): ");
            int experience = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите количество часов, проведённых в играх: ");
            int hoursPlayed = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите ваш текущий уровень: ");
            int level = Convert.ToInt32(Console.ReadLine());

            
            string rank;
            if (level < 50)
                rank = "Новичок";
            else if (level < 300)
                rank = "Опытный";
            else if (level < 800)
                rank = "Ветеран";
            else
                rank = "Легенда";

            
            Console.Clear();


            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║           ПАСПОРТ ИГРОВОГО ПРОФИЛЯ           ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine($"║  Никнейм:         {nickname,-26}║");
            Console.WriteLine($"║  Реальное имя:    {realName,-26}║");
            Console.WriteLine($"║  Возраст:         {age + " лет",-26}║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine($"║  Класс персонажа: {playerClass,-26}║");
            Console.WriteLine($"║  Текущий уровень: {level,-26}║");
            Console.WriteLine($"║  Ранг:            {rank,-26}║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine($"║  Игровой стаж:    {experience + " лет",-26}║");
            Console.WriteLine($"║  Наиграно часов:  {hoursPlayed,-26}║");
            Console.WriteLine($"║  Любимая игра:    {favoriteGame,-26}║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  Статус:          Активный игрок             ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine($"Добро пожаловать в игру, {nickname}!");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}