using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LabiryntWiedzy
{
    public class Block
    {
        public int x; //Poczatkowa wspolrzendna x obiektu
        public int y; //Poczatkowa wspolrzendna y obiektu
        public int currX; //Aktualna wspolrzendna x obiektu
        public int currY; //Aktualna wspolrzendna y obiektu
        public int type; //Rodzaj klocka
        public int width; //Szerokosc ikony obiektu
        public int height; //Wysokosc ikony obiektu
        public int sWidth; //Szerokosc pola graficznego
        public int sHeight; //Wysokosc pola graficznego
        public Image icon; //Ikona obiektu

        /**
         * Konstruktor - ustawienie parametrów obiektu, 
         * @param x początkowa współrzędna x
         * @param y początkowa współrzędna y
        */

       public Block(int x, int y, Image [] images)
       {
            this.x = x;
            this.y = y;
            currX = x;
            currY = y;
            sWidth = 1024;
            sHeight = 768;
            icon = images[5];
            width = icon.Width;
            height = icon.Height;
       }

    }
}
