using Microsoft.EntityFrameworkCore;
using StarCitizenCompanion.Models;
using StarCitizenCompanion.Repository;
using StarCitizenCompanion.Services;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StarCitizenCompanion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<NotificationEvent> _notifications;

        public MainWindow()
        {
            InitializeComponent();
            LoadData();

            NotifyFormatter.OnNotificationSaved += OnNotificationSaved;
        }

        private void LoadData()
        {
            using (var db = new Data.Context())
            {
                _notifications = new ObservableCollection<NotificationEvent>(
                    db.Notifications.OrderByDescending(f => f.Log.Date)
                    .Include(f => f.Log)
                    .Include(f => f.Message)
                    .ToList()

                );

                FeedGrid.ItemsSource = _notifications;
            }
        }

        private void OnNotificationSaved(NotificationEvent notif)
        {
            // Dispatcher → serve perché l’evento può arrivare da thread non UI
            Dispatcher.Invoke(() =>
            {
                _notifications.Insert(0, notif); // aggiungo in cima
            });
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            if (settingsWindow.ShowDialog() == true)
            {
                ConfigManager.Save(new AppConfig()
                {
                    LogFilePath = settingsWindow.LogPath,
                    OverlayDuration = settingsWindow.OverlayDuration,
                });

                var app = (App)Application.Current;
                app.TailerService.Start();
            }
        }
    }
}