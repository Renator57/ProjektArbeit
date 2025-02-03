using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Homeplanner
{
    public partial class MainWindow : Window
    {
        private bool isMenuOpen = false;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Uri("Pages/Dashboard.xaml", UriKind.Relative)); // Startseite
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
            Button clickedButton = sender as Button;
            string pagePath = clickedButton.Tag.ToString();
            MainFrame.Navigate(new Uri(pagePath, UriKind.Relative));



        }
    }
}
