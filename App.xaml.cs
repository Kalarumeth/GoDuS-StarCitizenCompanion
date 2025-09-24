using System.Windows;
using System.Windows.Forms;
using StarCitizenCompanion.Services;
using Application = System.Windows.Application;

namespace StarCitizenCompanion
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NotifyIcon _trayIcon;
        private LogWatcher _logWatcher;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new Data.Context())
            {
                db.Database.EnsureCreated();
            }

            // Inizializza tray icon
            _trayIcon = new NotifyIcon
            {
                Icon = System.Drawing.SystemIcons.Information,
                Visible = true,
                Text = "Star Citizen Companion"
            };

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Apri", null, (s, ev) => ShowMainWindow());
            contextMenu.Items.Add("Esci", null, (s, ev) => ShutdownApp());
            _trayIcon.ContextMenuStrip = contextMenu;

            // Avvia watcher sul log
            // Da spottare il path in appconfig
            //_logWatcher = new LogWatcher(@"C:\Program Files\Roberts Space Industries\StarCitizen\LIVE\Game.log");
            _logWatcher = new LogWatcher(@"C:\Users\claud\Desktop\test.txt");
            _logWatcher.OnNewEvent += LogWatcher_OnNewEvent;

            // Non mostrare subito la main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Hide();
        }

        private void LogWatcher_OnNewEvent(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NotificationService.ShowNotification(message);
            });
        }

        private void ShowMainWindow()
        {
            if (Current.MainWindow == null)
                Current.MainWindow = new MainWindow();

            Current.MainWindow.Show();
            Current.MainWindow.Activate();
        }

        private void ShutdownApp()
        {
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            Shutdown();
        }
    }
}
