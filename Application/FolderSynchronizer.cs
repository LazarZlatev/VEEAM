using Veeam.Application;

class FolderSynchronizer
{
    static int syncInterval;
    static string? logFilePath;
    static string? sourceFolder;
    static string? destFolder;

    static void Main(string[] args)
    {
        bool valid;
        // Source Folder One-way synchronization. 
        // You can add/delete files to source or destination folders, while program is running.

        Console.WriteLine("Enter full Source Folder path");
        sourceFolder = Console.ReadLine();
        Console.WriteLine("Enter full Destination Folder path");
        destFolder = Console.ReadLine();
        do
        {
            Console.WriteLine("Enter Interval In Seconds <int>"); // e.g : 1 
            string? i = Console.ReadLine();
            valid = int.TryParse(i, out syncInterval);
        } while (!valid);

        Console.WriteLine("Enter Log File Path");
        logFilePath = Console.ReadLine();

        Logger.GetLogPath = logFilePath;

        Logger.LogInfo($"Source Folder: {sourceFolder}, " +
                         $"Replication Folder: {destFolder}," +
                         $" Synchronization Interval: {syncInterval} seconds," +
                         $" Log File: {logFilePath},");
      

        // Start the synchronization loop
        while (true)
        {
            Synchronize.StartSynchronization(sourceFolder, destFolder);
            Thread.Sleep(syncInterval*1000); 
        }
    }
    
}
