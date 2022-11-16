using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using System.Drawing.Drawing2D;

namespace LabiryntWiedzy
{
    public class GamePanel : Panel
    {
        public int sWidth; //Szerokość pola graficznego gry
        public int sHeight; //Wysokość pola graficznego gry
        public int barHeight; //Wysokość paska menu
        public int gStatus; // zmienna reprezentujaca stan gry  0-stan poczatkowy, rysowanie menu
        public FontFamily fontFamily; //
        public Font menuFont; //Czcionki stosowane w pasku Menu
        public Font alertFont; //Czcionki stosowane jako alert w polu gry

        private Point MouseDownLocation;

        public GamePanel(int width, int height)
        {
            DoubleBuffered = true; // zapobieganie efektu typu blinking
            this.sWidth = width;
            this.sHeight = height;
            barHeight = 50;
            Size = new System.Drawing.Size(sWidth, sHeight);
            Location = new System.Drawing.Point(0, 0);


            fontFamily = new FontFamily("Haettenschweiler");
            menuFont = new Font(fontFamily, 52, FontStyle.Regular, GraphicsUnit.Pixel);
            alertFont = new Font(fontFamily, 32, FontStyle.Regular, GraphicsUnit.Pixel);

        } // koniec GamePanel()



        protected override void OnPaint( PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(GPars.bgImage, new Point(0, 0));
            if (GPars.gameStatus==0)
            {
                Rectangle startMenu = new Rectangle((sWidth-400)/2, (sHeight-400)/2, 400, 400); // prostokat reprezentujacy startowe menu
                g.DrawRectangle(Pens.Crimson, startMenu);
                g.FillRectangle(Brushes.WhiteSmoke, startMenu); // wypelnienie startowego menu
                g.DrawString("Nowa gra", menuFont, Brushes.Black, new Point(420, 225));
                g.DrawString("Wybierz poziom", menuFont, Brushes.Black, new Point(375, 305));
                g.DrawString("O grze...", menuFont, Brushes.Black, new Point(440, 390));
                g.DrawString("Wyjdź z gry", menuFont, Brushes.Black, new Point(400, 475));
            }
            else if (GPars.gameStatus == 1) 
            {
                Rectangle barMenu = new Rectangle(0, 0, sWidth-1, barHeight); // prostokat reprezentujacy pasek menu
                g.DrawRectangle(Pens.Crimson, barMenu); // narysowanie obwodki paska menu
                g.FillRectangle(Brushes.WhiteSmoke, barMenu); // wypelnienie paska menu
                g.DrawString("Menu", menuFont, Brushes.Black, new Point(0, 0));

                klocek.currX = klocek.rec.Left;
                klocek.currY = klocek.rec.Top;
                //g.FillRectangle(Brushes.DeepSkyBlue, klocek.rec);
                g.DrawImage(klocek.icon, klocek.currX, klocek.currY);
            }

        }

        protected override void OnMouseDown( MouseEventArgs e)
        {
            MouseDownLocation = e.Location;

            if (GPars.gameStatus == 0 && e.Button== MouseButtons.Left)
            {
                //Czy wybrano opcję nowa gra w startowym menu
                if (e.X > 439 && e.X < 592 && e.Y > 246 && e.Y < 281)
                {
                    GPars.gameStatus = 1;
                    Invalidate();
                }
                //Czy wybrano opcję o grze... w startowym menu
                if (e.X > 457 && e.X < 587 && e.Y > 411 && e.Y < 455)
                {
                    GPars.bgImage = Image.FromFile("images/bg_1024v2.jpg");
                    Invalidate();
                }
                //Czy wybrano wyjdz z gry w startowym menu
                if (e.X > 400 && e.X < 615 && e.Y < 542 && e.Y > 500)
                {
                    Application.Exit();
                }
            }
            else if (GPars.gameStatus == 1 && e.Button == MouseButtons.Left)
            {
                if (e.X > 19 && e.X < 102 && e.Y > 23 && e.Y < 58)
                {
                    GPars.gameStatus = 0;
                    Invalidate();
                }

            }

        }


        Block klocek = new Block(100, 100, 50, 100, GPars.blocks[1]);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MouseDownLocation.X > klocek.rec.Left && MouseDownLocation.X < (klocek.rec.Left + klocek.width) && MouseDownLocation.Y > klocek.rec.Top && MouseDownLocation.Y < (klocek.rec.Top + klocek.height))
                {
                    klocek.rec.Location = new Point((e.X - MouseDownLocation.X) + klocek.rec.Left, klocek.rec.Top);
                    MouseDownLocation = e.Location;
                    Invalidate();
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (MouseDownLocation.X > klocek.rec.Left && MouseDownLocation.X < (klocek.rec.Left + klocek.width) && MouseDownLocation.Y > klocek.rec.Top && MouseDownLocation.Y < (klocek.rec.Top + klocek.height))
                {
                    klocek.rec.Location = new Point(klocek.rec.Left, (e.Y - MouseDownLocation.Y) + klocek.rec.Top);
                    MouseDownLocation = e.Location;
                    Invalidate();
                }
            }
        }



    }
}
