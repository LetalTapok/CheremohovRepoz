using System;
using System.IO;
using System.Globalization;

namespace TempProject
{
    internal class Program
    {
        // 2026-09-01 12:16:01.101 [Info][User] User logged in: RavenFox
        // 2026-09-01 12:16:07.893 [Info][Server] Event "Frostfire Rebellion" scheduled to start in 2 hours
        // 2026-09-01 12:18:25.105 [Info][Event] Event started

        public static DateTime GetLogDateTime(string line)
        {
            // 2026-09-01 12:16:01.101 [Info][User] User logged in: RavenFox

            int index1 = line.IndexOf(' ');
            int index2 = line.IndexOf(' ', index1 + 1);

            string datetimeStr = line.Substring(0, index2);

            DateTime dateTime = DateTime.ParseExact(
                datetimeStr,
                "yyyy-MM-dd HH:mm:ss.fff",
                CultureInfo.InvariantCulture);

            return dateTime;
        }

        public static string GetLogLevel(string line)
        {
            // 2026-09-01 12:16:01.101 [Info][User] User logged in: RavenFox
            int index1 = line.IndexOf('[');
            int index2 = line.IndexOf(']');
            string ressult = line.Substring(index1, index2 - index1 + 1);
            return ressult;
        }

        public static string GetLogType(string line)
        {
            // 2026-09-01 12:16:01.101 [Info][User] User logged in: RavenFox
            int indexh1 = line.IndexOf('[');
            int indexh2 = line.IndexOf(']');
            int index1 = line.IndexOf('[', indexh1 + 1);
            int index2 = line.IndexOf(']', indexh2 + 1);
            string ressult = line.Substring(index1, index2 - index1 + 1);
            return ressult;
        }

        public static string GetLogText(string line)
        {
            // 2026-09-01 12:16:01.101 [Info][User] User logged in: RavenFox
            int indexh1 = line.IndexOf(']');
            indexh1 = line.IndexOf(' ', indexh1);
            string ressult = line.Substring(indexh1 + 1, line.Length - indexh1 - 1);
            return ressult;
        }

        public static void Main()
        {
            string[] lines = File.ReadAllLines("event_server.log");

            foreach (string line in lines)
            {
                // 2026-09-01 12:16:01.101 [Info][User] User logged in: RavenFox
                // date time [level][type] text

                DateTime dt = GetLogDateTime(line);
                Console.WriteLine(dt);

                string level = GetLogLevel(line);
                Console.WriteLine(level);

                string type = GetLogType(line);
                Console.WriteLine(type);

                string text = GetLogText(line);
                Console.WriteLine(text);

                Console.WriteLine();  
            }
        }
    }
}