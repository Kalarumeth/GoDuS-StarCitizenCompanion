using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace StarCitizenCompanion
{
    public partial class OverlayWindow : Window
    {
        private DispatcherTimer _timer;

        public OverlayWindow(string message)
        {
            InitializeComponent();
            MessageText.Text = message;

            Loaded += OverlayWindow_Loaded;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3) // Mostra per 3 secondi
            };
            _timer.Tick += (s, e) =>
            {
                _timer.Stop();
                Close();
            };
            _timer.Start();
        }
        private void OverlayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            // 🔹 Non attivabile + click-through
            exStyle |= WS_EX_NOACTIVATE | WS_EX_TRANSPARENT;

            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
        }

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;
        private const int WS_EX_TRANSPARENT = 0x00000020;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}
