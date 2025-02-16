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

        // Handle the PreviewMouseLeftButtonDown event to start the drag operation
        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Wenn der Klick auf den Border erfolgt und nicht auf eine CheckBox oder innerhalb der ListBox
            if (sender is Border border && !(e.OriginalSource is CheckBox) && !IsInsideListBox(e.OriginalSource))
            {
                // Nur dann Drag & Drop starten, wenn nicht auf eine CheckBox oder innerhalb der ListBox
                DragDrop.DoDragDrop(border, border, DragDropEffects.Move);
            }
        }

        // Helper method to check if the click is inside a ListBox or CheckBox
        private bool IsInsideListBox(object source)
        {
            return source is ListBox || (source is FrameworkElement element && element.Parent is ListBox);
        }

       
    }
}
