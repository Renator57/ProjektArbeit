using System.Windows;

namespace Homeplanner
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Beispiel für Benutzerdaten
            string validUsername = "admin";
            string validPassword = "1234";

            // Benutzername und Passwort vom UI holen
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Überprüfen, ob die eingegebenen Daten korrekt sind
            if (username == validUsername && password == validPassword)
            {
                // Login erfolgreich - MainWindow öffnen und Login-Fenster schließen
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();  // Öffnet das Hauptfenster

                this.Close();  // Schließt das Login-Fenster
            }
            else
            {
                // Fehlermeldung anzeigen
                ErrorMessage.Text = "Benutzername oder Passwort sind falsch!";
            }
        }
    }
}
