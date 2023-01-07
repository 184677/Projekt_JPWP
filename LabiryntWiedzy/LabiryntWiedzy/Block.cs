using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LabiryntWiedzy
{
    /// <summary>
    /// Klasa modelująca przesuwany klocek
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Początkowa współrzędna x obiektu
        /// </summary>
        public int x;
        /// <summary>
        /// Początkowa współrzędna y obiektu
        /// </summary>
        public int y;
        /// <summary>
        /// Rodzaj klocka: 1-porusza się w pionie, 2-porusza się w poziomie, 3-porusza się w pionie i poziomie
        /// </summary>
        public int type;
        /// <summary>
        /// Szerokość ikony obiektu
        /// </summary>
        public int width;
        /// <summary>
        /// Wysokość ikony obiektu
        /// </summary>
        public int height;
        /// <summary>
        /// Prostokąt do pobrania obecnych współrzędnych
        /// </summary>
        public Rectangle rec;
        /// <summary>
        /// Ikona obiektu
        /// </summary>
        public Image icon;

        /// <summary>
        /// Konstruktor - ustawienie parametrów obiektu
        /// </summary>
        /// <param name="x">Początkowa współrzędna x</param>
        /// <param name="y">Początkowa współrzędna y</param>
        /// <param name="width">Szerokość klocka</param>
        /// <param name="height">Wysokość klocka</param>
        /// <param name="type">Typ klocka - rozróżnienie sposobu poruszania</param>
        /// <param name="icon">Ikona klocka</param>
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

        /// <summary>
        /// Sprawdzenie możliwości poruszania się klocka - 
        /// czy nie wystąpiła kolizja z innym klockiem i czy nie przekroczono granic labiryntu
        /// </summary>
        /// <param name="i">Numer klocka w celu uniknięcia sprawdzania kolizji klocka z samym sobą</param>
        /// <param name="x_dif">Wartość przesunięcia na osi X o którą klocek ma być przesunięty</param>
        /// <param name="y_dif">Wartość przesunięcia na osi Y o którą klocek ma być przesunięty</param>
        /// <returns>Flaga informująca o możliwości wykonania ruchu</returns>
        public Boolean MoveCheck(int i, int x_dif, int y_dif)
        {
            bool flag_collision = true; // Kolizja z innym klockiem
            bool flag_border = true; // Kolizja z granicami labiryntu

            int temp_h = 0;
            int temp_w = 0;
            int temp_x = 0;
            int temp_y = 0;
            if (x_dif > 0) // Przesunięcie klocka w prawo
            {
                temp_x = this.rec.X;
                temp_y = this.rec.Y;
                temp_w = this.rec.Width + x_dif;
                temp_h = this.rec.Height;
            }
            else if (x_dif < 0) // Przesunięcie klocka w lewo
            {
                temp_x = this.rec.X + x_dif;
                temp_y = this.rec.Y;
                temp_w = this.rec.Width + Math.Abs(x_dif);
                temp_h = this.rec.Height;
            }
            else if (y_dif > 0) // Przesunięcie klocka w dół
            {
                temp_x = this.rec.X;
                temp_y = this.rec.Y;
                temp_w = this.rec.Width;
                temp_h = this.rec.Height + y_dif;
            }
            else if (y_dif < 0) // Przesunięcie klocka w górę
            {
                temp_x = this.rec.X;
                temp_y = this.rec.Y + y_dif;
                temp_w = this.rec.Width;
                temp_h = this.rec.Height + Math.Abs(y_dif);
            }

            Rectangle temp_rec = new Rectangle(temp_x, temp_y, temp_w, temp_h);

            for (int j = 0; j < GPars.noOfObjects; j++) // Sprawdzenie kolizji między klockami
            {
                if (i == j) continue;
                if (temp_rec.IntersectsWith(GamePanel.blocks[j].rec))
                {
                    flag_collision = false;
                    break;
                }
            }

            if (temp_rec.Left > 119 && temp_rec.Right < (GamePanel.labirynthRec.Width + GamePanel.labirynthRec.X+1) && temp_rec.Top > (GamePanel.labirynthRec.Y-1) && temp_rec.Bottom < (GamePanel.labirynthRec.Y + 1 + GamePanel.labirynthRec.Height)) flag_border = true;
            // Sprawdzenie czy klocek czerwony przechodzi przez wyjście nr 2
            else if (GamePanel.blocks[i].type == 3 && temp_rec.Top > (GamePanel.labirynthRec.Y + 75) && temp_rec.Bottom < (GamePanel.labirynthRec.Y + 170) && temp_rec.Left > (GamePanel.labirynthRec.X + 225)) flag_border = true;
            // Sprawdzenie czy klocek czerwony przechodzi przez wyjście nr 1
            else if (GamePanel.blocks[i].type == 3 && temp_rec.Bottom < (GamePanel.labirynthRec.Y + GamePanel.labirynthRec.Height - 75) && temp_rec.Top > (GamePanel.labirynthRec.Y + GamePanel.labirynthRec.Height - 170) && temp_rec.Left < (GamePanel.labirynthRec.X + 225)) flag_border = true;
            else flag_border = false;

            return flag_collision && flag_border;
        }// koniec MoveCheck()

        /// <summary>
        /// Tworzenie serii połączonych linii do narysowania prostokąta z zaokrąglonymi rogami
        /// </summary>
        /// <param name="bounds">Granice rysowanego prostokąta</param>
        /// <param name="radius">Promień zaokrąglonych rogów</param>
        /// <returns>Rysnuek prostokąta z zaokrąglonymi rogami</returns>
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

            // lewy górny łuk 
            path.AddArc(arc, 180, 90);

            // prawy górny łuk   
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // dolny prawy łuk  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // dolny lewy łuk
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }// koniec RoundedRect()

    }// koniec class Block
}
