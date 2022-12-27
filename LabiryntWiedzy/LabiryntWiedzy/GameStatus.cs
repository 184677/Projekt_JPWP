﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LabiryntWiedzy
{
    public class GameStatus
    {
        public int points; // Liczba punktow za poprawne odpowiedzi
        public int level; //Numer poziomu
        public double time; //Czas gry na danym poziomie

        public void reset() // Zerowanie parametrow gry
        {
            points = 0;
            level = 1;
            time = 0.0;
        }//reset()
 
        public void resetPoints() // zerowanie liczby punktow
        {
            points = 0;
        }//resetPoints()
    
        public void nextLevel() // zwiekszenie wskaznika poziomu
        {
            level++;
        }//nextLevel()
    }
}