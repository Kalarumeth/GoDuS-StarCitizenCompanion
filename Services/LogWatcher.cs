using System.IO;
using StarCitizenCompanion.Data;
using StarCitizenCompanion.Models;
using StarCitizenCompanion.Repository;

namespace StarCitizenCompanion.Services
{
    public class LogWatcher
    {
        private readonly string _filePath;
        private FileSystemWatcher _watcher;
        private long _lastPosition;

        public event Action<string> OnNewEvent;

        public LogWatcher(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File di log non trovato", filePath);

            _watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath))
            {
                Filter = Path.GetFileName(filePath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };

            _watcher.Changed += Watcher_Changed;
            _watcher.EnableRaisingEvents = true;

            _lastPosition = new FileInfo(filePath).Length;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs.Seek(_lastPosition, SeekOrigin.Begin);
                using var reader = new StreamReader(fs);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(NoticeTag.ActorDeath))
                        OnNewEvent?.Invoke(line);
                }

                _lastPosition = fs.Position;
            }
            catch { /* Ignora errori temporanei di lock */ }
        }
    }
}
