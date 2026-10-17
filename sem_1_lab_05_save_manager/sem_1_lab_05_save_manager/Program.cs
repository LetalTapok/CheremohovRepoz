using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace sem_1_lab_05_save_manager
{
    public class SaveEntry
    {
        public string Name { get; set; }

        [JsonProperty("File")]
        public string FileName { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class Position { public int X { get; set; } public int Y { get; set; } }

    public class SaveData
    {
        public string Character { get; set; }
        public string Map { get; set; }
        public Position Position { get; set; }
        public int Health { get; set; }
        public List<string> Inventory { get; set; }
    }

    public class Program
    {
        const string IdxPath = "index.json";
        const string SavesDir = "saves";
        const string BackupsDir = "backups";

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Directory.CreateDirectory(SavesDir);
            Directory.CreateDirectory(BackupsDir);

            List<SaveEntry> entries = LoadIndex(IdxPath);
            if (entries == null) return;

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Выберите действие:\n1 - Список\n2 - Просмотр\n3 - Изменение\n4 - Бэкап\n5 - Восстановить\n6 - Удалить\n7 - Выход");
                Console.Write("Выбор: ");

                string cmd = Console.ReadLine();

                if (cmd == "7") break;

                if (cmd == "1")
                {
                    Console.WriteLine("\n====СПИСОК СОХРАНЕНИЙ====");
                    foreach (var e in entries)
                        Console.WriteLine($"{e.Name} | {e.FileName} | {e.UpdatedAt:yyyy-MM-ddTHH:mm:ss}");
                }
                else if (cmd == "2" || cmd == "3" || cmd == "4" || cmd == "5" || cmd == "6")
                {
                    SaveEntry entry = AskEntry(entries);

                    if (entry != null)
                    {
                        if (cmd == "2")
                        {
                            var save = LoadSave(SavesDir, entry);
                            if (save != null) PrintSave(entry, save);
                        }
                        else if (cmd == "3")
                        {
                            var save = LoadSave(SavesDir, entry);
                            if (save != null)
                            {
                                EditSave(save);
                                entry.UpdatedAt = DateTime.Now;
                                SaveGame(SavesDir, entry, save);
                                SaveIndex(IdxPath, entries);
                                Console.WriteLine("\n[УСПЕХ] Изменения сохранены.");
                            }
                        }
                        else if (cmd == "4") CreateBackup(SavesDir, BackupsDir, entry);
                        else if (cmd == "5") RestoreBackup(SavesDir, BackupsDir, entry);
                        else if (cmd == "6") DeleteSave(SavesDir, IdxPath, entries, entry);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ОШИБКА] Неверный пункт меню. Введите число от 1 до 7.");
                    Console.ResetColor();
                }

                Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                Console.ReadKey(true);
            }
        }

        public static List<SaveEntry> LoadIndex(string path)
        {
            if (!File.Exists(path)) { Console.WriteLine("Нет index.json"); return null; }
            try { return JsonConvert.DeserializeObject<List<SaveEntry>>(File.ReadAllText(path)); }
            catch { Console.WriteLine("Ошибка в index.json"); return null; }
        }

        public static SaveData LoadSave(string dir, SaveEntry entry)
        {
            string path = Path.Combine(dir, entry.FileName);
            if (!File.Exists(path)) { Console.WriteLine("Файл не найден"); return null; }
            try { return JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(path)); }
            catch { Console.WriteLine("Ошибка в JSON сохранения"); return null; }
        }

        public static void SaveGame(string dir, SaveEntry entry, SaveData save)
        {
            File.WriteAllText(Path.Combine(dir, entry.FileName), JsonConvert.SerializeObject(save, Formatting.Indented));
        }

        public static void SaveIndex(string path, List<SaveEntry> entries)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(entries, Formatting.Indented));
        }

        public static void PrintSave(SaveEntry entry, SaveData save)
        {
            Console.WriteLine($"\n[{entry.Name}] {save.Character} | Карта: {save.Map} | X:{save.Position.X} Y:{save.Position.Y} | HP: {save.Health}");
            Console.WriteLine("Инвентарь: " + (save.Inventory != null ? string.Join(", ", save.Inventory) : "пусто"));
        }

        public static SaveEntry FindByName(List<SaveEntry> entries, string name)
        {
            foreach (var e in entries)
            {
                if (e.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                    e.FileName.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                    e.FileName.Equals(name + ".json", StringComparison.OrdinalIgnoreCase))
                {
                    return e;
                }
            }
            return null;
        }

        public static void CreateBackup(string savesDir, string backupDir, SaveEntry entry)
        {
            string src = Path.Combine(savesDir, entry.FileName);
            if (File.Exists(src))
            {
                File.Copy(src, Path.Combine(backupDir, entry.FileName), true);
                Console.WriteLine("Копия создана.");
            }
        }

        public static void RestoreBackup(string savesDir, string backupDir, SaveEntry entry)
        {
            string src = Path.Combine(backupDir, entry.FileName);
            if (File.Exists(src))
            {
                File.Copy(src, Path.Combine(savesDir, entry.FileName), true);
                Console.WriteLine("Восстановлено.");
            }
            else Console.WriteLine("Копия не найдена.");
        }

        public static void DeleteSave(string savesDir, string idxPath, List<SaveEntry> entries, SaveEntry entry)
        {
            string path = Path.Combine(savesDir, entry.FileName);
            if (File.Exists(path)) File.Delete(path);
            entries.Remove(entry);
            SaveIndex(idxPath, entries);
            Console.WriteLine("Удалено.");
        }

        public static SaveEntry AskEntry(List<SaveEntry> entries)
        {
            Console.Write("Введите имя сохранения: ");
            string name = Console.ReadLine();
            SaveEntry e = FindByName(entries, name);
            if (e == null) Console.WriteLine("Не найдено.");
            return e;
        }

        public static void EditSave(SaveData save)
        {
            Console.Write("Новая карта (Enter - пропустить): "); string m = Console.ReadLine(); if (m != "") save.Map = m;
            Console.Write("Новый X: "); string x = Console.ReadLine(); if (int.TryParse(x, out int nx)) save.Position.X = nx;
            Console.Write("Новый Y: "); string y = Console.ReadLine(); if (int.TryParse(y, out int ny)) save.Position.Y = ny;
            Console.Write("Новое HP: "); string h = Console.ReadLine(); if (int.TryParse(h, out int nh)) save.Health = nh;
            Console.Write("Инвентарь (через запятую): "); string inv = Console.ReadLine();
            if (inv != "") save.Inventory = new List<string>(inv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}