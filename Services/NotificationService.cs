using StarCitizenCompanion.Data;
using StarCitizenCompanion.Models;
using System.Windows;

namespace StarCitizenCompanion.Services
{
    public static class NotificationService
    {
        public static event Action<NotificationEvent> OnNotificationSaved;
        private static readonly Queue<string> _queue = new();
        private static bool _isShowing = false;

        public static void ShowNotification(string message)
        {
            SaveAndNotify(message);

            // Notification Not Work
            //_queue.Enqueue(message);
            //if (!_isShowing)
            //    ShowNext();
        }

        private static void ShowNext()
        {
            if (_queue.Count == 0)
            {
                _isShowing = false;
                return;
            }

            _isShowing = true;
            string nextMessage = _queue.Dequeue();

            var overlay = new OverlayWindow(nextMessage);
            overlay.Left = SystemParameters.WorkArea.Width - overlay.Width - 20;
            overlay.Top = SystemParameters.WorkArea.Height - overlay.Height - 20;

            // Quando si chiude, mostra il prossimo
            overlay.Closed += (s, e) => ShowNext();

            overlay.Show();
        }


        private static void SaveAndNotify(string message)
        {
            var notif = new NotificationEvent
            {
                Message = message,
                Date = DateTime.Now
            };

            using (var db = new Context())
            {
                db.Notifications.Add(notif);
                db.SaveChanges();
            }

            OnNotificationSaved?.Invoke(notif);
        }
    }
}
