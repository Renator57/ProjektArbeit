using System;
using System.Windows;
using System.Windows.Media;
using Homeplanner.Properties;

namespace Homeplanner
{
    public partial class App : Application
    {
        // Beim Starten der Anwendung die Einstellungen laden
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Einstellungen für DarkMode und Schriftgröße aus der Benutzereinstellung laden
            bool isDarkMode = Settings.Default.IsDarkMode;
            int fontSize = Settings.Default.FontSize;

            // Dark Mode anwenden
            if (isDarkMode)
            {
                ApplyDarkMode();
            }
            else
            {
                ApplyLightMode();
            }

            // Schriftgröße anwenden
            ApplyFontSize(fontSize);
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
            Application.Current.Resources["BackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5"));
            Application.Current.Resources["ForegroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
            Application.Current.Resources["MenuBackground"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            Application.Current.Resources["PrimaryColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1ABC9C"));
        }

        // Schriftgröße anwenden
        private void ApplyFontSize(int fontSize)
        {
            Application.Current.Resources["FontSize"] = fontSize;
        }
    }
}
