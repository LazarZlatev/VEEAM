namespace Veeam.Application
{
    internal class Logger
    {
        public static void LogInfo(string message, string logFilePath)
        {
            string logMessage = $"{DateTime.Now}: {message}";

            // Log to the console
            Console.WriteLine(logMessage);

            // Log to the file
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(logMessage);
            }
        }
    }
}
