using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Homeplanner.Pages
{
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
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

        // Handle the DragOver event to specify the allowed effects
        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        // Handle the Drop event to swap the widgets
        private void OnDrop(object sender, DragEventArgs e)
        {
            var targetBorder = sender as Border;
            var draggedBorder = e.Data.GetData(typeof(Border)) as Border;

            if (targetBorder != null && draggedBorder != null && targetBorder != draggedBorder)
            {
                // Swap the content of the borders
                var targetRow = Grid.GetRow(targetBorder);
                var targetColumn = Grid.GetColumn(targetBorder);

                var draggedRow = Grid.GetRow(draggedBorder);
                var draggedColumn = Grid.GetColumn(draggedBorder);

                // Swap the grid positions of the borders
                Grid.SetRow(targetBorder, draggedRow);
                Grid.SetColumn(targetBorder, draggedColumn);
                Grid.SetRow(draggedBorder, targetRow);
                Grid.SetColumn(draggedBorder, targetColumn);
            }
        }

       
    }
}
