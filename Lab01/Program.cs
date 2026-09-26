using System;

namespace ServerWatch
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // шапка
            Console.WriteLine("=================================================");
            Console.WriteLine("    ТЕРМИНАЛ АДМИНИСТРАТОРА ИГРОВОГО СЕРВЕРА");
            Console.WriteLine("=================================================");
            Console.WriteLine();

            // проверка доступа - верный код - 850214763
            Console.Write("Введите код доступа: ");
            long accessCode;

            
            if (!long.TryParse(Console.ReadLine(), out accessCode))
            {
                Console.WriteLine();
                Console.WriteLine("[!] Некорректный ввод. Нужно ввести число.");
                return;
            }

            
            if (accessCode != 850214763)
            {
                Console.WriteLine();
                Console.WriteLine("[!] Доступ запрещён. Неверный код.");
                return;
            }
            // запуск
            //   byte   — пинг
            //   short  — число игроков
            //   double — время и скорость сети
            //   bool   — ночной режим
            byte pingNow = 28;
            short playersNow = 1874;
            double uptimeNow = 62.75;
            double speedNow = 913.6;
            bool isNightNow = false;

            Console.WriteLine();
            Console.WriteLine("+============= ТЕКУЩЕЕ СОСТОЯНИЕ =============+");
            Console.WriteLine("| Пинг            : " + pingNow + " мс");
            Console.WriteLine("| Игроков онлайн  : " + playersNow);
            Console.WriteLine("| Uptime          : " + uptimeNow + " ч");
            Console.WriteLine("| Скорость сети   : " + speedNow + " Мбит/с");
            Console.WriteLine("| Ночной режим    : " + (isNightNow ? "включён" : "выключен"));
            Console.WriteLine("+=============================================+");

            // сравнение
            Console.WriteLine();
            Console.Write("Сравнить с предыдущим запуском? (Y/N): ");
            string answer = Console.ReadLine();

            if (answer == "Y" || answer == "y")
            {
                // данные прошлого запуска
                byte pingPrev = 94;
                short playersPrev = 1301;
                double speedPrev = 604.2;

                // разница
                int pingDiff = pingPrev - pingNow;
                int playersDiff = playersNow - playersPrev;
                double speedDiff = speedNow - speedPrev;

                Console.WriteLine();
                Console.WriteLine("+=============== ДИНАМИКА ====================+");

                Console.WriteLine("| Пинг       : " + pingDiff.ToString().PadLeft(5) + " мс   "
                    + (pingDiff > 0 ? "стало лучше" : "стало хуже"));

                Console.WriteLine("| Игроков    : " + playersDiff.ToString().PadLeft(5) + "      "
                    + (playersDiff > 0 ? "прирост" : "отток"));

                Console.WriteLine("| Скорость   : " + speedDiff.ToString("F1").PadLeft(5)
                    + " Мбит/с " + (speedDiff > 0 ? "рост" : "падение"));

                Console.WriteLine("+=============================================+");

                // итог
                bool isBetter = (pingNow < pingPrev) && (speedNow > speedPrev);
                bool isWorse = (pingNow > pingPrev) || (speedNow < speedPrev);

                Console.WriteLine();
                if (isBetter)
                    Console.WriteLine(">>> ИТОГ: сервер работает ЛУЧШЕ прошлого запуска.");
                else if (isWorse)
                    Console.WriteLine(">>> ИТОГ: показатели УХУДШИЛИСЬ, проверьте каналы.");
                else
                    Console.WriteLine(">>> ИТОГ: состояние СТАБИЛЬНОЕ.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(">>> Сравнение пропущено. Хорошей смены!");
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}