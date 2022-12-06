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
        private Block[] blocks; // tablica obiektów pierwszego planu - klocki
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

            blocks = new Block[GPars.noOfObjects];

            restartGame();

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

                g.DrawImage(blocks[0].icon, blocks[0].rec.Left, blocks[0].rec.Top);
                g.DrawImage(blocks[1].icon, blocks[1].rec.Left, blocks[1].rec.Top);
                g.DrawImage(blocks[2].icon, blocks[2].rec.Left, blocks[2].rec.Top);
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


        protected override void OnMouseMove(MouseEventArgs e)
        {
            Boolean kolizja;
            for (int i=0; i<3 ;i++)
            {
                if (e.Button == MouseButtons.Left) // poruszanie klockiem w poziomie
                {
                    kolizja = false;
                    if (MouseDownLocation.X > blocks[i].rec.Left && MouseDownLocation.X < (blocks[i].rec.Left + blocks[i].width) && MouseDownLocation.Y > blocks[i].rec.Top && MouseDownLocation.Y < (blocks[i].rec.Top + blocks[i].height))
                    {
                        for (int j = 0; j < 3; j++) // sprawdzenie kolizji
                        {
                            if (i == j) continue;
                            CheckCollisions(blocks[i].rec, blocks[j].rec);
                        }

                        if (blocks[i].type == 3 || blocks[i].type == 2 && kolizja == false)
                        {
                            blocks[i].rec.Location = new Point((e.X - MouseDownLocation.X) + blocks[i].rec.Left, blocks[i].rec.Top);
                            MouseDownLocation = e.Location;
                            Invalidate();
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right) // poruszanie klockiem w pionie
                {
                    kolizja = false;
                    if (MouseDownLocation.X > blocks[i].rec.Left && MouseDownLocation.X < (blocks[i].rec.Left + blocks[i].width) && MouseDownLocation.Y > blocks[i].rec.Top && MouseDownLocation.Y < (blocks[i].rec.Top + blocks[i].height))
                    {
                        for (int j = 0; j < 3; j++) // sprawdzenie kolizji
                        {
                            if (i == j) continue;
                            CheckCollisions(blocks[i].rec, blocks[j].rec);
                        }

                        if (blocks[i].type == 3 || blocks[i].type == 1)
                        {
                            blocks[i].rec.Location = new Point(blocks[i].rec.Left, (e.Y - MouseDownLocation.Y) + blocks[i].rec.Top);
                            MouseDownLocation = e.Location;
                            Invalidate();
                        }
                    }
                }
            }
        }



        public void CheckCollisions(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.IntersectsWith(rect2) == false) return;

            bool touching_right =
            rect1.Right > rect2.Left &&
            rect1.Left < rect2.Left &&
            rect1.Bottom > rect2.Top &&
            rect1.Top < rect2.Bottom;

            bool touching_left =
            rect1.Left < rect2.Right &&
            rect1.Right > rect2.Right &&
            rect1.Bottom > rect2.Top &&
            rect1.Top < rect2.Bottom;

            bool touching_bottom =
            rect1.Bottom > rect2.Top &&
            rect1.Top < rect2.Top &&
            rect1.Right > rect2.Left &&
            rect1.Left < rect2.Right;

            bool touching_top =
            rect1.Top < rect2.Bottom &&
            rect1.Bottom > rect2.Bottom &&
            rect1.Right > rect2.Left &&
            rect1.Left < rect2.Right;

            if (touching_left && !touching_right && !touching_bottom && !touching_top) Console.WriteLine("Collision LEFT side");
            if (!touching_left && touching_right && !touching_bottom && !touching_top) Console.WriteLine("Collision RIGHT side");
            if (!touching_left && !touching_right && !touching_bottom && touching_top) Console.WriteLine("Collision TOP side");
            if (!touching_left && !touching_right && touching_bottom && !touching_top) Console.WriteLine("Collsion BOTTOM side");
            if (!touching_left && touching_right && touching_bottom && !touching_top)
            {
               // Console.WriteLine("Collision right bottom corner");
                if (rect1.Right - rect2.Left > rect1.Bottom - rect2.Top) Console.WriteLine("Touching BOTTOM on corner");
                else Console.WriteLine("Collision RIGHT on corner");
            }
            if (!touching_left && touching_right && !touching_bottom && touching_top)
            {
               // Console.WriteLine("Collision right top corner");
                if (rect1.Right - rect2.Left > rect2.Bottom - rect1.Top) Console.WriteLine("Touching TOP on corner");
                else Console.WriteLine("Collision RIGHT on corner");
            }
            if (touching_left && !touching_right && touching_bottom && !touching_top)
            {
               // Console.WriteLine("Collision left bottom corner");
                if (rect2.Right - rect1.Left > rect1.Bottom - rect2.Top) Console.WriteLine("Touching BOTTOM on corner");
                else Console.WriteLine("Collison LEFT on corner");
            }
            if (touching_left && !touching_right && !touching_bottom && touching_top)
            {
               // Console.WriteLine("Collision left top corner");
                if (rect2.Right - rect1.Left > rect2.Bottom - rect1.Top) Console.WriteLine("Touching TOP on corner");
                else Console.WriteLine("Collision LEFT on corner");
            }
        }



        private void restartGame() // zastanowic sie czy nie lepiej od razu zdefiniowac wszytskie klocki i potem rysowac tylko te ktore chcemy
        {
            blocks[0]= new Block(100, 100, 50, 50, 3, GPars.blocks[0]);
            blocks[1]= new Block(200, 200, 50, 100, 1, GPars.blocks[1]);
            blocks[2] = new Block(400, 400, 100, 50, 2, GPars.blocks[2]);
        }//koniec restartGame()

    }
}
