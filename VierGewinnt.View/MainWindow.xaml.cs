using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VierGewinnt.Controller.Steuerung;
using VierGewinnt.View.Ansichten;

namespace VierGewinnt.View
{
    // Stellt das Hauptfenster des Spiels dar
    // Initialisiert die Steuerung und zeigt verschiedene Ansichten
    /// Enthält: ZeigeHighscore, ZeigeNamenseingabe, ZeigeSpielfeld, ZeigeMenue, ZeigeKurzeMeldung, Window_KeyDown
    public partial class MainWindow : Window
    {
        private HauptSteuerung steuerung;

        // Konstruktor, erzeugt HauptSteuerung und lädt Hauptmenü
        public MainWindow()
        {
            InitializeComponent();

            // Erzeugt Steuerung und lädt das Hauptmenü (OverlayMenueControl)
            steuerung = new HauptSteuerung();
            InhaltControl.Content = new OverlayMenueControl(steuerung, this);
        }

        // Zeigt den Highscore-Bildschirm an
        public void ZeigeHighscore()
        {
            InhaltControl.Content = new HighscoreControl(steuerung, this);
        }

        // Zeigt die Eingabeansicht für Spielernamen
        public void ZeigeNamenseingabe()
        {
            InhaltControl.Content = new NamensEingabeControl(steuerung, this);
        }

        // Wechselt zur Spielfeldansicht
        public void ZeigeSpielfeld()
        {
            InhaltControl.Content = new SpielfeldControl(steuerung, this);
        }

        // Zeigt das Hauptmenü an (Overlay-Menü)
        public void ZeigeMenue()
        {
            InhaltControl.Content = new OverlayMenueControl(steuerung, this);
        }

        // Zeigt eine kurze Meldung als halbtransparente Messagebox
        // Verschwindet nach 1 Sekunde automatisch
        public void ZeigeKurzeMeldung(string nachricht)
        {
            txtAnzeige.Text = nachricht;
            OverlayMeldung.Visibility = Visibility.Visible;

            var t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(1);
            t.Tick += (s, e) =>
            {
                t.Stop();
                OverlayMeldung.Visibility = Visibility.Collapsed;
            };
            t.Start();
        }

        // Tastaturabfrage: ESC => Aufgeben, 1..7 => Stein in Spalte
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // ESC losgelassen => Spielfeld reagiert
            if (e.Key == Key.Escape && InhaltControl.Content is SpielfeldControl sc)
            {
                sc.ESC_losgelassen();
            }

            // Taste 1..7 => an Spielfeld weiterleiten 
            if (e.Key >= Key.D1 && e.Key <= Key.D7 && InhaltControl.Content is SpielfeldControl sc2)
            {
                int sp = (int)e.Key - (int)Key.D1;
                sc2.TastenEingabe(sp);
            }
        }
    }
}
