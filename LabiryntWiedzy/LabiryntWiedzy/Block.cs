using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LabiryntWiedzy
{
    public class Block
    {
        public int x; //Poczatkowa wspolrzedna x obiektu
        public int y; //Poczatkowa wspolrzedna y obiektu
        public int currX; //Aktualna wspolrzedna x obiektu
        public int currY; //Aktualna wspolrzedna y obiektu
        //public int type; //Rodzaj klocka
        public int width; //Szerokosc ikony obiektu
        public int height; //Wysokosc ikony obiektu
        public Rectangle rec;
        public Image icon; //Ikona obiektu

        /**
         * Konstruktor - ustawienie parametrów obiektu, 
         * @param x początkowa współrzędna x
         * @param y początkowa współrzędna y
        */

        public Block(int x, int y, int width, int height, Image icon)
       {
            this.x = x;
            this.y = y;
            currX = x;
            currY = y;
            this.icon = icon;
            this.width = width;
            this.height = height;
            this.rec = new Rectangle(x, y, width, height);
        }

    }
}
