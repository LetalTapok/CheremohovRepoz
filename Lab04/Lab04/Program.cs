using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace sem_1_lab_04_event_log
{
    // запись журнала, только данные
    public class LogEntry
    {
        public DateTime Timestamp;
        public string Level;
        public string Category;
        public string Message;
    }

    public class Program
    {
        // разбор строк лога в массив записей
        public static LogEntry[] ParseLog(string[] lines)
        {
            LogEntry[] entries = new LogEntry[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                LogEntry entry = new LogEntry();

                // первые 23 символа это дата и время
                entry.Timestamp = DateTime.ParseExact(
                    line.Substring(0, 23),
                    "yyyy-MM-dd HH:mm:ss.fff",
                    CultureInfo.InvariantCulture);

                // первая пара скобок это уровень
                int firstOpenBracket = line.IndexOf('[');
                int firstCloseBracket = line.IndexOf(']', firstOpenBracket);

                // вторая пара скобок это категория
                int secondOpenBracket = line.IndexOf('[', firstCloseBracket);
                int secondCloseBracket = line.IndexOf(']', secondOpenBracket);

                entry.Level = line.Substring(firstOpenBracket + 1, firstCloseBracket - firstOpenBracket - 1);
                entry.Category = line.Substring(secondOpenBracket + 1, secondCloseBracket - secondOpenBracket - 1);
                entry.Message = line.Substring(secondCloseBracket + 2);

                entries[i] = entry;
            }

            return entries;
        }

        // фильтр по дате
        public static LogEntry[] FilterByDate(LogEntry[] entries, DateTime date)
        {
            List<LogEntry> result = new List<LogEntry>();

            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Timestamp.Date == date.Date)
                {
                    result.Add(entries[i]);
                }
            }

            return result.ToArray();
        }

        // фильтр по уровню
        public static LogEntry[] FilterByLevel(LogEntry[] entries, string level)
        {
            List<LogEntry> result = new List<LogEntry>();

            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Level == level)
                {
                    result.Add(entries[i]);
                }
            }

            return result.ToArray();
        }

        // фильтр по категории
        public static LogEntry[] FilterByCategory(LogEntry[] entries, string category)
        {
            List<LogEntry> result = new List<LogEntry>();

            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Category == category)
                {
                    result.Add(entries[i]);
                }
            }

            return result.ToArray();
        }

        // поиск по тексту без учёта регистра
        public static LogEntry[] Search(LogEntry[] entries, string text)
        {
            List<LogEntry> result = new List<LogEntry>();

            for (int i = 0; i < entries.Length; i++)
            {
                string message = entries[i].Message.ToLower();
                string needle = text.ToLower();

                if (message.Contains(needle))
                {
                    result.Add(entries[i]);
                }
            }

            return result.ToArray();
        }

        // подсчёт записей уровня
        public static int CountByLevel(LogEntry[] entries, string level)
        {
            int count = 0;

            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Level == level)
                {
                    count++;
                }
            }

            return count;
        }

        // статус сервера
        public static string GetServerStatus(LogEntry[] entries)
        {
            bool hasError = false;
            bool hasFatal = false;
            bool fatalOnServer = false;

            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Level == "Error") hasError = true;

                if (entries[i].Level == "Fatal")
                {
                    hasFatal = true;
                    if (entries[i].Category == "Server") fatalOnServer = true;
                }
            }

            if (fatalOnServer)
                return "КРИТИЧЕСКАЯ ОШИБКА: сервер остановлен";
            if (hasError || hasFatal)
                return "требуется проверка";
            return "Сервер работает штатно";
        }

        // сохранить записи в исходном формате
        public static void ExportFiltered(string path, LogEntry[] entries)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < entries.Length; i++)
            {
                sb.Append(entries[i].Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                sb.Append(" [");
                sb.Append(entries[i].Level);
                sb.Append("][");
                sb.Append(entries[i].Category);
                sb.Append("] ");
                sb.AppendLine(entries[i].Message);
            }

            File.WriteAllText(path, sb.ToString());
        }

        // точка входа
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string[] lines = File.ReadAllLines("event_server.log");
            LogEntry[] entries = ParseLog(lines);

            Console.WriteLine("Всего записей: " + entries.Length);
            Console.WriteLine("Info:          " + CountByLevel(entries, "Info"));
            Console.WriteLine("Warning:       " + CountByLevel(entries, "Warning"));
            Console.WriteLine("Error:         " + CountByLevel(entries, "Error"));
            Console.WriteLine("Fatal:         " + CountByLevel(entries, "Fatal"));
            Console.WriteLine();
            Console.WriteLine("Статус: " + GetServerStatus(entries));

            // примеры фильтров
            Console.WriteLine();
            Console.WriteLine("--- Предупреждения ---");
            LogEntry[] warnings = FilterByLevel(entries, "Warning");
            for (int i = 0; i < warnings.Length; i++)
            {
                Console.WriteLine(warnings[i].Timestamp.ToString("HH:mm:ss") + "  " + warnings[i].Message);
            }
        }
    }
}