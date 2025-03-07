using System;
using System.Data.SQLite;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;

namespace Homeplanner.Pages
{
    public partial class ToDo : Page
    {
        private string connectionString = "Data Source=../../../Pages/homeplanne1.db;Version=3;";

        public ToDo()
        {
            InitializeComponent();
            InitializeDatabase(); // Erstellt die Datenbank, falls nicht vorhanden
            LoadToDos(); // Lade bestehende Aufgaben
        }

        private void InitializeDatabase()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string createTableQuery = "CREATE TABLE IF NOT EXISTS todos (id INTEGER PRIMARY KEY AUTOINCREMENT, task_text TEXT NOT NULL, task_date TEXT NOT NULL, notify INTEGER DEFAULT 0);";
                    SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Initialisieren der Datenbank: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string todoText = InputTextBox.Text.Trim();
            DateTime? selectedDate = DatumFeld.SelectedDate;
            bool notify = NotificationCheckBox.IsChecked == true;

            if (string.IsNullOrEmpty(todoText))
            {
                MessageBox.Show("Bitte Text eingeben!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedDate == null)
            {
                MessageBox.Show("Bitte ein Datum auswählen!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO todos (task_text, task_date, notify) VALUES (@text, @date, @notify)";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@text", todoText);
                    cmd.Parameters.AddWithValue("@date", selectedDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@notify", notify ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }

                string item = $"{selectedDate.Value.ToShortDateString()} - {todoText}";
                ToDoListbox.Items.Add(item);
                InputTextBox.Clear();
                DatumFeld.SelectedDate = null;

                MessageBox.Show("Aufgabe erfolgreich gespeichert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadToDos()
        {
            ToDoListbox.Items.Clear();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT task_text, task_date, notify FROM todos";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string taskText = reader.GetString(0);
                        string taskDate = reader.GetString(1);
                        bool notify = reader.GetInt32(2) == 1;

                        ToDoListbox.Items.Add($"{taskDate} - {taskText}");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der To-Dos: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ToDoListbox.SelectedItem != null)
            {
                string selectedItem = ToDoListbox.SelectedItem.ToString();
                string[] parts = selectedItem.Split(new[] { " - " }, StringSplitOptions.None);

                if (parts.Length == 2)
                {
                    string taskDate = parts[0];
                    string taskText = parts[1];

                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                        {
                            conn.Open();
                            string query = "DELETE FROM todos WHERE task_text = @text AND task_date = @date";
                            SQLiteCommand cmd = new SQLiteCommand(query, conn);
                            cmd.Parameters.AddWithValue("@text", taskText);
                            cmd.Parameters.AddWithValue("@date", taskDate);
                            cmd.ExecuteNonQuery();
                        }

                        ToDoListbox.Items.Remove(selectedItem);
                        MessageBox.Show("Aufgabe erfolgreich gelöscht!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fehler beim Löschen: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Bitte ein Element auswählen!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}