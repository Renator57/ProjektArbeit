using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Homeplanner.Properties;
using System.Data.SQLite;
using System.Net;
using System.Net.Mail;

namespace Homeplanner
{
    public partial class MainWindow : Window
    {
        private bool isMenuOpen = false;
        private string connectionString = "Data Source=../../../Pages/homeplanne1.db;Version=3;";
        private string userEmail = "r38447282@gmail.com"; // Deine Absender-Mail
        private string appPassword = "shka sdgr odoq yehn"; // Google App-Passwort

        public MainWindow()
        {
            InitializeComponent();

            LoadSettings();

            StartDailyEmailTask(); // Starte den geplanten E-Mail-Versand um 18:00 Uhr
            MainFrame.Navigate(new Uri("Pages/Dashboard.xaml", UriKind.Relative));
        }


        private async void StartDailyEmailTask()
        {
            while (true)
            {
                DateTime now = DateTime.Now;
                DateTime nextSendTime = DateTime.Today.AddHours(8); // 8:00 Uhr heute

                // Wenn es bereits nach 18:00 Uhr ist, plane es für morgen
                if (now > nextSendTime)
                {
                    nextSendTime = nextSendTime.AddDays(1);
                }

                // Berechne die Wartezeit bis zur nächsten 18:00 Uhr
                TimeSpan waitTime = nextSendTime - now;

                // Warte bis zum geplanten Zeitpunkt
                await Task.Delay(waitTime);

                // Sende die E-Mail
                SendEmailImmediately();
            }
        }

        private void SendEmailImmediately()
        {
            List<string> todosForToday = new List<string>();

            try
            {
                // Verbinde mit der SQLite-Datenbank und hole Aufgaben, die für heute fällig sind
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT task_text FROM todos WHERE notify = 1 AND task_date = @today";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@today", DateTime.Now.ToString("yyyy-MM-dd"));
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        todosForToday.Add(reader.GetString(0));
                    }
                }

                // Wenn Aufgaben für heute vorhanden sind, sende die E-Mail
                if (todosForToday.Count > 0)
                {
                    SendEmail(todosForToday);
                }
                else
                {
                    MessageBox.Show("Keine fälligen Aufgaben für heute.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Senden der E-Mails: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendEmail(List<string> todoList)
        {
            try
            {
                // Erstelle den Inhalt der E-Mail
                string emailBody = "Hier sind Ihre fälligen Aufgaben für heute:\n\n";
                foreach (string task in todoList)
                {
                    emailBody += $"- {task}\n";
                }

                // Erstelle die E-Mail-Nachricht
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(userEmail);
                mail.To.Add("furkans_erkan57@outlook.de"); // Hier könnte eine andere Empfängeradresse stehen
                mail.Subject = "To-Do Erinnerung - Heute fällige Aufgaben";
                mail.Body = emailBody;

                // SMTP-Client für Gmail
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(userEmail, appPassword);
                smtp.EnableSsl = true;

                // E-Mail senden
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim E-Mail-Versand: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            Application.Current.Resources["BackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
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
