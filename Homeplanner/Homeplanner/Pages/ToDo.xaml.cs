using System.Windows;
using System.Windows.Controls;

namespace Homeplanner.Pages
{
    /// <summary>
    /// Interaktionslogik für ToDo.xaml
    /// </summary>
    public partial class ToDo : Page
    {
        public ToDo()
        {
            InitializeComponent();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string ToDOText = InputTextBox.Text; // TextBox-Inhalt holen
            string filePath = "savedText.txt"; // Datei zum Speichern

            try
            {
                //File.WriteAllText(filePath, text); // Speichern in Datei
                MessageBox.Show("Text gespeichert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String text = InputTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(text)) // Nur hinzufügen, wenn nicht leer
            {
                ToDoListbox.Items.Add(text); // In ListBox einfügen
                InputTextBox.Clear(); // TextBox leeren
            }
            else
            {
                MessageBox.Show("Bitte Text eingeben!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ToDoListbox.SelectedItem != null)
            {
                ToDoListbox.Items.Remove(ToDoListbox.SelectedItem);
            }
            else
            {
                MessageBox.Show("Bitte ein Element auswählen!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        
    }
}
