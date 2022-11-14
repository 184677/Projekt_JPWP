using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;


namespace LabiryntWiedzy
{
    public class GamePanel : Panel
    {
        public int sWidth; //Szerokość pola graficznego gry
        public int sHeight; //Wysokość pola graficznego gry
        public int barHeight; //Wysokość paska menu
        public int gStatus; // zmienna reprezentujaca stan gry  0-stan poczatkowy, rysowanie menu
        public Font menuFont; //Czcionki stosowane w pasku Menu
        public Font alertFont; //Czcionki stosowane jako alert w polu gry
       

        public GamePanel(int width, int height)
        {
            DoubleBuffered = true; // zapobieganie efektu typu blinking
            this.sWidth = width;
            this.sHeight = height;
            barHeight = 50;
            Size = new System.Drawing.Size(sWidth, sHeight);
            Location = new System.Drawing.Point(0, 0);
            
            menuFont = new Font(FontFamily.GenericSansSerif, 32.0F, FontStyle.Bold);
            alertFont = new Font(FontFamily.GenericSansSerif, 92.0F, FontStyle.Bold);
            
            Paint += new System.Windows.Forms.PaintEventHandler(Draw); // rysowanie tla
            MouseClick += new System.Windows.Forms.MouseEventHandler(MouseClicked); // obsluga klikniecia myszka

        }

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle barMenu = new Rectangle(0,0, sWidth-1, barHeight); // prostokat reprezentujacy pasek menu na gorze ekranu
            g.DrawImage(GPars.bgImage, new Point(0, 0));
            g.DrawRectangle(Pens.Black,barMenu); // narysowanie obwodki paska menu
            g.FillRectangle(Brushes.Blue,barMenu); // wypelnienie paska menu
            g.DrawString("Wyjdz z gry", menuFont, Brushes.Red, new Point(0, 0));
            g.DrawString("Zmien tło", menuFont, Brushes.Red, new Point(820, 0));
            //g.DrawImage(GPars.blocks[1], new Point(0, 0));

        }


        private void MouseClicked(object sender, MouseEventArgs e)
        {
            
            //Czy wybrano opcję Menu w pasku gornym
            if (e.X > (sWidth - 150) && e.Y < barHeight)
            {
                GPars.bgImage = Image.FromFile("images/bg_1024v2.jpg");
                Invalidate();
            }
            //Czy wybrano z Menu pozycję Koniec gry
            if (e.X < 300 && e.Y < barHeight)
            {
                Application.Exit();
            }

        }



    }
}
