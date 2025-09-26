using StarCitizenCompanion.Data;
using StarCitizenCompanion.Models;
using System.Windows;

namespace StarCitizenCompanion.Services
{
    public static class NotificationService
    {        
        private static readonly Queue<string> _queue = new();
        private static bool _isShowing = false;

        public static void ShowNotification(string message)
        {
            _queue.Enqueue(message);
            if (!_isShowing)
                ShowNext();
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

            Application.Current.Dispatcher.Invoke(() =>
            {
                var overlay = new OverlayWindow(nextMessage);
                overlay.Left = SystemParameters.WorkArea.Width - overlay.Width - 20;
                overlay.Top = SystemParameters.WorkArea.Height - overlay.Height - 20;
                overlay.Closed += (s, e) => ShowNext();
                overlay.Show();
            });
        }
    }
}
