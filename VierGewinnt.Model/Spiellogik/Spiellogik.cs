using System;

namespace VierGewinnt.Model.Spiellogik
{
    // Erhält ein Spielbrett-Objekt im Konstruktor
    // Bietet Methoden zum Platzieren eines Steins und zum Gewinn-Check
    /// Enthält Methoden: SteinFallenlassen, CheckWin
    public class SpielLogik
    {
        private Spielbrett brett;

        // Konstruktor => erhält Spielbrett-Referenz
        public SpielLogik(Spielbrett br)
        {
            brett = br;
        }

        // Legt Stein in "spalte" in die unterste freie Position
        /// Falls Spalte voll => false
        public bool SteinFallenlassen(int spalte, int spieler)
        {
            if (brett.IstSpalteVoll(spalte))
                return false;

            // Suche von unten nach oben
            for (int r = Spielbrett.REIHEN - 1; r >= 0; r--)
            {
                if (brett.HoleZellenwert(r, spalte) == 0)
                {
                    brett.SetzeZellenwert(r, spalte, spieler);
                    return true;
                }
            }
            return false;
        }

        // Prüft, ob "spieler" 4 in einer Reihe hat
        public bool CheckWin(int spieler)
        {
            return brett.PruefeGewinn(spieler);
        }
    }
}
