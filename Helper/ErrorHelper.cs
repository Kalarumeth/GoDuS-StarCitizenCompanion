using System.Windows;

namespace StarCitizenCompanion.Helper
{
    public static class ErrorHelper
    {
        public static void ShowError(string message, string title = "Errore")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }
    }
}
