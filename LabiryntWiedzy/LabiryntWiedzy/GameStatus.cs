using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LabiryntWiedzy
{
    /// <summary>
    /// Klasa pomocnicza z zapisanem stanu gry
    /// </summary>
    public class GameStatus
    {
        /// <summary>
        /// Liczba punktów za poprawne odpowiedzi
        /// </summary>
        public int points;
        /// <summary>
        /// Numer poziomu
        /// </summary>
        public int level;
        /// <summary>
        ///  Zmienna do przechowywania czasów przejścia poszczególnych poziomów i ilości prawidłowych odpowiedzi
        /// </summary>
        public StringBuilder sb = new StringBuilder(1000);

        /// <summary>
        /// Zerowanie parametrow gry
        /// </summary>
        public void reset()
        {
            points = 0;
            level = 1;
        }//koniec reset()

        /// <summary>
        /// Zerowanie liczby punktów
        /// </summary>
        public void resetPoints() 
        {
            points = 0;
        }//koniec resetPoints()

        /// <summary>
        /// Zwiększenia wskaźnika poziomu
        /// </summary>
        public void nextLevel() 
        {
            if (level < GPars.noOfLevels) level++;
            else GPars.end = true;

        }//koniec nextLevel()

        /// <summary>
        /// Zapisanie numeru poziomu, czasu przejścia i czy wybrano prawidłową odpowiedź
        /// </summary>
        public void saveLevelPassTime() // zapisanie czasu przejscia poziomu
        {
            TimeSpan stopwatchElapsed = GPars.stopwatch.Elapsed;
            string time = ", czas przejścia: " + stopwatchElapsed.Minutes + "m " + stopwatchElapsed.Seconds + "s " + stopwatchElapsed.Milliseconds + "ms";
            string ans = "answer";

            if (GPars.rightAns) ans = "TAK";
            else ans = "NIE";

            StringBuilder dane_label = new StringBuilder("Poziom nr: " + level + time + ", czy prawidłowa odpowiedź: " +ans+"\n");
            sb.Append(dane_label);
        }// koniec saveLevelPassTime()

        /// <summary>
        /// Zapisanie wyników gry - czasów przejścia poszczególnych poziomów i ilości prawidłowych odpowiedzi
        /// </summary>
        public void saveResults() // zapisanie czasow przejscia poszczegolnych poziomow i ilosci prawidlowych odpowiedzi
        {
            using (StreamWriter sw = new StreamWriter("WynikiGry.txt"))
            {
                sw.WriteLine(sb);
            }
        }// koniec saveResults()

        /// <summary>
        /// Usunięcie wyników gry z poprzedniego uruchomienia gry
        /// </summary>
        public void deleteResults()
        {
            File.Delete("WynikiGry.txt");
        }// koniec deleteResults()
    }//koniec class GameStatus
}
