using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Homeplanner.Pages
{
    public partial class Dashboard : Page
    {
        private readonly WeatherViewModel _viewModel;
        private string connectionString = "Data Source=../../../Pages/homeplanne1.db;Version=3;";

        public ObservableCollection<ToDoItem> ToDoList { get; set; } = new ObservableCollection<ToDoItem>();

        public Dashboard()
        {
            InitializeComponent();
            _viewModel = new WeatherViewModel();
            DataContext = _viewModel;
            ToDoListboxDashboard.ItemsSource = ToDoList;
            InitializeDatabase();
            LoadTodaysTasks();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string createTableQuery = @"CREATE TABLE IF NOT EXISTS todos (
                                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                task_text TEXT NOT NULL,
                                                task_date TEXT NOT NULL,
                                                task_status INTEGER DEFAULT 0)";
                    SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Initialisieren der Datenbank: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTodaysTasks()
        {
            ToDoList.Clear();
            DateTime today = DateTime.Today;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT task_text, task_status FROM todos WHERE task_date = @date";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@date", today.ToString("yyyy-MM-dd"));
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string taskText = reader.GetString(0);
                        bool taskStatus = reader.GetInt32(1) == 1; // SQLite speichert BOOL als INTEGER (0 oder 1)
                        ToDoList.Add(new ToDoItem { TaskText = taskText, IsChecked = taskStatus });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Aufgaben: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.DataContext is ToDoItem item)
            {
                item.IsChecked = true;
                UpdateTaskStatusInDatabase(item, true);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.DataContext is ToDoItem item)
            {
                item.IsChecked = false;
                UpdateTaskStatusInDatabase(item, false);
            }
        }

        private void UpdateTaskStatusInDatabase(ToDoItem item, bool isChecked)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE todos SET task_status = @status WHERE task_text = @taskText AND task_date = @date";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@status", isChecked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@taskText", item.TaskText);
                    cmd.Parameters.AddWithValue("@date", DateTime.Today.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aktualisieren des Status: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Falls du noch Logik für Selektionen brauchst, kannst du hier erweitern.
        }

        private async void GetWeather_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadWeatherAsync();
        }
    }

    public class ToDoItem
    {
        public string TaskText { get; set; }
        public bool IsChecked { get; set; }
    }
}
