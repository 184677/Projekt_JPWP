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



        public static Boolean MoveCheck(Rectangle rect, int i, int x_dif, int y_dif)
        {
            bool flag_collision = true;
            bool flag_border = true;

            int temp_h = 0;
            int temp_w = 0;
            int temp_x = 0;
            int temp_y = 0;
            if (x_dif > 0)
            {
                temp_x = rect.X;
                temp_y = rect.Y;
                temp_w = rect.Width + x_dif;
                temp_h = rect.Height;
            }
            else if (x_dif < 0)
            {
                temp_x = rect.X + x_dif;
                temp_y = rect.Y;
                temp_w = rect.Width + Math.Abs(x_dif);
                temp_h = rect.Height;
            }
            else if (y_dif > 0)
            {
                temp_x = rect.X;
                temp_y = rect.Y;
                temp_w = rect.Width;
                temp_h = rect.Height + y_dif;
            }
            else if (y_dif < 0)
            {
                temp_x = rect.X;
                temp_y = rect.Y + y_dif;
                temp_w = rect.Width;
                temp_h = rect.Height + Math.Abs(y_dif);
            }

            Rectangle temp_rec = new Rectangle(temp_x, temp_y, temp_w, temp_h);

            for (int j = 0; j < GPars.noOfObjects; j++) // sprawdzenie kolizji miedzy klockami
            {
                if (i == j) continue;
                if (temp_rec.IntersectsWith(GamePanel.blocks[j].rec))
                {
                    flag_collision = false;
                    break;
                }
            }

            if (temp_rec.Left > 119 && temp_rec.Right < (415 + 121) && temp_rec.Top > 214 && temp_rec.Bottom < (216 + 415)) flag_border = true;
            else if (GamePanel.blocks[i].type == 3 && temp_rec.Top > (215 + 75) && temp_rec.Bottom < (215 + 170) && temp_rec.Left > (120 + 225)) flag_border = true;
            else if (GamePanel.blocks[i].type == 3 && temp_rec.Bottom < (215 + 415 - 75) && temp_rec.Top > (215 + 415 - 170) && temp_rec.Left < (120 + 225)) flag_border = true;
            else flag_border = false;

            return flag_collision && flag_border;
        }


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
