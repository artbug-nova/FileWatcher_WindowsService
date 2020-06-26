using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WindowsServiceWorker.Service
{
    public interface IWorkerService
    {
        void Start();
        void Stop();
        void RecordEntry(string fileEvent, string filePath);
        void Watcher_Deleted(object sender, FileSystemEventArgs e);
        void Watcher_Created(object sender, FileSystemEventArgs e);
        void Watcher_Changed(object sender, FileSystemEventArgs e);
        void Watcher_Renamed(object sender, RenamedEventArgs e);

    }
}
