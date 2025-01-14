using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VierGewinnt.Persistence.Dateiverwaltung
{

   
    public class HighscoreEintrag
    {
        public string Name { get; set; } = "";
        public int Siege { get; set; }
    }
    // Liest/schreibt "Name;Siege" in score.txt und verwaltet bis zu 20 Einträge
    // Bietet Funktionalität zum Anlegen/Löschen/Begrenzen/Speichern
    /// Enthält Methoden: SpeichereMatch, AlleLoeschen, HoleHighscoreListe, etc.
    public class HighscoreSpeicher
    {
        private static readonly string DATEI_NAME
            = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                           "Persistence","score.txt");

        private List<HighscoreEintrag> eintraege;

        // Konstruktor => lädt ggf. bereits bestehende Daten aus Datei
        public HighscoreSpeicher()
        {
            eintraege= new List<HighscoreEintrag>();
            if(File.Exists(DATEI_NAME))
            {
                LadeDatei();
            }
        }

        // Speichert ein Match:
        // Falls Name neu => anlegen, Siege++ wenn gewonnen
        public void SpeichereMatch(string spielerName, bool hatGewonnen)
        {
            var e= eintraege.FirstOrDefault(x=> x.Name==spielerName);
            if(e==null)
            {
                e= new HighscoreEintrag{
                    Name= spielerName,
                    Siege=0
                };
                eintraege.Add(e);
            }
            if(hatGewonnen) e.Siege++;

            BegrenzeAuf20();
            SpeichereDatei();
        }

        // Stellt sicher, dass nur 20 Einträge vorhanden sind
        private void BegrenzeAuf20()
        {
            eintraege= eintraege
                .OrderByDescending(x=> x.Siege)
                .ThenBy(x=> x.Name)
                .ToList();

            while(eintraege.Count>20)
            {
                eintraege.RemoveAt(eintraege.Count-1);
            }
        }

        // Löscht alle Einträge + speichert leere Datei
        public void AlleLoeschen()
        {
            eintraege.Clear();
            SpeichereDatei();
        }

        // Gibt sortierte Liste (Siege desc, Name) zurück
        public List<HighscoreEintrag> HoleHighscoreListe()
        {
            return eintraege
                .OrderByDescending(x=> x.Siege)
                .ThenBy(x=> x.Name)
                .ToList();
        }

        // Versucht, Datei score.txt zu laden (Zeilen: "Name;Siege")
        private void LadeDatei()
        {
            try
            {
                var lines= File.ReadAllLines(DATEI_NAME);
                eintraege.Clear();
                foreach(var line in lines)
                {
                    var parts= line.Split(';');
                    if(parts.Length<2) continue;

                    string nm= parts[0];
                    if(!int.TryParse(parts[1], out int sg)) sg=0;

                    eintraege.Add(new HighscoreEintrag{
                        Name= nm,
                        Siege= sg
                    });
                }
            }
            catch
            {
                // Falls Datei kaputt oder nicht lesbar => ignorieren
            }
        }

        // Schreibt aktuelle Liste in score.txt
        private void SpeichereDatei()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DATEI_NAME)??".");
                if(!File.Exists(DATEI_NAME))
                {
                    using(File.Create(DATEI_NAME)){}
                }
                var lines= new List<string>();
                foreach(var e in eintraege)
                {
                    lines.Add($"{e.Name};{e.Siege}");
                }
                File.WriteAllLines(DATEI_NAME, lines);
            }
            catch
            {
                // Schreib-/Zugriffsfehler => ignorieren
            }
        }
    }
}
