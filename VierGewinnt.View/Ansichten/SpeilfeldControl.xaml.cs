using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using VierGewinnt.Controller.Steuerung;
using VierGewinnt.Model.Spiellogik;

namespace VierGewinnt.View.Ansichten
{
    // Stellt das eigentliche Spielfeld dar:
    // - 6x7 Board - Aktueller Spieler - Spalten-Buttons
    /// Enthält Methoden: BoardErzeugen, BrettAktualisieren, BtnSpalte_Click, ESC_losgelassen, TastenEingabe, BtnAufgeben_Click
    public partial class SpielfeldControl : UserControl
    {
        private HauptSteuerung steuerung;
        private MainWindow mainWindow;
        private Ellipse[,] kreise = new Ellipse[0, 0];

        // Konstruktor, verknüpft Events und baut Board
        public SpielfeldControl(HauptSteuerung s, MainWindow w)
        {
            InitializeComponent();
            steuerung = s; mainWindow = w;
            steuerung.SpielerHatGewonnen += SpielerHatGewonnenHandler;
            steuerung.Unentschieden += UnentschiedenHandler;
            steuerung.ManuellBeendet += ManuellBeendetHandler;
            steuerung.SpalteVoll += (msg) => {
                mainWindow.ZeigeKurzeMeldung(msg);
            };

            BoardErzeugen();
            BrettAktualisieren();
        }

        // Erzeugt das visuelle Spielbrett mit Ellipsen
        private void BoardErzeugen()
        {
            kreise = new Ellipse[Spielbrett.REIHEN, Spielbrett.SPALTEN];
            FelderGrid.Children.Clear();

            for (int r = 0; r < Spielbrett.REIHEN; r++)
            {
                for (int c = 0; c < Spielbrett.SPALTEN; c++)
                {
                    var ell = new Ellipse
                    {
                        Width = 30,
                        Height = 30,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Fill = new SolidColorBrush(Color.FromRgb(245, 224, 204))
                    };
                    FelderGrid.Children.Add(ell);
                    kreise[r, c] = ell;
                }
            }
        }

        // Aktualisiert die Farben der Ellipsen je nach Spieler-Wert 0..2
        private void BrettAktualisieren()
        {
            // Zeigt aktuellen Spieler
            txtSpieler.Text = (steuerung.AktuellerSpieler == 1)
                              ? steuerung.NameSpieler1
                              : steuerung.NameSpieler2;

            // Färbt alle Ellipsen passend ein
            for (int r = 0; r < Spielbrett.REIHEN; r++)
            {
                for (int c = 0; c < Spielbrett.SPALTEN; c++)
                {
                    int val = steuerung.HoleSpielbrettZellenwert(r, c);
                    kreise[r, c].Fill = val switch
                    {
                        1 => Brushes.Red,
                        2 => Brushes.Yellow,
                        _ => new SolidColorBrush(Color.FromRgb(245, 224, 204)),
                    };
                }
            }
        }

        // Button-Klick auf Spalte => Stein fallenlassen, Brett aktualisieren
        private void BtnSpalte_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag as string, out int sp))
            {
                if (steuerung.SteinFallenlassen(sp))
                    BrettAktualisieren();
            }
        }

        //  ESC-Taste => Aufgeben (Spieler beendet Spiel)
        public void ESC_losgelassen()
        {
            steuerung.SpielerBeendetManuell();
        }

        // Verarbeitet Tastatureingaben für das Einwerfen von Steinen
        public void TastenEingabe(int sp)
        {
            if (steuerung.SteinFallenlassen(sp))
                BrettAktualisieren();
        }

        // Aufgeben Button gedrückt
        private void BtnAufgeben_Click(object sender, RoutedEventArgs e)
        {
            int loser = steuerung.AktuellerSpieler;
            int winner = (loser == 1) ? 2 : 1;
            string gewinnerName = (winner == 1) ? steuerung.NameSpieler1 : steuerung.NameSpieler2;

            steuerung.AddMatchToHighscore(gewinnerName, true);
            steuerung.SpielerBeendetManuell();
        }

        //   Spieler XY hat gewonnen, Meldung zeigen, Zurück ins Menü
        private void SpielerHatGewonnenHandler(int sp)
        {
            Application.Current.Dispatcher.Invoke(() => {
                string gewinner = (sp == 1) ? steuerung.NameSpieler1 : steuerung.NameSpieler2;
                mainWindow.ZeigeKurzeMeldung($"{gewinner} hat gewonnen!");
                mainWindow.ZeigeMenue();
            });
        }

        //  Unentschieden, Meldung, zurück ins Menü
        private void UnentschiedenHandler()
        {
            Application.Current.Dispatcher.Invoke(() => {
                mainWindow.ZeigeKurzeMeldung("Unentschieden! Brett ist voll.");
                mainWindow.ZeigeMenue();
            });
        }

        //  Manuell Beendet (Aufgegeben), Meldung (Gegner Gewinnt), zurück ins Menü
        private void ManuellBeendetHandler(int loser, int winner)
        {
            Application.Current.Dispatcher.Invoke(() => {
                string verlierer = (loser == 1) ? steuerung.NameSpieler1 : steuerung.NameSpieler2;
                string gewinner = (winner == 1) ? steuerung.NameSpieler1 : steuerung.NameSpieler2;
                mainWindow.ZeigeKurzeMeldung($"{verlierer} hat aufgegeben,\n{gewinner} gewinnt!");
                mainWindow.ZeigeMenue();
            });
        }
    }
}
