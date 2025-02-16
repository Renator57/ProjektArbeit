using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Homeplanner.Properties;  // Sicherstellen, dass der richtige Namensraum importiert wird

namespace Homeplanner.Pages
{
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            LoadSettings(); // Lade gespeicherte Einstellungen
        }

        // Lade gespeicherte Einstellungen
        private void LoadSettings()
        {
            // Dark Mode Zustand wiederherstellen
            DarkModeCheckBox.IsChecked = Properties.Settings.Default.IsDarkMode;

            // Schriftgröße wiederherstellen
            foreach (ComboBoxItem item in FontSizeCombo.Items)
            {
                if (item.Tag.ToString() == Properties.Settings.Default.FontSize.ToString())
                {
                    item.IsSelected = true;
                    break;
                }
            }

            // Dark Mode anwenden
            if (DarkModeCheckBox.IsChecked == true)
            {
                ApplyDarkMode();
            }
            else
            {
                ApplyLightMode();
            }
        }

        // 🔄 Dark Mode EIN
        private void DarkModeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsDarkMode = true;  // Ändern der Benutzereinstellung
            Properties.Settings.Default.Save(); // Speichern der Einstellung
            ApplyDarkMode();
        }

        // 🔄 Dark Mode AUS (Light Mode)
        private void DarkModeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsDarkMode = false; // Ändern der Benutzereinstellung
            Properties.Settings.Default.Save(); // Speichern der Einstellung
            ApplyLightMode();
        }

        // 🎨 Schriftgröße ändern
        private void FontSizeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeCombo.SelectedItem is ComboBoxItem selectedItem)
            {
                if (int.TryParse(selectedItem.Tag.ToString(), out int fontSize))
                {
                    Properties.Settings.Default.FontSize = fontSize;  // Ändern der Benutzereinstellung
                    Properties.Settings.Default.Save(); // Speichern der Einstellung
                    ApplyFontSize(fontSize);
                }
            }
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
        private void ApplyFontSize(int fontSize)
        {
            Application.Current.Resources["FontSize"] = fontSize;
        }
    }
}
