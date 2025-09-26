using System.Windows;
using System.Windows.Forms;
using StarCitizenCompanion.Models;
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
        public LogTailerService TailerService { get; } = new LogTailerService();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new Data.Context())
            {
                db.Database.EnsureCreated();
            }

            _trayIcon = new NotifyIcon
            {
                Icon = System.Drawing.SystemIcons.Information,
                Visible = true,
                Text = "Star Citizen Companion"
            };

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Apri", null, (s, ev) => ShowMainWindow());
            contextMenu.Items.Add("Impostazioni", null, (s, ev) => ShowSettingsWindow());
            contextMenu.Items.Add("Esci", null, (s, ev) => ShutdownApp());
            _trayIcon.ContextMenuStrip = contextMenu;
            
            TailerService.Start();
                        
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Hide();
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

        private void ShowSettingsWindow()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.Activate();
                    var settingsWindow = new SettingsWindow()
                    {
                        Owner = mainWindow
                    };
                    settingsWindow.ShowDialog();
                }
            });
        }
    }
}
