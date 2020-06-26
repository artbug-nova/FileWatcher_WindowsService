using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace WindowsServiceWorker.Service
{

    public class WorkerService : IWorkerService
    {
        private ILogger<WorkerService> _logger;
        private FileSystemWatcher watcher;
        private readonly object obj = new object();

        public WorkerService(ILogger<WorkerService> logger)
        {
            watcher = new FileSystemWatcher($"{AppContext.BaseDirectory}");
            watcher.Deleted += Watcher_Deleted;
            watcher.Renamed += Watcher_Renamed;
            watcher.Changed += Watcher_Changed;
            watcher.Created += Watcher_Created;
            _logger = logger;
        }

        public void RecordEntry(string fileEvent, string filePath)
        {
            lock (obj)
            {
                _logger.LogInformation(String.Format("File {0} was {1}", filePath, fileEvent));
            }
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        public void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "changed";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        public void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "create";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        public void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "delete";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        public void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "rename " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }
    }
}
