using System.Windows;
using System.Windows.Controls;
using VierGewinnt.Controller.Steuerung;

namespace VierGewinnt.View.Ansichten
{
    // Benutzersteuerung für Eingabe der Spielernamen
    // Prüft Gültigkeit der eingegebenen Namen und startet ggf. ein neues Spiel
    /// Enthält Methoden: BtnOk_Click, BtnAbbrechen_Click, CheckName
    public partial class NamensEingabeControl : UserControl
    {
        private HauptSteuerung steuerung; 
        private MainWindow mainWindow;

        // Konstruktor erhält HauptSteuerung und MainWindow
        public NamensEingabeControl(HauptSteuerung s, MainWindow w)
        {
            InitializeComponent();
            steuerung = s; mainWindow = w;
        }

        // Klick auf "OK" Button: Validiert beide Namen, startet neues Spiel bei Erfolg
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            // Liest Text aus den beiden TextBoxen
            string p1 = txtP1.Text.Trim(); 
            string p2 = txtP2.Text.Trim(); 

            // Prüft, ob beide Namen nur Buchstaben (1..8) enthalten
            if (!CheckName(p1) || !CheckName(p2))
            {
                txtFehler.Text = "Nur Buchstaben (1..8)!";
                txtFehler.Visibility = Visibility.Visible;
                return;
            }

            // Prüft, ob Namen verschieden sind
            if (p1 == p2)
            {
                txtFehler.Text = "Namen dürfen nicht gleich sein!";
                txtFehler.Visibility = Visibility.Visible;
                return;
            }

            // Neues Spiel mit eingegebenen Namen starten, zum Spielfeld wechseln
            steuerung.NeuesSpielStarten(p1, p2);
            mainWindow.ZeigeSpielfeld(); 
        }

        // Klick-Handler für "Abbrechen": Zurück ins Hauptmenü
        private void BtnAbbrechen_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ZeigeMenue(); 
        }

        //  Prüft, ob ein Name gültig ist (1..8 Buchstaben, keine Ziffern/Sonderzeichen)
        private bool CheckName(string n)
        {
            if (n.Length < 1 || n.Length > 8) return false; 
            foreach (char c in n)
            {
                if (!char.IsLetter(c)) return false; 
            }
            return true;
        }
    }
}
