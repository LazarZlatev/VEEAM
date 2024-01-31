namespace Veeam.Application
{
    internal class Synchronize
    {
        private static readonly string _getlogpath = Logger.GetLogPath;
        public static void StartSynchronization(string sourceFolder, string replicaFolder)
        {
            try
            {
                // Create the log file
                if (!File.Exists(_getlogpath))
                {
                    using StreamWriter sw = File.CreateText(_getlogpath);
                    sw.WriteLine($"Log File Created: {DateTime.Now}");
                }

                // Synchronize folders
                SynchronizeCopying(sourceFolder, replicaFolder);
                SynchronizeDeleting(sourceFolder, replicaFolder);

             
                // Logging
                Logger.LogInfo($"Synchronization Completed: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Logger.LogInfo($"Error during synchronization: {ex.Message}");
            }
        }

        static private void SynchronizeCopying(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder) || Directory.GetLastWriteTimeUtc(sourceFolder) > File.GetLastWriteTimeUtc(destFolder))
            {
                Directory.CreateDirectory(destFolder);
                Thread.Sleep(500);
                Logger.LogInfo($"Directory Created: {destFolder}");
            }

            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                if (!File.Exists(dest) || File.GetLastWriteTimeUtc(Path.Combine(sourceFolder, name)) > File.GetLastWriteTimeUtc(dest))
                {
                    File.Copy(file, dest, true);
                    File.SetAttributes(dest, FileAttributes.Normal);
                    Logger.LogInfo($"File Copied: {dest}");
                }
            }

            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                SynchronizeCopying(folder, dest);
            }
        }

        static private void SynchronizeDeleting(string sourceFolder, string destFolder)
        {
            string[] files = Directory.GetFiles(destFolder);

            foreach (string file in files)
            {
                var name = Path.GetFileName(file);
                var dest = Path.Combine(destFolder, name);
                var source = Path.Combine(sourceFolder, name);
                if (!File.Exists(source))
                {
                    File.SetAttributes(dest, FileAttributes.Normal);
                    File.Delete(dest);
                    Logger.LogInfo($"File Deleted: {dest}");
                }
            }

            string[] foldersPath = Directory.GetDirectories(destFolder);
            foreach (string subfolder in foldersPath)
            {
                string subfoldername = Path.GetFileName(subfolder);
                string dest = Path.Combine(destFolder, subfoldername);
                string source = Path.Combine(sourceFolder, subfoldername);
                if (!Directory.Exists(source))
                {
                    Directory.Delete(dest,true);
                    Logger.LogInfo($"Directory Deleted:{dest}");
                }
                else
                {
                    SynchronizeDeleting(source, dest);
                }
            }
        }
    }
}
