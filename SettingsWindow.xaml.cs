using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace StarCitizenCompanion
{
    public partial class SettingsWindow : Window
    {
        public string LogPath { get; private set; }
        public int OverlayDuration { get; private set; }

        public SettingsWindow()
        {
            InitializeComponent();
            var cm = ConfigManager.Load();
            LogPathTextBox.Text = cm.LogFilePath;
            OverlayDurationTextBox.Text = cm.OverlayDuration.ToString();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Log file (*.log)|*.log",
                FileName = LogPathTextBox.Text
            };
            if (dlg.ShowDialog() == true)
            {
                LogPathTextBox.Text = dlg.FileName;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(LogPathTextBox.Text))
            {
                MessageBox.Show("Il file specificato non esiste.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LogPath = LogPathTextBox.Text;

            if(int.TryParse(OverlayDurationTextBox.Text, out int duration))
                OverlayDuration = duration;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
