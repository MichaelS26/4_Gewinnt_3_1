using System;
using System.Collections.Generic;
using VierGewinnt.Model.Spiellogik; 
using VierGewinnt.Persistence.Dateiverwaltung;

namespace VierGewinnt.Controller.Steuerung
{
    /// Delegate: Manuell Spiel Beenden
    public delegate void SpielEndeHandler(int loser, int winner);

    // Enthält den Spielfluss (aktueller Spieler, SteinFallenlassen, usw.)
    // Verknüpft Spielbrett/SpielLogik und Highscore
    /// Enthält Methoden und Events: NeuesSpielStarten, SteinFallenlassen, SpielerBeendetManuell, AddMatchToHighscore, AlleHighscoresLoeschen, LadeHighscores
    public class HauptSteuerung
    {
        private Spielbrett brett;
        private SpielLogik logik;
        private HighscoreSpeicher highscore;

        public int AktuellerSpieler { get; private set; } = 1;
        public string NameSpieler1 { get; private set; } = "";
        public string NameSpieler2 { get; private set; } = "";

        public event SpielEndeHandler? ManuellBeendet;
        public event Action<int>? SpielerHatGewonnen;
        public event Action? Unentschieden;
        public event Action<string>? SpalteVoll;

        // Konstruktor => Initialisiert Spielbrett, SpielLogik, HighscoreVerwaltung
        public HauptSteuerung()
        {
            brett = new Spielbrett();
            logik = new SpielLogik(brett);
            highscore = new HighscoreSpeicher();
        }

        // Startet ein neues Spiel => Brett-Reset, aktiver Spieler=1
        public void NeuesSpielStarten(string p1, string p2)
        {
            NameSpieler1 = p1; NameSpieler2 = p2;
            brett.Reset();
            AktuellerSpieler = 1;
        }

        // Legt Stein in Spalte ab => Event SpalteVoll, SpielerHatGewonnen, Unentschieden prüfen
        public bool SteinFallenlassen(int spalte)
        {
            bool ok = logik.SteinFallenlassen(spalte, AktuellerSpieler);
            if (!ok)
            {
                SpalteVoll?.Invoke("Spalte ist voll!");
                return false;
            }
            // check Win
            if (logik.CheckWin(AktuellerSpieler))
            {
                SpielerHatGewonnen?.Invoke(AktuellerSpieler);
                var w = (AktuellerSpieler == 1) ? NameSpieler1 : NameSpieler2;
                AddMatchToHighscore(w, true);
                return true;
            }
            // check Brett voll => Unentschieden
            if (brett.IstBrettVoll())
            {
                Unentschieden?.Invoke();
                return true;
            }
            // Nächster Spieler
            AktuellerSpieler = (AktuellerSpieler == 1) ? 2 : 1;
            return true;
        }

        // Spieler bricht ab => loser=aktueller, winner=anderer
        public void SpielerBeendetManuell()
        {
            int loser = AktuellerSpieler;
            int winner = (loser == 1) ? 2 : 1;
            ManuellBeendet?.Invoke(loser, winner);
        }

        // Fügt gewonnenes Match in die Highscore-Liste
        public void AddMatchToHighscore(string spielerName, bool gw)
        {
            highscore.SpeichereMatch(spielerName, gw);
        }

        // Löscht alle Einträge der Highscore-Liste
        public void AlleHighscoresLoeschen()
        {
            highscore.AlleLoeschen();
        }

        // Lädt die sortierte Highscore-Liste
        public List<HighscoreEintrag> LadeHighscores()
        {
            return highscore.HoleHighscoreListe();
        }

        // Zugriff auf das Brett -> Wert an Position (r,c)
        public int HoleSpielbrettZellenwert(int r, int c)
        {
            return brett.HoleZellenwert(r, c);
        }
    }
}
