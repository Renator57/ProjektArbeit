using System.Text;
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
            DateTime? selectedDate = DatumFeld.SelectedDate;

            if (selectedDate == null)
            {
                MessageBox.Show("Bitte ein Datum auswählen!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } else if (!string.IsNullOrEmpty(text)) // Nur hinzufügen, wenn nicht leer
            {
             
                InputTextBox.Clear(); // TextBox leeren
                string item = $"{selectedDate.Value.ToShortDateString()} - {text}";
                ToDoListbox.Items.Add(item);

            }
            else
            {
                MessageBox.Show("Bitte Text eingeben!", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Aufgabe + Datum in die ListBox einfügen
          
        }
        public string GetToDoItemsAsString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in ToDoListbox.Items)
            {
                sb.AppendLine(item.ToString());
            }

            return sb.ToString();
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
