using StarCitizenCompanion.Helper;
using StarCitizenCompanion.Models;
using StarCitizenCompanion.Repository;
using StarCitizenCompanion.Services;
using System.IO;

public class LogTailerService
{
    private LogTailer _tailer;
    public event Action<string> OnNewLine;

    public void Start()
    {
        _tailer?.Stop();
        _tailer = new LogTailer();
        _tailer.OnNewLine += line => NotificationService.ShowNotification(line);
        _tailer.Start();
    }

    public void Stop() => _tailer?.Stop();
}

public class LogTailer : IDisposable
{
    private readonly string _filePath;
    private CancellationTokenSource _cts;
    private Task _task;

    public event Action<string> OnNewLine;

    public LogTailer()
    {
        if (!File.Exists(ConfigManager.Load().LogFilePath))
            ErrorHelper.ShowError("File di log non trovato");

        _filePath = ConfigManager.Load().LogFilePath;
    }

    public void Start()
    {
        _cts = new CancellationTokenSource();
        _task = Task.Run(() => Run(_cts.Token));
    }

    public void Stop()
    {
        _cts?.Cancel();
    }

    private async Task Run(CancellationToken token)
    {
        using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fs);

        fs.Seek(0, SeekOrigin.End);

        while (!token.IsCancellationRequested)
        {
            string line = await reader.ReadLineAsync();
            if (line != null)
            {
                NotificationEvent _ne = NotifyFormatter.Notify(line); 

                if (_ne.MessageComposer != null)
                {

                    OnNewLine?.Invoke(_ne.MessageNotify);
                }
            }
            else
            {
                await Task.Delay(10, token);
            }
        }
    }

    public void Dispose()
    {
        Stop();
    }
}