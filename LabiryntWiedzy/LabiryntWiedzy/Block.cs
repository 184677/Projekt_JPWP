using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

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

        public static void CheckCollisions(ref Rectangle rect1, ref Rectangle rect2)
        {
            bool touching_right =
            rect1.Right >= rect2.Left &&
            rect1.Left < rect2.Left &&
            rect1.Bottom > rect2.Top &&
            rect1.Top < rect2.Bottom;

            bool touching_left =
            rect1.Left <= rect2.Right &&
            rect1.Right > rect2.Right &&
            rect1.Bottom > rect2.Top &&
            rect1.Top < rect2.Bottom;

            bool touching_bottom =
            rect1.Bottom >= rect2.Top &&
            rect1.Top < rect2.Top &&
            rect1.Right > rect2.Left &&
            rect1.Left < rect2.Right;

            bool touching_top =
            rect1.Top <= rect2.Bottom &&
            rect1.Bottom > rect2.Bottom &&
            rect1.Right > rect2.Left &&
            rect1.Left < rect2.Right;

            if (touching_left && !touching_right && !touching_bottom && !touching_top) rect1.Location = new Point(rect1.Left + 1, rect1.Top); // Collision LEFT side
            else if (!touching_left && touching_right && !touching_bottom && !touching_top) rect1.Location = new Point(rect1.Left - 1, rect1.Top); //Collision RIGHT
            else if (!touching_left && !touching_right && !touching_bottom && touching_top) rect1.Location = new Point(rect1.Left, rect1.Top + 1); //Collision TOP side
            else if (!touching_left && !touching_right && touching_bottom && !touching_top) rect1.Location = new Point(rect1.Left, rect1.Top - 1); //Collsion BOTTOM side
            else if (!touching_left && touching_right && touching_bottom && !touching_top) // Collision right bottom corner
            {
                if (rect1.Right - rect2.Left > rect1.Bottom - rect2.Top) rect1.Location = new Point(rect1.Left, rect1.Top - 1); //ouching BOTTOM on corner
                else rect1.Location = new Point(rect1.Left - 1, rect1.Top); //Collision RIGHT on corner
            }
            else if (!touching_left && touching_right && !touching_bottom && touching_top) //Collision right top corner
            {
                if (rect1.Right - rect2.Left > rect2.Bottom - rect1.Top) rect1.Location = new Point(rect1.Left, rect1.Top + 1); // Touching TOP on corner
                else rect1.Location = new Point(rect1.Left - 1, rect1.Top); //Collision RIGHT on corner
            }
            else if (touching_left && !touching_right && touching_bottom && !touching_top) // Collision left bottom corner
            {
                if (rect2.Right - rect1.Left > rect1.Bottom - rect2.Top) rect1.Location = new Point(rect1.Left, rect1.Top - 1); //Touching BOTTOM on corner
                else rect1.Location = new Point(rect1.Left + 1, rect1.Top); //Collison LEFT on corner
            }
            else if (touching_left && !touching_right && !touching_bottom && touching_top) // Collision left top corner
            {
                if (rect2.Right - rect1.Left > rect2.Bottom - rect1.Top) rect1.Location = new Point(rect1.Left, rect1.Top + 1); //Touching TOP on corner
                else rect1.Location = new Point(rect1.Left + 1, rect1.Top); //Collision LEFT on corner
            }
        } // CheckCollsions()

        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

    }
}
