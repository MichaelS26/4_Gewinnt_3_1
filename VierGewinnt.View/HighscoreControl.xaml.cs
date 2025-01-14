using System.Windows;
using System.Windows.Controls;
using VierGewinnt.Controller.Steuerung;

namespace VierGewinnt.View.Ansichten
{
    // Zeigt Highscore 
    // Ermöglicht das Löschen und erneutes Laden der Highscore-Einträge
    /// Enthält Methoden: LadeListe, BtnClear_Click, BtnClose_Click
    public partial class HighscoreControl : UserControl
    {
        private HauptSteuerung st;  
        private MainWindow mw;    

        // Speichert Steuerung und Hauptfenster, lädt anschließend die Highscore-Daten.
        public HighscoreControl(HauptSteuerung s, MainWindow w)
        {
            InitializeComponent();
            st = s; 
            mw = w;
            LadeListe();
        }

        // Lädt Einträge aus der Highscore-Liste 
        private void LadeListe()
        {
            var list = st.LadeHighscores();
            dgHighscore.ItemsSource = list;
        }

        // Löscht alle Einträge und lädt die Liste neu
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            st.AlleHighscoresLoeschen();
            LadeListe();
        }

        // Schließt den Highscore-Bildschirm und kehrt zum Hauptmenü zurück
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            mw.ZeigeMenue();
        }
    }
}
