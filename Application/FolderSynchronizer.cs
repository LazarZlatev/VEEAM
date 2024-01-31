using Veeam.Application;

class FolderSynchronizer
{
    static void Main(string[] args)
    {
        // Source Folder One-way synchronization. 
        // You can add/delete files to source or destination folders, while program is running.

        Console.WriteLine("Enter full Source Folder path");
        var sourceFolder = Console.ReadLine();
        Console.WriteLine("Enter full Destination Folder path");
        var destFolder = Console.ReadLine();
        Console.WriteLine("Enter Interval In Seconds");
        var syncInterval = int.Parse(Console.ReadLine()); // e.g : 1 
        Console.WriteLine("Enter Log File Path");
        var logFilePath = Console.ReadLine();

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
