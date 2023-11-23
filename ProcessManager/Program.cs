using System.Diagnostics;

namespace ProcessManager
{
    public class ProcessManager
    {
        internal static readonly List<Process> Processes = Process.GetProcesses().ToList();
        public static void ShowProcesses()
        {
            int i = 0;
            Console.WriteLine("Список процессов:");
            foreach (Process process in Processes)
            {
                try
                {
                    process.Refresh();
                    Console.WriteLine($"{i}: {process.ProcessName} - Занятость CPU: {process.TotalProcessorTime}, Занятость памяти: {process.WorkingSet64} байт");
                    i++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось получить информацию о процессе {process.ProcessName}: {ex.Message}");
                }
            }
        }

        public static void TerminateProcess(int selectedIndex)
        {
            if (selectedIndex >= 0 && selectedIndex < Processes.Count)
            {
                Process selectedProcess = Processes[selectedIndex];
                try
                {
                    selectedProcess.Kill();
                    Console.WriteLine($"Процесс {selectedProcess.ProcessName} был завершен");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось завершить процесс {selectedProcess.ProcessName}: {ex.Message}");
                }
            }
        }

        public static void TerminateProcessesByName(string? processName)
        {
            try
            {
                Process[] matchingProcesses = Process.GetProcessesByName(processName);
                foreach (Process process in matchingProcesses)
                {
                    try
                    {
                        process.Kill();
                        Console.WriteLine($"Процесс {process.ProcessName} был завершен");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Не удалось завершить процесс {process.ProcessName}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось завершить процессы с именем {processName}: {ex.Message}");
            }
        }
    }

    public enum SpecialKeys
    {
        D = -1,
        Delete = -2,
        Backspace = -3
    }

    class Program
    {
        static void Main()
        {
            while (true)

            {
                Console.Clear();
                ProcessManager.ShowProcesses();

                Console.WriteLine("\nМеню:");
                Console.WriteLine("D) Завершить выбранный процесс");
                Console.WriteLine("Delete) Завершить все процессы с определенным именем");
                Console.WriteLine("Backspace) Вернуться к списку процессов");

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                SpecialKeys specialKey = GetSpecialKey(keyInfo.Key);

                switch (specialKey)
                {
                    case SpecialKeys.D:
                        int selectedIndex = GetSelectedIndex();
                        ProcessManager.TerminateProcess(selectedIndex);
                        break;
                    case SpecialKeys.Delete:
                        string? processName = GetProcessName();
                        ProcessManager.TerminateProcessesByName(processName);
                        break;
                    case SpecialKeys.Backspace:
                        continue;
                    default:
                        Console.WriteLine("Неверный ввод");
                        break;
                }
                Console.WriteLine("\nНажмите любую клавишу для продолжения");
                Console.ReadKey();
            }
        }

        private static SpecialKeys GetSpecialKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D:
                    return SpecialKeys.D;
                case ConsoleKey.Delete:
                    return SpecialKeys.Delete;
                case ConsoleKey.Backspace:
                    return SpecialKeys.Backspace;
                default:
                    return (SpecialKeys)Enum.Parse(typeof(SpecialKeys), key.ToString());
            }
        }

        static int GetSelectedIndex()
        {
            Console.WriteLine("\nВведите номер выбранного процесса:");
            int selectedIndex;
            while (!int.TryParse(Console.ReadLine(), out selectedIndex))
            {
                Console.WriteLine("Неверный ввод. Введите число:");
            }
            return selectedIndex - 1;
        }

        static string? GetProcessName()
        {
            Console.WriteLine("\nВведите название процесса:");
            return Console.ReadLine();
        }
    }
}
