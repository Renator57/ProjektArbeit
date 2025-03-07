using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Homeplanner.Pages
{
    public partial class Kalender : Page
    {
        private DateTime _currentDate = DateTime.Today;
        private const string DatabasePath = "homeplanner1.db";  // SQLite Datenbank Pfad

        public Kalender()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadCalendar();
        }

        // Initialisiert die Datenbank und erstellt die Tabelle für Termine
        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Termine (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Datum TEXT,
                        Bezeichnung TEXT
                    )";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        // Kalender mit den richtigen Tagen und Monaten laden
        private void LoadCalendar()
        {
            DaysContainer.Children.Clear();
            CurrentMonthText.Text = _currentDate.ToString("MMMM yyyy");

            DateTime firstDay = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int startOffset = (int)firstDay.DayOfWeek == 0 ? 6 : (int)firstDay.DayOfWeek - 1;
            int daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);

            for (int i = 0; i < startOffset; i++)
            {
                DaysContainer.Children.Add(new TextBlock());
            }

            for (int day = 1; day <= daysInMonth; day++)
            {
                Button dayButton = new Button
                {
                    Content = day.ToString(),
                    Tag = new DateTime(_currentDate.Year, _currentDate.Month, day),
                    Margin = new Thickness(5),
                    Background = Brushes.White,
                };

                // Markiere den Tag, wenn es Termine gibt
                if (HasEvents(new DateTime(_currentDate.Year, _currentDate.Month, day)))
                {
                    dayButton.Background = Brushes.LightGreen;  // Beispiel: Hintergrundfarbe ändern
                }

                dayButton.Click += DayButton_Click;
                DaysContainer.Children.Add(dayButton);
            }
        }

        // Überprüft, ob es Termine an einem bestimmten Tag gibt
        private bool HasEvents(DateTime selectedDate)
        {
            bool hasEvents = false;

            using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(*) FROM Termine WHERE Datum = @Datum";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Datum", selectedDate.ToString("yyyy-MM-dd"));
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    hasEvents = count > 0;
                }
                connection.Close();
            }

            return hasEvents;
        }

        // Klick auf "◀" für den vorherigen Monat
        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddMonths(-1);
            LoadCalendar();
        }

        // Klick auf "▶" für den nächsten Monat
        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            _currentDate = _currentDate.AddMonths(1);
            LoadCalendar();
        }

        // Klick auf einen Tag, um ihn auszuwählen und Ereignisse anzuzeigen
        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is DateTime selectedDate)
            {
                SelectedDateText.Text = selectedDate.ToString("dd.MM.yyyy");
                EventInput.Text = "";
                EventsList.Items.Clear();

                // Lade die gespeicherten Termine aus der Datenbank
                LoadEvents(selectedDate);

                EventPanel.Visibility = Visibility.Visible;
                EventPanel.Tag = selectedDate;
            }
        }

        // Lade gespeicherte Termine aus der Datenbank für das ausgewählte Datum
        private void LoadEvents(DateTime selectedDate)
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
            {
                connection.Open();
                string selectQuery = "SELECT Bezeichnung FROM Termine WHERE Datum = @Datum";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Datum", selectedDate.ToString("yyyy-MM-dd"));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string eventText = reader["Bezeichnung"].ToString();
                            EventsList.Items.Add(eventText);
                        }
                    }
                }
                connection.Close();
            }
        }

        // Ereignis speichern
        private void SaveEvent_Click(object sender, RoutedEventArgs e)
        {
            if (EventPanel.Tag is DateTime selectedDate && !string.IsNullOrWhiteSpace(EventInput.Text))
            {
                // Speichere das Ereignis in der Datenbank
                using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
                {
                    connection.Open();
                    string insertQuery = @"
                        INSERT INTO Termine (Datum, Bezeichnung)
                        VALUES (@Datum, @Bezeichnung)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Datum", selectedDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Bezeichnung", EventInput.Text);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }

                // Die Anzeige der gespeicherten Termine aktualisieren
                EventsList.Items.Add(EventInput.Text);
                EventInput.Text = "";  // TextBox leeren

                // Kalender neu laden, um die visuelle Markierung hinzuzufügen
                LoadCalendar();
            }
        }

        // Schließen des Ereignis-Panels
        private void CloseEventPanel_Click(object sender, RoutedEventArgs e)
        {
            EventPanel.Visibility = Visibility.Hidden;
        }
    }
}
