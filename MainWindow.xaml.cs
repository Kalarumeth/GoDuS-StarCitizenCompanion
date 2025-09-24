using StarCitizenCompanion.Models;
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

            NotificationService.OnNotificationSaved += OnNotificationSaved;
        }

        private void LoadData()
        {
            using (var db = new Data.Context())
            {
                _notifications = new ObservableCollection<NotificationEvent>(
                    db.Notifications.OrderByDescending(f => f.Date).ToList()
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
    }
}