using System;
using System.Globalization;
using System.IO;

namespace sem_1_lab_03_report_generator
{
    public class Program
    {
        public static string BuildReport(string[] lines)
        {
            string result = "";

            bool isEventRunning = false;
            DateTime eventDate = DateTime.MinValue;
            string winner = "";
            int winnerPoints = 0;
            string questItem = "";
            string consolation = "";
            int warnings = 0;
            int errors = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

               
                if (line.Contains("Событие началось:"))
                {
                    isEventRunning = true;

                    
                    string dateStr = line.Substring(0, 10);
                    eventDate = DateTime.ParseExact(
                        dateStr,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                    continue;
                }

                
                if (isEventRunning && line.Contains("закрыто"))
                {
                    break;
                }

                
                if (!isEventRunning) continue;

                
                if (line.Contains("объявлены победителями"))
                {
                    int messageStart = line.IndexOf("] ") + 2;
                    int winnerEnd = line.IndexOf(" объявлены победителями");
                    winner = line.Substring(messageStart, winnerEnd - messageStart);
                }

                
                if (winner != "")
                {
                    string startText = winner + " получили ";
                    int start = line.IndexOf(startText);

                    if (start >= 0)
                    {
                        start += startText.Length;
                        int end = line.IndexOf(" очков", start);
                        winnerPoints = int.Parse(line.Substring(start, end - start));
                    }
                }

               
                if (line.Contains("ивентовый предмет:"))
                {
                    string marker = "ивентовый предмет: ";
                    int start = line.IndexOf(marker) + marker.Length;
                    questItem = line.Substring(start);
                }

                
                if (line.Contains("утешительную награду:"))
                {
                    int msgStart = line.IndexOf("] ") + 2;
                    int gotIndex = line.IndexOf(" получили");

                    int pointsStart = line.IndexOf(": ", gotIndex) + 2;
                    int pointsEnd = line.IndexOf(" очков", pointsStart);

                    string teamName = line.Substring(msgStart, gotIndex - msgStart);
                    string pointsStr = line.Substring(pointsStart, pointsEnd - pointsStart);

                    consolation = teamName + ": " + pointsStr + " очков";
                }

                
                if (line.Contains("[Warning]")) warnings++;
                if (line.Contains("[Error]")) errors++;
            }

            
            result += "# Итоги события: Восстание Ледяного Пламени\n\n";
            result += "Дата: " + eventDate.ToString("dd.MM.yyyy") + "\n";
            result += "Победитель: " + winner + "\n";
            result += "Очки победителя: " + winnerPoints + "\n";
            result += "Ивентовый предмет: " + questItem + "\n";
            result += "Утешительная награда " + consolation + "\n";
            result += "Предупреждений во время события: " + warnings + "\n";
            result += "Ошибок во время события: " + errors;

            return result;
        }

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string[] lines = File.ReadAllLines("event_server.log");
            Console.WriteLine(BuildReport(lines));
        }
    }
}