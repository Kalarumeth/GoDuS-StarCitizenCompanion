using StarCitizenCompanion.Models;
using StarCitizenCompanion.Repository;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

public class LogTailer : IDisposable
{
    private readonly string _filePath;
    private CancellationTokenSource _cts;
    private Task _task;

    public event Action<string> OnNewLine;

    public LogTailer(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File di log non trovato", filePath);

        _filePath = filePath;
    }

    public void Start()
    {
        _cts = new CancellationTokenSource();
        _task = Task.Run(() => Run(_cts.Token));
    }

    public void Stop()
    {
        _cts?.Cancel();
        _task?.Wait();
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