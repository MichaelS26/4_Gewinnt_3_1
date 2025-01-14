using System;

namespace VierGewinnt.Model.Spiellogik
{


    //  Enthält Felder[,] sowie Methoden zum Zurücksetzen, Spalten-/Brett-Vollprüfung und Gewinn-Prüfung
    /// Enthält : Reset, IstSpalteVoll, IstBrettVoll, SetzeZellenwert, HoleZellenwert, PruefeGewinn
    public class Spielbrett
    {
        public const int REIHEN = 6;
        public const int SPALTEN = 7;
        private int[,] felder;

        // Konstruktor => Felder erstellen und Reset auf 0
        public Spielbrett()
        {
            felder = new int[REIHEN, SPALTEN];
            Reset();
        }

        // Setzt alle Felder auf 0
        public void Reset()
        {
            for (int r = 0; r < REIHEN; r++)
            {
                for (int c = 0; c < SPALTEN; c++)
                {
                    felder[r, c] = 0;
                }
            }
        }

        // Oben (Reihe 0) belegt => Spalte voll
        public bool IstSpalteVoll(int sp)
        {
            return (felder[0, sp] != 0);
        }

        // Kein Feld == 0 => Brett voll
        public bool IstBrettVoll()
        {
            for (int r = 0; r < REIHEN; r++)
            {
                for (int c = 0; c < SPALTEN; c++)
                {
                    if (felder[r, c] == 0) return false;
                }
            }
            return true;
        }

        // Setzt den Wert in Felder[r, c]
        public void SetzeZellenwert(int r, int c, int val)
        {
            felder[r, c] = val;
        }

        // Gibt Feldwert 0..2 zurück
        public int HoleZellenwert(int r, int c)
        {
            return felder[r, c];
        }

        // Prüft 4er-Reihen: horizontal, vertikal, diagonal
        public bool PruefeGewinn(int sp)
        {
            // horizontal
            for (int r = 0; r < REIHEN; r++)
            {
                for (int c = 0; c < SPALTEN - 3; c++)
                {
                    if (felder[r, c] == sp &&
                       felder[r, c + 1] == sp &&
                       felder[r, c + 2] == sp &&
                       felder[r, c + 3] == sp) return true;
                }
            }
            // vertikal
            for (int r = 0; r < REIHEN - 3; r++)
            {
                for (int c = 0; c < SPALTEN; c++)
                {
                    if (felder[r, c] == sp &&
                       felder[r + 1, c] == sp &&
                       felder[r + 2, c] == sp &&
                       felder[r + 3, c] == sp) return true;
                }
            }
            // diagonal \
            for (int r = 0; r < REIHEN - 3; r++)
            {
                for (int c = 0; c < SPALTEN - 3; c++)
                {
                    if (felder[r, c] == sp &&
                       felder[r + 1, c + 1] == sp &&
                       felder[r + 2, c + 2] == sp &&
                       felder[r + 3, c + 3] == sp) return true;
                }
            }
            // diagonal /
            for (int r = 3; r < REIHEN; r++)
            {
                for (int c = 0; c < SPALTEN - 3; c++)
                {
                    if (felder[r, c] == sp &&
                       felder[r - 1, c + 1] == sp &&
                       felder[r - 2, c + 2] == sp &&
                       felder[r - 3, c + 3] == sp) return true;
                }
            }
            return false;
        }
    }
}
