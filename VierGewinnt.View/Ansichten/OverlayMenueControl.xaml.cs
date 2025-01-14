using System.Windows;
using System.Windows.Controls;
using VierGewinnt.Controller.Steuerung;

namespace VierGewinnt.View.Ansichten
{
    //  Stellt das Overlay-Menü mit drei Hauptfunktionen bereit
    // - Highscores anzeigen - Spiel starten - Beenden
    /// Enthält Methoden: BtnHighscores_Click, BtnSpielStarten_Click, BtnBeenden_Click
    public partial class OverlayMenueControl : UserControl
    {
        // Referenz zur Steuerung und MainWindow
        private HauptSteuerung st;
        private MainWindow mw;

        // Konstruktor: Erhält HauptSteuerung und MainWindow
        public OverlayMenueControl(HauptSteuerung s, MainWindow w)
        {
            InitializeComponent();
            st = s; mw = w;
        }
        /// Buttons 
        
        // Zeigt Highscore-Ansicht
        private void BtnHighscores_Click(object sender, RoutedEventArgs e)
        {
            mw.ZeigeHighscore();
        }

        // Wechselt zur Namenseingabe
        private void BtnSpielStarten_Click(object sender, RoutedEventArgs e)
        {
            mw.ZeigeNamenseingabe();
        }

        // Schließt die Anwendung
        private void BtnBeenden_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
