namespace Veeam.Application
{
    internal class Logger
    {
        public static string GetLogPath { get; internal set; } = default!;

        public static void LogInfo(string message)
        {
            var logMessage = $"{DateTime.Now}: {message}";

            // Log to the console
            Console.WriteLine(logMessage);

            // Log to the file
            using StreamWriter sw = File.AppendText(GetLogPath);
            sw.WriteLine(logMessage);
        }
    }
}
