using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Homeplanner.Pages
{
    public partial class Profile : Page
    {
        public Profile()
        {
            InitializeComponent();
        }

        private void ChangeProfilePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Bilddateien|*.jpg;*.png;*.jpeg",
                Title = "Wähle ein Profilbild"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                imgProfile.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
