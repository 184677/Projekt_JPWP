using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LabiryntWiedzy
{
    public class GameStatus
    {
        public int points; // Liczba punktow za poprawne odpowiedzi
        public int level; //Numer poziomu
        public StringBuilder sb = new StringBuilder(1000); // zmienna do przechowywania czasow przejscia poszczegolnych poziomow i ilosci prawidlowych odpowiedzi

        public void reset() // Zerowanie parametrow gry
        {
            points = 0;
            level = 1;
        }//reset()
 
        public void resetPoints() // zerowanie liczby punktow
        {
            points = 0;
        }//resetPoints()

        public void nextLevel() // zwiekszenie wskaznika poziomu
        {
            if (level < GPars.noOfLevels) level++;
            else GPars.end = true;

        }//nextLevel()

        public void saveLevelPassTime() // zapisanie czasu przejscia poziomu
        {
            TimeSpan stopwatchElapsed = GPars.stopwatch.Elapsed;
            string time = ", czas przejścia: " + stopwatchElapsed.Minutes + "m " + stopwatchElapsed.Seconds + "s " + stopwatchElapsed.Milliseconds + "ms";
            string ans = "answer";

            if (GPars.rightAns) ans = "TAK";
            else ans = "NIE";

            StringBuilder dane_label = new StringBuilder("Poziom nr: " + level + time + ", czy prawidłowa odpowiedź: " +ans+"\n");
            sb.Append(dane_label);
        }

        public void saveResults() // zapisanie czasow przejscia poszczegolnych poziomow i ilosci prawidlowych odpowiedzi
        {
            using (StreamWriter sw = new StreamWriter("WynikiGry.txt"))
            {
                sw.WriteLine(sb);
            }
        }

        public void deleteResults() // usuniecie czasow przejscia poszczegolnych poziomow i ilosci prawidlowych odpowiedzi
        {
            File.Delete("WynikiGry.txt");
        }
    }
}
