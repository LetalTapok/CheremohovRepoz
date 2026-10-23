using System;
using System.IO;
using System.Timers;

public class ServerMonitor
{
    private Random _random;
    private Timer _timer;
    private double _cpu = 35;
    private double _ram = 42;
    private double _temperature = 48;

    public event Action<double, double, double> DataReceived;

    public ServerMonitor()
    {
        _random = new Random();
        _timer = new Timer(1000);
        _timer.Elapsed += Timer_Elapsed;
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        GenerateData();
    }

    private void GenerateData()
    {
        _cpu += _random.NextDouble() * 10 - 5;
        _ram += _random.NextDouble() * 6 - 3;
        _temperature += _random.NextDouble() * 4 - 2;

        _cpu = Math.Max(0, Math.Min(100, _cpu));
        _ram = Math.Max(10, Math.Min(100, _ram));
        _temperature = Math.Max(30, Math.Min(95, _temperature));

        if (DataReceived != null)
        {
            DataReceived(
                Math.Round(_cpu, 1),
                Math.Round(_ram, 1),
                Math.Round(_temperature, 1));
        }
    }
}

public class Monitor
{
    private static int[] cpuHistory = new int[20];
    private static int[] ramHistory = new int[20];
    private static int[] tempHistory = new int[20];

    private static int scaleSeconds = 1;
    private static int tickCounter = 0;

    public static string RenderGraph(int[] data)
    {
        if (data == null || data.Length == 0) return new string('.', 20);

        int latestValue = data[data.Length - 1];
        int filled = (int)Math.Round(latestValue / 5.0);

        if (filled > 20) filled = 20;
        if (filled < 0) filled = 0;

        return new string('#', filled).PadRight(20, '.');
    }

    private static void ResetHistory()
    {
        cpuHistory = new int[20];
        ramHistory = new int[20];
        tempHistory = new int[20];
        tickCounter = 0;
    }

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine("        МОНИТОРИНГ СЕРВЕРА");
            Console.WriteLine("==========================================");
            Console.WriteLine("1. Запустить симулятор в реальном времени");
            Console.WriteLine("2. Загрузить данные из measurements.txt");
            Console.WriteLine("3. Выйти");
            Console.WriteLine("==========================================");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            if (choice == "3") break;

            if (choice == "1")
            {
                ResetHistory();
                Console.WriteLine("\nВыберите масштаб графика (сколько секунд на одну колонку):");
                Console.WriteLine("1 - 10 секунд");
                Console.WriteLine("2 - 1 минута (60 сек)");
                Console.WriteLine("3 - 5 минут (300 сек)");
                Console.Write("Ваш выбор (1/2/3): ");

                string scaleChoice = Console.ReadLine();
                if (scaleChoice == "1") scaleSeconds = 10;
                else if (scaleChoice == "2") scaleSeconds = 60;
                else if (scaleChoice == "3") scaleSeconds = 300;
                else scaleSeconds = 1;

                ServerMonitor server = new ServerMonitor();
                server.DataReceived += OnDataReceived;

                Console.WriteLine("\nМониторинг запущен. Нажмите Enter для остановки.");
                server.Start();

                Console.ReadLine();
                server.Stop();
                Console.WriteLine("\nМониторинг остановлен. Нажмите любую клавишу для возврата в меню...");
                Console.ReadKey(true);
            }
            else if (choice == "2")
            {
                ResetHistory();
                LoadFromFile("measurements.txt");
                Console.WriteLine("\nДанные загружены. Нажмите любую клавишу для возврата в меню...");
                Console.ReadKey(true);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ОШИБКА] Неверный пункт меню.");
                Console.ResetColor();
                Console.WriteLine("\nНажмите любую клавишу для возврата...");
                Console.ReadKey(true);
            }
        }
    }

    private static void LoadFromFile(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("Файл " + path + " не найден!");
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split('|');
            if (parts.Length == 3)
            {
                int cpu = int.Parse(parts[0]);
                int ram = int.Parse(parts[1]);
                int temp = int.Parse(parts[2]);

                ShiftAndAdd(cpuHistory, cpu);
                ShiftAndAdd(ramHistory, ram);
                ShiftAndAdd(tempHistory, temp);
            }
        }

        DrawScreen(cpuHistory[19], ramHistory[19], tempHistory[19]);
    }

    private static void OnDataReceived(double cpu, double ram, double temp)
    {
        tickCounter++;

        if (tickCounter % scaleSeconds != 0) return;

        ShiftAndAdd(cpuHistory, (int)Math.Round(cpu));
        ShiftAndAdd(ramHistory, (int)Math.Round(ram));
        ShiftAndAdd(tempHistory, (int)Math.Round(temp));

        DrawScreen(cpu, ram, temp);
    }

    private static void ShiftAndAdd(int[] arr, int newValue)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }
        arr[arr.Length - 1] = newValue;
    }

    private static void DrawScreen(double cpu, double ram, double temp)
    {
        Console.Clear();

        Console.WriteLine("==========================================");
        Console.WriteLine("        МОНИТОРИНГ СЕРВЕРА");
        Console.WriteLine("==========================================");
        Console.WriteLine(string.Format("CPU:         {0,5} % | {1}", cpu, RenderGraph(cpuHistory)));
        Console.WriteLine(string.Format("RAM:         {0,5} % | {1}", ram, RenderGraph(ramHistory)));
        Console.WriteLine(string.Format("Temperature: {0,5} C | {1}", temp, RenderGraph(tempHistory)));
        Console.WriteLine("==========================================");
    }
}