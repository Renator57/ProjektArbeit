using System.Windows;
using System.Windows.Input;

namespace Homeplanner
{
    public partial class Login : Window
    {
        private bool isLoggedIn = false;  // Flag, um mehrfaches Login zu verhindern

        public Login()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        // Login-Logik (dieser Code wird bei Button-Klick oder Enter ausgeführt)
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Verhindert mehrfaches Öffnen des Hauptfensters
            if (!isLoggedIn)
            {
                PerformLogin();
            }
        }

        // Wenn Enter gedrückt wird, soll Login durchgeführt werden
        private void LoginFields_KeyDown(object sender, KeyEventArgs e)
        {
            // Wenn Enter gedrückt wird und noch kein Login erfolgt ist
            if (e.Key == Key.Return && !isLoggedIn)
            {
                PerformLogin();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Wenn Enter gedrückt wird und noch kein Login erfolgt ist
            if (e.Key == Key.Return && !isLoggedIn)
            {
                PerformLogin();
            }
        }

        private void PerformLogin()
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Beispielhafte Login-Prüfung (Hier kannst du deine Authentifizierung einfügen)
            if (username == "admin" && password == "1234")
            {
                // Login erfolgreich - MainWindow öffnen und Login-Fenster schließen
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();  // Öffnet das Hauptfenster

                this.Close();  // Schließt das Login-Fenster
                isLoggedIn = true;  // Setzt das Flag auf true, damit das Login nicht erneut ausgeführt wird
            }
            else
            {
                ErrorMessage.Text = "Falscher Benutzername oder Passwort!";
            }
        }
    }
}
