using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Homeplanner.Pages
{
    public partial class Dashboard : Page
    {

        private readonly WeatherViewModel _viewModel;
        public Dashboard()
        {
            InitializeComponent();
            _viewModel = new WeatherViewModel();
            DataContext = _viewModel;
           



        }

        private async void GetWeather_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadWeatherAsync();
        }

        // Handle the Checked event for the CheckBoxes
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox != null)
            {
                MessageBox.Show($"{checkbox.Content} wurde abgehakt!");
                // Hier kannst du zusätzliche Logik hinzufügen, um den Status zu speichern
            }
        }

        // Handle the Unchecked event for the CheckBoxes
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox != null)
            {
                MessageBox.Show($"{checkbox.Content} wurde abgehakt!");
                // Hier kannst du zusätzliche Logik hinzufügen, um den Status zu speichern
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void AddToDoItem(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                ToDoListboxDashboard.Items.Add(item);
            }
        }
    }
}
