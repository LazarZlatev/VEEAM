namespace Veeam.Application
{
    internal class Synchronize
    {
        public static void StartSynchronization(string sourceFolder, string replicaFolder, string logfilepath)
        {
            try
            {
                // Create the log file
                if (!File.Exists(logfilepath))
                {
                    using (StreamWriter sw = File.CreateText(logfilepath))
                    {
                        sw.WriteLine($"Log File Created: {DateTime.Now}");
                    }
                }

                // Synchronize folders
                SynchronizeCopying(sourceFolder, replicaFolder, logfilepath);
                SynchronizeDeleting(sourceFolder, replicaFolder,logfilepath);

             
                // Logging
                Logger.LogInfo($"Synchronization Completed: {DateTime.Now}", logfilepath);
            }
            catch (Exception ex)
            {
                Logger.LogInfo($"Error during synchronization: {ex.Message}", logfilepath);
            }
        }

        static private void SynchronizeCopying(string sourceFolder, string destFolder, string logfilepath)
        {
            if (!Directory.Exists(destFolder) || Directory.GetLastWriteTimeUtc(sourceFolder) > File.GetLastWriteTimeUtc(destFolder))
            {
                Directory.CreateDirectory(destFolder);
                Thread.Sleep(1000);
                Logger.LogInfo($"Directory Created: {destFolder}", logfilepath);
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
                    Logger.LogInfo($"File Copied: {dest}", logfilepath);
                }
            }

            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                SynchronizeCopying(folder, dest, logfilepath);
            }
        }

        static private void SynchronizeDeleting(string sourceFolder, string destFolder, string logfilepath)
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
                    Logger.LogInfo($"File Deleted: {dest}", logfilepath);
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
                    Logger.LogInfo($"Directory Deleted:{dest}", logfilepath);
                }
                else
                {
                    SynchronizeDeleting(source, dest, logfilepath);
                }
            }
        }
    }
}
