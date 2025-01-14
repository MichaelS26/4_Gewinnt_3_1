using System.Windows;
using System.Windows.Controls;
using VierGewinnt.Controller.Steuerung;

namespace VierGewinnt.View.Ansichten
{
    
    public partial class StartMenueControl : UserControl
    {
        // Hauptsteuerung,  MainWindow 
        private  HauptSteuerung st;
        private  MainWindow mw;

        // Lädt das Startmenü-Layout und speichert Steuerung + Hauptfenster.
        public StartMenueControl(HauptSteuerung s, MainWindow w)
        {
            InitializeComponent();
            st = s; mw = w;
        }

        // Zeigt die Highscore-Ansicht.
        private void BtnHighscore_Click(object sender, RoutedEventArgs e)
        {
            mw.ZeigeHighscore();
        }

        // Startet die Namenseingabe.
        private void BtnSpielStarten_Click(object sender, RoutedEventArgs e)
        {
            mw.ZeigeNamenseingabe();
        }

        // Beendet die Anwendung.
        private void BtnBeenden_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
