using System;
using System.IO;
using System.Windows;

namespace AstroPhotometry.ViewModels
{
    public class FileWatcherVM
    {
        FileSystemWatcher watcher;
        public FileWatcherVM(string relative_path, string filter)
        {
            Directory.CreateDirectory(relative_path);

            watcher = new FileSystemWatcher(Path.GetFullPath(relative_path));

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;
            

            watcher.Filter = filter; // The file to watch 
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

        }
        
        // IOC for updating if file created or change
        Action<string> IOCupdateUri = null;

        public void Ioc(Action<string> updateUri)
        {
            this.IOCupdateUri = updateUri;
            
        }

        // TODO: maybe use queue
        private string last_change;
        private string last_create;

        public string lastChange
        {
            get { return last_change; }
        }
        public string lastCreate
        {
            get { return last_create; }
        }
        public FileSystemWatcher getWatcher()
        {
            return watcher;
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            if (IOCupdateUri != null)
            {
                IOCupdateUri(e.FullPath);
            }

            this.last_change = e.FullPath;

            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (IOCupdateUri != null)
            {
                IOCupdateUri(e.FullPath);
            }

            this.last_create = e.FullPath;

            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
        }


        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted: {e.FullPath}");
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

    }
}
