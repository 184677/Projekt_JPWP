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
        public int type; //Rodzaj klocka: 1-porusza sie w pionie, 2-porusza sie w poziomie, 3-porusza sie w pionie i poziomie
        public int width; //Szerokosc ikony obiektu
        public int height; //Wysokosc ikony obiektu
        public Rectangle rec; //stworzenie prostokata, umozliwia pobieranie obecnych wspolrzednych
        public Image icon; //Ikona obiektu

        /**
         * Konstruktor - ustawienie parametrów obiektu, 
         * @param x początkowa współrzędna x
         * @param y początkowa współrzędna y
         * @param type rodzaj klocka - poruszanie w poziomie/pionie lub we wszystkich kierunkach
         * @param width szerokosc klocka
         * @param height wysokosc klocka
        */

        public Block(int x, int y, int width, int height, int type, Image icon)
       {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.type = type;
            this.icon = icon;
            this.rec = new Rectangle(x, y, width, height);
        }

    }
}
