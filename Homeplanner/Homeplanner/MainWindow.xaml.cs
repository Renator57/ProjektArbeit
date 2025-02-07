using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Homeplanner.Properties;

namespace Homeplanner
{
    public partial class MainWindow : Window
    {
        private bool isMenuOpen = false;

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings(); // Lade gespeicherte Einstellungen sofort beim Start
            MainFrame.Navigate(new Uri("Pages/Dashboard.xaml", UriKind.Relative)); // Startseite
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Login-Fenster öffnen
            Login loginWindow = new Login();
            loginWindow.Show();

            // Hauptfenster schließen
            this.Close();
        }
        // Lade gespeicherte Einstellungen
        private void LoadSettings()
        {
            // Dark Mode Zustand wiederherstellen
            if (Properties.Settings.Default.IsDarkMode)
            {
                ApplyDarkMode();
            }
            else
            {
                ApplyLightMode();
            }

            // Schriftgröße anwenden
            ApplyFontSize(Properties.Settings.Default.FontSize);
        }

        // Dark Mode anwenden
        private void ApplyDarkMode()
        {
            Application.Current.Resources["BackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2C2C2C"));
            Application.Current.Resources["ForegroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCDCDC"));
            Application.Current.Resources["MenuBackground"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A3A3A"));
            Application.Current.Resources["PrimaryColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#16A085"));
        }

        // Light Mode anwenden
        private void ApplyLightMode()
        {
            Application.Current.Resources["BackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDC4E5"));
            Application.Current.Resources["ForegroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
            Application.Current.Resources["MenuBackground"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            Application.Current.Resources["PrimaryColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1ABC9C"));
        }

        // Schriftgröße anwenden
        // Schriftgröße anwenden mit Default-Wert
        private void ApplyFontSize(int? fontSize)
        {
            Application.Current.Resources["FontSize"] = fontSize ?? 14; // Falls null, setze Standardwert 14
        }

        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            isMenuOpen = !isMenuOpen;

            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                To = isMenuOpen ? 1 : 0,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            MenuPanel.BeginAnimation(OpacityProperty, fadeAnimation);
            MenuPanel.Visibility = isMenuOpen ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Navigate(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is string page && !string.IsNullOrWhiteSpace(page))
            {
                MainFrame.Navigate(new Uri(page, UriKind.Relative));
            }
        }


    }
}
