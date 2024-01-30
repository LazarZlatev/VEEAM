using Veeam.Application;

class FolderSynchronizer
{
    static void Main(string[] args)
    {
        // Source Folder One-way synchronization. You can add/delete files to any folder while programm is running.

        Console.WriteLine("Enter full Source Folder path");
        var sourceFolder = Console.ReadLine();
        Console.WriteLine("Enter full Destination Folder path");
        var destFolder = Console.ReadLine();
        Console.WriteLine("Enter Interval In Seconds");
        var syncInterval = int.Parse(Console.ReadLine()); // e.g : 1 
        Console.WriteLine("Enter Log File Path");
        var logFilePath = Console.ReadLine();

        Logger.LogInfo($"Source Folder: {sourceFolder}, " +
                         $"Replication Folder: {destFolder}," +
                         $" Synchronization Interval: {syncInterval} seconds," +
                         $" Log File: {logFilePath},", logFilePath);
      

        // Start the synchronization loop
        while (true)
        {
            Synchronize.StartSynchronization(sourceFolder, destFolder, logFilePath);
            Thread.Sleep(syncInterval * 1000); 
        }
    }
    
}
